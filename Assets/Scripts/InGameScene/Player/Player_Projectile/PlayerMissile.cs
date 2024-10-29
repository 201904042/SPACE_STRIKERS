using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissile : PlayerProjectile
{
    private int expDmgRate;
    private float expLiveTime;
    private float expRange;

    public override void SetAddParameter(float value1, float value2 = 0, float value3 = 0, float value4 = 0)
    {
        base.SetAddParameter(value1, value2, value3, value4);
        expDmgRate = (int)value1;
        expLiveTime = value2;
        expRange = value3;
    }


    public override void SetProjParameter(int _projSpeed, int _dmgRate, float _liveTime, float _range)
    {
        base.SetProjParameter(_projSpeed, _dmgRate, _liveTime, _range);
        expDmgRate = PlayerMain.ExplosionBaseDamageRate; //�⺻�� 80 ���⿡ ������ �������� ���غ���
        expRange = PlayerMain.ExplosionBaseRange; //�⺻�� 1 ���⿡ ������ �������� ���غ���
        expLiveTime = PlayerMain.ExplosionBaseLiveTime; //�⺻ 1�� ����. ���� ��������
    }

    protected override void TriggedEnemy(Collider2D collision)
    {
        base.TriggedEnemy(collision);
        PlayerExplosion proj = GameManager.Instance.Pool.GetPlayerProj(PlayerProjType.Explosion, transform.position, transform.rotation).GetComponent<PlayerExplosion>();
        proj.SetProjParameter(0, expDmgRate, expLiveTime, expRange);
        SingleEnemyDamage();
    }

    
}
