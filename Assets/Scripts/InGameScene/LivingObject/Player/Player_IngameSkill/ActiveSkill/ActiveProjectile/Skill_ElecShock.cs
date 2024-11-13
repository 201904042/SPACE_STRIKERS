using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Skill_ElecShock : PlayerProjectile
{

    [SerializeField] protected bool isCycleDamage; //여러번 데미지를 주는 변수 인가 = 장판스킬
    [SerializeField] protected float cycleRate;

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void ResetProj()
    {
        base.ResetProj();
        isCycleDamage = false;
        cycleRate = 0;
        isSlow = false;
        slowRate = 0;
        isSlowExtraDamage = false;
        extraDamageRate = 0;
    }

    private bool isSlow;
    private int slowRate;
    private bool isSlowExtraDamage;
    private int extraDamageRate;

    public override void SetAddParameter(float value1, float value2 =0, float value3 = 0, float value4 = 0)
    {
        base.SetAddParameter(value1, value2, value3, value4);
        if (value1 == 0)
        {
            return;
        }
        isCycleDamage = true;
        cycleRate = value1;

        if (value2 == 0)
        {
            return;
        }
        isSlow = true;
        slowRate = (int)value2;

        if (value3 == 0)
        {
            return;
        }
        isSlowExtraDamage = true;
        extraDamageRate = (int)value3;
    }

    public override void SetProjParameter(int _projSpeed, int _dmgRate, float _liveTime, float _range)
    {
        base.SetProjParameter(_projSpeed, _dmgRate, _liveTime, _range);
        isHitOnce = false;

        
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        
        if (collision.gameObject.tag == "Enemy")
        {
            //적 리뉴얼 후 슬로우 효과 추가
            if (isSlow && !collision.GetComponent<EnemyObject>().isShocked)
            {
                StartCoroutine(ShockingEnemy(collision.GetComponent<EnemyObject>()));
            }
        }
    }

    protected override void TriggedEnemy(Collider2D collision)
    {
        base.TriggedEnemy(collision);
        

        if (damaging == null)
        {
            damaging = StartCoroutine(AreaDamageLogic(cycleRate));
        }
    }

    protected override IEnumerator AreaDamageLogic(float _cycleRate)
    {
        while (true)
        {
            if (hittedEnemyList.Count == 0)
            {
                yield return new WaitForSeconds(_cycleRate);
                continue;
            }
            MultiEnemyDamage(); // 기존 데미지 로직 호출

            yield return new WaitForSeconds(_cycleRate); // 주기적으로 데미지 적용
        }
    }

    private IEnumerator ShockingEnemy(EnemyObject enemy)
    {
        if(enemy.type == EnemyType.Boss)
        {
            yield break ; //보스는 디버프를 받지 않음
        }
        Debug.Log("적 쇼크");
        enemy.isShocked = true;
        enemy.isAttackable = false;
        int enemyMoveSpeed = enemy.moveSpeed;
        enemy.moveSpeed = enemyMoveSpeed /2;
        Color colorInstance = enemy.GetComponent<SpriteRenderer>().color;
        enemy.GetComponent<SpriteRenderer>().color = Color.blue;
        yield return new WaitForSeconds(3);

        enemy.isShocked = false;
        enemy.isAttackable = true;
        enemy.moveSpeed = enemyMoveSpeed;
        enemy.GetComponent<SpriteRenderer>().color = colorInstance;
    }

    protected override void MultiEnemyDamage()
    {
        for (int i = hittedEnemyList.Count - 1; i >= 0; i--)
        {
            if (hittedEnemyList.Count <= 0)
            {
                return;

            }
            GameObject enemy = hittedEnemyList[i];

            if (!enemy.activeSelf)
            {
                hittedEnemyList.RemoveAt(i); // 비활성화된 적 제거
            }
            else
            {
                if (isSlowExtraDamage ) //해당 적이 슬로우 상태라면? 추가뎀
                {
                    enemy.GetComponent<EnemyObject>().EnemyDamaged(gameObject, finalDamage + (finalDamage*(extraDamageRate / 100))); // 적에게 데미지
                }
                else
                {
                    enemy.GetComponent<EnemyObject>().EnemyDamaged(gameObject,finalDamage); // 적에게 데미지
                }
                
                if (isHitOnce)
                {
                    hittedEnemyList.RemoveAt(i);
                }
            }
        }
    }

}
