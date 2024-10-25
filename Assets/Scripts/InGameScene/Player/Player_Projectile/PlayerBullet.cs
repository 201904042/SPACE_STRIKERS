using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class PlayerBullet : PlayerProjectile
{
    public override void SetProjParameter(int _projSpeed, int _dmgRate, float _liveTime, float _range)
    {
        Debug.Log("메인 파라미터 세팅");
        isParameterSet = true;

        isShootingObj = true;
        speed = _projSpeed;
        damageRate = _dmgRate;
        finalDamage = finalDamage = (int)playerStat.damage * damageRate / 100; //기본 최종 데미지 구조. 수정사항은 개인 덮어쓰기로
    }

    protected override void TriggedEnemy(Collider2D collision)
    {
        base.TriggedEnemy(collision);
        SingleEnemyDamage();
    }

}
