using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Enemy_Common : EnemyAct
{
    [Header("커먼 스텟")]
    public bool isAttack;
    public bool isMoveforward;

    [SerializeField]private int curProjNum;
    [SerializeField] private float curProjAngle;
    private int stopTrigCount;
    private int stopCount;
    private float enemyAttackTime;
    private float enemyAttackDealy;

    private bool isAttackCoroutineActive = false;

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Init();
    }

    private void Init()
    {
        isMoveforward = true;
        stopTrigCount = 0;
        stopCount = Random.Range(1, 3);
        enemyAttackTime = 10;
        enemyAttackDealy = 2;
        SetAttackPref();
        if(enemyStat.enemyMoveAttack == true)
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
                StartCoroutine(AttackRepeatly(100f,2f));
                isAttackCoroutineActive = true;
            }
        }
        else
        {
            if(isMoveforward)
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
        curProjNum = 1;
        curProjAngle = 45;
    }

    private void Attack()
    {
        Debug.Log("공격");
        MultiShot(curProjNum, curProjAngle,enemyStat.enemyAttackSpeed, enemyStat.isEnemyAiming);
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
        isMoveforward = true;
        isAttackCoroutineActive= false;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Enemy_StopZone"))
        {
            stopTrigCount++;
            if (stopCount == stopTrigCount)
            {
                isMoveforward = false;
                EnemyMoveStop(gameObject);
            }
        }
    }
}
