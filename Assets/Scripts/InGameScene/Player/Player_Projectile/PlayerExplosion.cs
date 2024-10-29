using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerExplosion : PlayerProjectile
{
    // 필요 변수 : 데미지, 생성시간, 크기, 틱

    public float tikDelay;

    protected override void Update()
    {
        //부모 업데이트 내용 제거
    }

    public override void SetAddParameter(float value1, float value2 = 0, float value3 = 0, float value4 = 0)
    {
        base.SetAddParameter(value1, value2, value3, value4);
        if(value1 != 0) //tik 설정
        {
            tikDelay = value1;
            isHitOnce = false;
        }
        else
        {
            isHitOnce = true ;
        }
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
                damaging = StartCoroutine(AreaDamageLogic(tikDelay));
            }
        }
    }
}

