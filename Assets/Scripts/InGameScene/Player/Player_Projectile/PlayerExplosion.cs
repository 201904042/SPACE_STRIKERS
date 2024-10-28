using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerExplosion : PlayerProjectile
{

    protected override void Update()
    {
        //부모 업데이트 내용 제거
    }

    /// <summary>
    /// t: 데미지를 적용하면 리스트에서 제거  f:데미지 적용해도 리스트에서 제거안함
    /// </summary>
    /// <param name="tf"></param>
    public void OnHitOnce(bool tf)
    {
        isHitOnce = tf;
    }

    public override void SetProjParameter(int _projSpeed, int _dmgRate, float _liveTime, float _range)
    {
        base.SetProjParameter(_projSpeed, _dmgRate, _liveTime , _range);
        Debug.Log("메인 파라미터 세팅");
        isParameterSet = true;
        isShootingObj = true; //true를 하지 않으면 플레이어 위치로 가버리니 주의 speed만 0이 되도록 -> 생성된 위치에 그대로 라이브타임만큼 존재
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

        if (isHitOnce)
        {
            MultiEnemyDamage();
        }
        else
        {
            if (damaging == null)
            {
                damaging = StartCoroutine(AreaDamageLogic(0.5f));
            }
        }
        

    }
}

