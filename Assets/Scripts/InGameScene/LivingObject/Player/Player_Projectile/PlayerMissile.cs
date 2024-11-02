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
        expDmgRate = PlayerMain.ExplosionBaseDamageRate; //기본은 80 여기에 레벨별 증가비율 곱해보기
        expRange = PlayerMain.ExplosionBaseRange; //기본은 1 여기에 레벨별 증가비율 곱해보기
        expLiveTime = PlayerMain.ExplosionBaseLiveTime; //기본 1초 고정. 추후 수정사항
    }

    protected override void TriggedEnemy(Collider2D collision)
    {
        base.TriggedEnemy(collision);
        PlayerExplosion proj = GameManager.Game.Pool.GetPlayerProj(PlayerProjType.Explosion, transform.position, transform.rotation).GetComponent<PlayerExplosion>();
        proj.SetProjParameter(0, expDmgRate, expLiveTime, expRange);
        SingleEnemyDamage();
    }

    
}
