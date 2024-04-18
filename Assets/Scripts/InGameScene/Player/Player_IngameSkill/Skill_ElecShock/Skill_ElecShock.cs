using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Skill_ElecShock : PlayerShoot
{
    private float shock_damage;
    private float player_statDamage;
    public float shock_damageRate;
    public float shock_range;
    private float shock_speed;
    public float slowRate;
    public float slowTime;
    public bool is_ExtraDamageToSlowEnemyOn;
    private float damageTime;
    private float damageTimer;

    protected override void Awake()
    {
        base.Awake();
        shock_damageRate = 1.5f;
        shock_range = 1.0f;
        player_statDamage = player_stat.damage;
        shock_speed = 1f;
        shock_damage = player_statDamage * shock_damageRate;

        damageTime = 0.5f;
        damageTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        damageTimer -= Time.deltaTime;
        if (!is_firstSet)
        {
            shock_damage = player_statDamage * shock_damageRate;
            transform.localScale *= shock_range;
            is_firstSet = true;
        }
        
        transform.position += transform.up * shock_speed * Time.deltaTime/2;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            if(damageTimer <= 0)
            {
                damageTimer = damageTime;
                if (collision.GetComponent<Enemy>() != null)
                {
                    Enemy enemy = collision.GetComponent<Enemy>();
                    if (enemy.is_Slow)
                    {
                        enemy.Enemydamaged(shock_damage * 2, gameObject);
                    }
                    else
                    {
                        collision.GetComponent<Enemy>().Enemydamaged(shock_damage, gameObject);
                    }

                }
                
            }

            if (!collision.GetComponent<Enemy>().is_Slow)
            {
                StartCoroutine(getSlow(collision));
            }
           
        }
    }

    IEnumerator getSlow(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        SpriteRenderer enemySprite = collision.GetComponent<SpriteRenderer>();

        float original_speed = enemy.Enemy_MoveSpeed;

        enemy.Can_Attack = false;
        enemy.is_Slow = true;
        enemy.Enemy_MoveSpeed *= 1 - slowRate;
        enemySprite.color = new Color(1, 0.5f, 0.5f, 1);
        yield return new WaitForSeconds(slowTime);
        if (enemy != null)
        {
            enemy.Can_Attack = true;
            enemy.is_Slow = false;
            enemy.Enemy_MoveSpeed = original_speed;
            enemySprite.color = new Color(1, 1f, 1f, 1);
        }
        


    }
}
