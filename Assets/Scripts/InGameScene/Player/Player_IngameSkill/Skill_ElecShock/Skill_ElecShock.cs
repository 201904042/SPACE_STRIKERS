using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Skill_ElecShock : PlayerShoot
{
    private float shockDamage;
    private float playerStatDamage;
    public float shockDamageRate;
    public float shockRange;
    private float shockSpeed;
    public float slowRate;
    public float slowTime;
    public bool isExtraDamageToSlowEnemyOn;
    private float damageTime;
    private float damageTimer;

    protected override void Awake()
    {
        base.Awake();
        shockDamageRate = 1.5f;
        shockRange = 1.0f;
        playerStatDamage = playerStat.damage;
        shockSpeed = 1f;
        shockDamage = playerStatDamage * shockDamageRate;

        damageTime = 0.5f;
        damageTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        damageTimer -= Time.deltaTime;
        if (!isFirstSet)
        {
            shockDamage = playerStatDamage * shockDamageRate;
            transform.localScale *= shockRange;
            isFirstSet = true;
        }
        
        transform.position += transform.up * shockSpeed * Time.deltaTime/2;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            if(damageTimer <= 0)
            {
                damageTimer = damageTime;
                if (collision.GetComponent<EnemyObject>() != null)
                {
                    EnemyObject enemy = collision.GetComponent<EnemyObject>();
                    if (enemy.isEnemySlow)
                    {
                        enemy.EnemyDamaged(shockDamage * 2, gameObject);
                    }
                    else
                    {
                        collision.GetComponent<EnemyObject>().EnemyDamaged(shockDamage, gameObject);
                    }

                }
                
            }

            if (!collision.GetComponent<EnemyObject>().isEnemySlow)
            {
                StartCoroutine(getSlow(collision));
            }
           
        }
    }

    IEnumerator getSlow(Collider2D collision)
    {
        EnemyObject enemy = collision.GetComponent<EnemyObject>();
        SpriteRenderer enemySprite = collision.GetComponent<SpriteRenderer>();

        float originalSpeed = enemy.enemyStat.enemyMoveSpeed;

        enemy.isEnemyCanAttack = false;
        enemy.isEnemySlow = true;
        enemy.enemyStat.enemyMoveSpeed *= 1 - slowRate;
        enemySprite.color = new Color(1, 0.5f, 0.5f, 1);
        yield return new WaitForSeconds(slowTime);
        if (enemy != null)
        {
            enemy.isEnemyCanAttack = true;
            enemy.isEnemySlow = false;
            enemy.enemyStat.enemyMoveSpeed = originalSpeed;
            enemySprite.color = new Color(1, 1f, 1f, 1);
        }
        


    }
}
