using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissile : PlayerProjectile
{
    private int explosionDamageRate;
    private float explosionRange; // �⺻ 1 , 1 -> 1.5 -> 2
    private float explosionLiveTime;
   

    public override void SetProjParameter(int _projSpeed, int _dmgRate, float _liveTime, float _range)
    {
        base.SetProjParameter(_projSpeed, _dmgRate, _liveTime, _range);
        explosionDamageRate = PlayerMain.ExplosionBaseDamageRate; //�⺻�� 80 ���⿡ ������ �������� ���غ���
        explosionRange = PlayerMain.ExplosionBaseRange; //�⺻�� 1 ���⿡ ������ �������� ���غ���
        explosionLiveTime = PlayerMain.ExplosionBaseLiveTime; //�⺻ 1�� ����. ���� ��������
    }

    protected override void TriggedEnemy(Collider2D collision)
    {
        base.TriggedEnemy(collision);
        PlayerExplosion proj = GameManager.Instance.Pool.GetPlayerProj(PlayerProjType.Explosion, transform.position, transform.rotation).GetComponent<PlayerExplosion>();
        proj.OnHitOnce(true);
        proj.SetProjParameter(0, explosionDamageRate, explosionLiveTime, explosionRange);
        SingleEnemyDamage();
    }

    
}
