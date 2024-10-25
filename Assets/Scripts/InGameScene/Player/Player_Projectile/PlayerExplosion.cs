using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerExplosion : PlayerProjectile
{

    protected override void Update()
    {
        //부모 업데이트 내용 제거
    }


    public override void SetProjParameter(int _projSpeed, int _dmgRate, float _liveTime, float _range)
    {
        Debug.Log("메인 파라미터 세팅");
        isParameterSet = true;

        isShootingObj = true;
        speed = _projSpeed;
        damageRate = _dmgRate;
        finalDamage = finalDamage = (int)playerStat.damage * damageRate / 100; //기본 최종 데미지 구조. 수정사항은 개인 덮어쓰기로
        liveTime = _liveTime; //생성된 발사체가 가 이 시간후에 자동으로 파괴됨
        StartCoroutine(LiveTimer(liveTime));
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            TriggedEnemy(collision);
        }
    }

    protected override void TriggedEnemy(Collider2D collision)
    {
        base.TriggedEnemy(collision);

        MultiEnemyDamage();
    }
}

