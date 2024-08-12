using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Elite : EnemyAct
{
    [SerializeField] private int curProjNum;
    [SerializeField] private float curProjAngle;
    private int stopTrigCount;
    private int stopCount;
    private float enemyAttackTime;
    private float enemyAttackDealy;

    private bool isAttackCoroutineActive = false;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        Init();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Init();
    }

    private void Init()
    {
        isMove = true;
        stopTrigCount = 0;
        stopCount = Random.Range(1, 3);
        enemyAttackTime = 10;
        enemyAttackDealy = 2;
        SetAttackPref();
        if (enemyStat.enemyMoveAttack == true)
        {
            isAttackReady = true;
        }
    }

    protected override void Update()
    {
        base.Update();

        if (enemyStat.enemyMoveAttack)
        {
            //움직이며 공격하는 몹
            EnemyMoveForward(gameObject);
            if (!isAttackCoroutineActive)
            {
                StartCoroutine(AttackRepeatly(100f, 2f));
                isAttackCoroutineActive = true;
            }
        }
        else
        {
            if (isMove)
            {
                EnemyMoveForward(gameObject);
            }
            else
            {
                if (!isAttackCoroutineActive)
                {
                    StartCoroutine(AttackRepeatly(enemyAttackTime, enemyAttackTime / enemyAttackDealy));
                    isAttackCoroutineActive = true;
                }
            }
        }
    }

    private void SetAttackPref()
    {
        //일반 적이 쏠 탄환이 일반탄인지 분열탄인지 결정
        //분열탄이면 몇게의 분열탄인지
        curProjNum = 3;
        curProjAngle = 45;
    }

    private void Attack()
    {
        Debug.Log("공격");
        MultiShot(curProjNum, curProjAngle, enemyStat.enemyAttackSpeed, enemyStat.isEnemyAiming);
    }

    private IEnumerator AttackRepeatly(float attackTime, float attackDelay)
    {
        float timer = 0f;
        while (timer < attackTime)
        {
            if (isAttackReady)
            {
                Attack();
            }
            yield return new WaitForSeconds(attackDelay);
            timer += attackDelay;
        }
        isMove = true;
        isAttackCoroutineActive = false;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Enemy_StopZone"))
        {
            stopTrigCount++;
            if (stopCount == stopTrigCount)
            {
                isMove = false;
                EnemyMoveStop(gameObject);
            }
        }
    }
}
