using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using static PlayerjsonReader;

public class Enemy_Common : MonoBehaviour
{
    public GameObject Enemy_Projectile;
    Enemy cur_enemy;
    int proj_num;
    float bulletSpeed = 3f;
    float attack_timeRange;
    float col_stopZone;
    float stop_height; //1이면 상단 2면 중단에 정지 후 공격
    bool move_stop;
    float spacing;
    float attack_Timer;
    bool can_Attack;
    bool isAttacking = false;
    

    private void Awake()
    {
        cur_enemy = GetComponent<Enemy>();
        proj_num_set();
        can_Attack = cur_enemy.Can_Attack;
        stop_height = 1;
        col_stopZone = 0;
        attack_timeRange = cur_enemy.Enemy_MoveAttack ? 2f : 4f;
        attack_Timer = attack_timeRange;
        move_stop = false;

        if(!cur_enemy.Enemy_MoveAttack)
        {
            StartCoroutine(StopEnemyRoutine());
        }
        else
        {
            StartCoroutine(MoveEnemyRoutine());
        }
        
    }

    void proj_num_set()
    {
        if (cur_enemy.Enemy_ID < 10)
        {
            proj_num = 1;
        }
        else if(cur_enemy.Enemy_ID < 20 || cur_enemy.Enemy_ID > 10)
        {
            proj_num = 3;
        }
    }

    private void Update()
    {
        can_Attack = cur_enemy.Can_Attack;
        if (can_Attack)
        {
            attack_Timer -= Time.deltaTime;
        }
       

        if (isAttacking && (attack_Timer <=0))
        {
            if (can_Attack)
            {
                Attack();
                attack_Timer = attack_timeRange;
            }
        }
    }
    void Attack()
    {

        if (cur_enemy.Enemy_IsAiming)
        {
            Transform player = GameObject.Find("Player").transform;

            Vector3 dir = (player.position - transform.position).normalized;
            
            GameObject proj = Instantiate(Enemy_Projectile, transform.position, transform.rotation);
            Enemy_Bullet e_proj = proj.GetComponent<Enemy_Bullet>();
            if(e_proj == null)
            {
                Debug.Log("not have EnemyBullet");
            }
            e_proj.setDamage(cur_enemy.Enemy_Damage);
            Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
            rb.velocity = dir * bulletSpeed;

            

        }
        else
        {

            Vector3 dir = transform.up;
            for (int i = 0; i < proj_num; i++)
            {
                if(proj_num == 1)
                {
                    GameObject proj = Instantiate(Enemy_Projectile, transform.position, transform.rotation);
                    Enemy_Bullet e_proj = proj.GetComponent<Enemy_Bullet>();
                    if (e_proj == null)
                    {
                        Debug.Log("not have EnemyBullet");
                    }
                    e_proj.setDamage(cur_enemy.Enemy_Damage);
                    Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
                    rb.velocity = dir * bulletSpeed;
                }
                else if(proj_num%2==1)//홀수 발사
                {
                    spacing = 0.2f;
                    GameObject proj = Instantiate(Enemy_Projectile, transform.position + new Vector3(-spacing * (proj_num / 2) + spacing * i, 0f, 0f), transform.rotation);
                    Enemy_Bullet e_proj = proj.GetComponent<Enemy_Bullet>();
                    if (e_proj == null)
                    {
                        Debug.Log("not have EnemyBullet");
                    }
                    e_proj.setDamage(cur_enemy.Enemy_Damage);
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
            yield return new WaitForSeconds(10f); //10초간 멈춰서 공격
            isAttacking = false;
            move_stop = false;
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
        while (!move_stop)
        {
            transform.Translate(Vector3.up * cur_enemy.Enemy_MoveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy_StopZone")
        {
            col_stopZone += 1;
            if (col_stopZone == stop_height&& !cur_enemy.Enemy_MoveAttack)
            {
                move_stop = true;
            }
        }
    }
}
