using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;


public class Enemy_Common : MonoBehaviour
{
    public GameObject enemyProjectile;
    EnemyObject curEnemy;
    int projNum;
    float bulletSpeed = 3f;
    float attackTimeRange;
    float colStopZone;
    float stopHeight; //1�̸� ��� 2�� �ߴܿ� ���� �� ����
    bool moveStop;
    float spacing;
    float attackTimer;
    bool isEnemyAttackable;
    bool isAttacking = false;
    

    private void Awake()
    {
        curEnemy = GetComponent<EnemyObject>();
        projNumSet();
        isEnemyAttackable = curEnemy.attackable;
        stopHeight = 1;
        colStopZone = 0;
        attackTimeRange = curEnemy.enemyMoveAttack ? 2f : 4f;
        attackTimer = attackTimeRange;
        moveStop = false;

        if(!curEnemy.enemyMoveAttack)
        {
            StartCoroutine(StopEnemyRoutine());
        }
        else
        {
            StartCoroutine(MoveEnemyRoutine());
        }
        
    }

    void projNumSet()
    {
        if (curEnemy.curEnemyId < 10)
        {
            projNum = 1;
        }
        else if(curEnemy.curEnemyId < 20 || curEnemy.curEnemyId > 10)
        {
            projNum = 3;
        }
    }

    private void Update()
    {
        isEnemyAttackable = curEnemy.attackable;
        if (isEnemyAttackable)
        {
            attackTimer -= Time.deltaTime;
        }
       

        if (isAttacking && (attackTimer <=0))
        {
            if (isEnemyAttackable)
            {
                Attack();
                attackTimer = attackTimeRange;
            }
        }
    }
    void Attack()
    {

        if (curEnemy.enemyIsAiming)
        {
            Transform player = GameObject.Find("Player").transform;

            Vector3 dir = (player.position - transform.position).normalized;
            
            GameObject proj = Instantiate(enemyProjectile, transform.position, transform.rotation);
            EnemyBullet e_proj = proj.GetComponent<EnemyBullet>();
            if(e_proj == null)
            {
                Debug.Log("not have EnemyBullet");
            }
            e_proj.setDamage(curEnemy.enemyDamage);
            Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
            rb.velocity = dir * bulletSpeed;

            

        }
        else
        {

            Vector3 dir = transform.up;
            for (int i = 0; i < projNum; i++)
            {
                if(projNum == 1)
                {
                    GameObject proj = Instantiate(enemyProjectile, transform.position, transform.rotation);
                    EnemyBullet e_proj = proj.GetComponent<EnemyBullet>();
                    if (e_proj == null)
                    {
                        Debug.Log("not have EnemyBullet");
                    }
                    e_proj.setDamage(curEnemy.enemyDamage);
                    Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
                    rb.velocity = dir * bulletSpeed;
                }
                else if(projNum%2==1)//Ȧ�� �߻�
                {
                    spacing = 0.2f;
                    GameObject proj = Instantiate(enemyProjectile, transform.position + new Vector3(-spacing * (projNum / 2) + spacing * i, 0f, 0f), transform.rotation);
                    EnemyBullet e_proj = proj.GetComponent<EnemyBullet>();
                    if (e_proj == null)
                    {
                        Debug.Log("not have EnemyBullet");
                    }
                    e_proj.setDamage(curEnemy.enemyDamage);
                    Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
                    rb.velocity = dir * bulletSpeed;
                }

            }
        }

        }
    IEnumerator StopEnemyRoutine()
    {
        while (true)
        {
            yield return StartCoroutine(move_down());
            StopCoroutine(move_down());
            isAttacking = true;
            yield return new WaitForSeconds(10f); //10�ʰ� ���缭 ����
            isAttacking = false;
            moveStop = false;
        }
    }

    IEnumerator MoveEnemyRoutine()
    {
        isAttacking = true;
        while (true)
        {
            yield return StartCoroutine(move_down());
        }
    }

    IEnumerator move_down()
    {
        while (!moveStop)
        {
            transform.Translate(Vector3.up * curEnemy.enemyMoveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyStopZone")
        {
            colStopZone += 1;
            if (colStopZone == stopHeight&& !curEnemy.enemyMoveAttack)
            {
                moveStop = true;
            }
        }
    }
}
