using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Enemy_Common : EnemyAct
{
    private EnemyObject curEnemyObj;
    private GameObject cummonEnemyAttackPref;
    private int curProjNum;
    private float curProjAngle;

    public bool isAttack;
    public bool isMoveforward;

    private int stopTrigCount;
    private int stopCount;

    private float enemyAttackTime;
    private float enemyAttackDealy;

    private bool isAttackCoroutineActive = false;

    protected override void Awake()
    {
        base.Awake();
        curEnemyObj = GetComponent<EnemyObject>();
        isMoveforward = true;
        stopTrigCount = 0;
        stopCount = Random.Range(1, 4);
        enemyAttackTime = 10;
        enemyAttackDealy = 2;
        SetAttackPref();
    }

    private void Update()
    {
        if(curEnemyObj.enemyStat.enemyMoveAttack)
        {
            //�����̸� �����ϴ� ��
            enemyMoveForward(gameObject);
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
                enemyMoveForward(gameObject);
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
        //�Ϲ� ���� �� źȯ�� �Ϲ�ź���� �п�ź���� ����
        //�п�ź�̸� ����� �п�ź����

        switch (curEnemyObj.enemyStat.enemyId)
        {
            case 1:
            case 2:
                {
                    cummonEnemyAttackPref = enemyBullet;
                    curProjNum = 1;
                    curProjAngle = 45;
                    break;
                }
        }
    }

    private void Attack()
    {
        MultiShot(cummonEnemyAttackPref, curProjNum, curProjAngle, 
            curEnemyObj.enemyStat.enemyAttackSpeed, curEnemyObj.enemyStat.isEnemyAiming);
    }

    private IEnumerator AttackRepeatly(float attackTime, float attackDelay)
    {
        float timer = 0f;
        while (timer < attackTime)
        {
            if (curEnemyObj.isEnemyCanAttack)
            {
                Attack();
            }
            yield return new WaitForSeconds(attackDelay);
            timer += attackDelay;
        }
        isMoveforward = true;
        isAttackCoroutineActive= false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy_StopZone"))
        {
            if (stopCount == ++stopTrigCount)
            {
                isMoveforward = false;
                enemyMoveStop(gameObject);
            }
        }
    }
}
