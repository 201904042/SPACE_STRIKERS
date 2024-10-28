using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class USkill_Bomber : PlayerProjectile
{
    private float explosionRange;
    private float explosionDamageRate;

    public override void SetProjParameter(int _projSpeed, int _dmgRate, float _liveTime, float _range)
    {
        ResetProj();
        Debug.Log("���� �Ķ���� ����");
        isParameterSet = true;
        isHitOnce = true;
        isShootingObj = true;
        speed = _projSpeed;
        damageRate = _dmgRate;
        finalDamage = finalDamage = (int)playerStat.IG_Dmg * damageRate / 100; //�⺻ ���� ������ ����. ���������� ���� ������
        explosionDamageRate = _dmgRate / 2;

        liveTime = _liveTime;
        explosionRange = _range;
    }

    protected override void TriggedEnemy(Collider2D collision)
    {
        base.TriggedEnemy(collision);
        PlayerExplosion proj = GameManager.Instance.Pool.GetPlayerProj(PlayerProjType.Explosion, transform.position, transform.rotation).GetComponent<PlayerExplosion>();
        proj.OnHitOnce(false);

        proj.SetProjParameter(0, (int)explosionDamageRate, liveTime, explosionRange);
        
        SingleEnemyDamage();

    }

}
