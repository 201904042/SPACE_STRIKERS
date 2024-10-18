using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Enemy_Common : EnemyObject
{
    private int curProjNum; //���� �߻�ü ����
    private float curProjAngle; //���� �߻�ü�� ����

    private int stopTrigCount; //��� ��ž������ ����ؾ� ������
    
    private float enemyAttackTime; //���� �����ϴ� �ð�
    private float enemyAttackDealy; //���ݰ� ���� ������ ������

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
        enemyAttackTime = 10;
        enemyAttackDealy = 2;
        SetAttackPref();

        if(enemyStat.isStop == true)
        {
            isAttackReady = true;
        }
    }

    protected override void Update()
    {
        base.Update();

        if (enemyStat.isStop)
        {
            //�����̸� �����ϴ� ��
            EnemyAct.EnemyMoveForward(gameObject);
            if (!isAttackCoroutineActive)
            {
                StartCoroutine(AttackRepeatly(100f,2f));
                isAttackCoroutineActive = true;
            }
        }
        else
        {
            if(isMove)
            {
                EnemyAct.EnemyMoveForward(gameObject);
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
        curProjNum = 1;
        curProjAngle = 45;
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
        isAttackCoroutineActive= false;
    }

    private void Attack()
    {

        EnemyAct.BulletAttack(this,curProjNum, curProjAngle, enemyStat.attackSpeed, enemyStat.isAim);
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
                EnemyAct.EnemyMoveStop(gameObject);
            }
        }
    }
}
