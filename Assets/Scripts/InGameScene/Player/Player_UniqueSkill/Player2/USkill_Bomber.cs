using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class USkill_Bomber : PlayerProjectile
{ // �ʿ� ���� : ������, �����ð�, ũ��, ƽ
    private int expDmgRate;
    private float expLiveTime;
    private float expRange;
    private float expDamageTik;

    public override void SetAddParameter(float value1, float value2 = 0, float value3 = 0,float value4 = 0)
    {
        base.SetAddParameter(value1, value2, value3,value4);
        expDmgRate = (int)value1;
        expLiveTime = value2;
        expRange = value3;
        expDamageTik = value4;
    }

    
    protected override void TriggedEnemy(Collider2D collision)
    {
        base.TriggedEnemy(collision);
        PlayerExplosion proj = GameManager.Instance.Pool.GetPlayerProj(PlayerProjType.Explosion, transform.position, transform.rotation).GetComponent<PlayerExplosion>();
        proj.SetAddParameter(expDamageTik);
        proj.SetProjParameter(0, expDmgRate, expLiveTime, expRange);
        
        SingleEnemyDamage();

    }

}
