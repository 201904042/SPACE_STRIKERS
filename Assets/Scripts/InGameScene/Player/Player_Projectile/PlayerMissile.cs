using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissile : PlayerProjectile
{
    private int explosionDamageRate;
    private float explosionRange; // 기본 1 , 1 -> 1.5 -> 2
    private float explosionLiveTime;
   

    public override void SetProjParameter(int _projSpeed, int _dmgRate, float _liveTime, float _range)
    {
        base.SetProjParameter(_projSpeed, _dmgRate, _liveTime, _range);

        explosionDamageRate = LauncherStat.ExplosionBaseDamageRate; //기본은 80 여기에 레벨별 증가비율 곱해보기
        explosionRange = LauncherStat.ExplosionBaseRange; //기본은 1 여기에 레벨별 증가비율 곱해보기
        explosionLiveTime = LauncherStat.ExplosionBaseLiveTime; //기본 1초 고정. 추후 수정사항
    }

    protected override void TriggedEnemy(Collider2D collision)
    {
        base.TriggedEnemy(collision);
        PlayerExplosion proj = GameManager.Instance.Pool.GetPlayerProj(PlayerProjType.Explosion, transform.position, transform.rotation).GetComponent<PlayerExplosion>();
        proj.SetProjParameter(0, explosionDamageRate, explosionLiveTime, explosionRange);
        SingleEnemyDamage();
    }

    
}
