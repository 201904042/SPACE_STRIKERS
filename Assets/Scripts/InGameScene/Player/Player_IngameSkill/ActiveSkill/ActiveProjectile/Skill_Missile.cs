using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class Skill_Missile : PlayerProjectile
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
        ResetProj();
        Debug.Log("메인 파라미터 세팅");
        isParameterSet = true;
        
        isShootingObj = true;
        speed = _projSpeed;
        damageRate = _dmgRate;
        finalDamage = finalDamage = (int)playerStat.IG_Dmg * damageRate / 100; //기본 최종 데미지 구조. 수정사항은 개인 덮어쓰기로
    }

    protected override void TriggedEnemy(Collider2D collision)
    {
        base.TriggedEnemy(collision);
        PlayerExplosion proj = GameManager.Game.Pool.GetPlayerProj(PlayerProjType.Explosion, transform.position, transform.rotation).GetComponent<PlayerExplosion>();
        proj.SetProjParameter(0, expDmgRate, expLiveTime, expRange);
        SingleEnemyDamage();
        
    }

}
