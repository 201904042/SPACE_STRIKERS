using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class Skill_Missile : PlayerProjectile
{
    private float explosionRange;
    private float explosionDamageRate;

    

    public override void SetProjParameter(int _projSpeed, int _dmgRate, float _liveTime, float _range)
    {
        Debug.Log("메인 파라미터 세팅");
        isParameterSet = true;

        isShootingObj = true;
        speed = _projSpeed;
        damageRate = _dmgRate;
        finalDamage = finalDamage = (int)playerStat.damage * damageRate / 100; //기본 최종 데미지 구조. 수정사항은 개인 덮어쓰기로
        explosionDamageRate = _dmgRate / 2;
        
        explosionRange = _range;
    }

    protected override void TriggedEnemy(Collider2D collision)
    {
        base.TriggedEnemy(collision);
        PlayerExplosion proj = GameManager.Instance.Pool.GetPlayerProj(PlayerProjType.Explosion, transform.position, transform.rotation).GetComponent<PlayerExplosion>();
        proj.SetProjParameter(0, (int)explosionDamageRate, 1, explosionRange);
        SingleEnemyDamage();
        
    }

}
