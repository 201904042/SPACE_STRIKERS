using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Skill_ElecShock : PlayerProjectile
{

    private float damage;
    public float shockDamageRate;
    public float shockRange;
    private float shockSpeed;
    public float slowRate;
    public float slowTime;

    public bool isExtraDamageToSlowEnemyOn;

    protected override void Awake()
    {
        base.Awake();
        shockSpeed = 1;
    }
    protected override void OnEnable()
    {
        Init();
    }

    protected override void Init()
    {
        base.Init();

        damage = playerStat.damage * shockDamageRate;
        transform.localScale *= shockRange;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        transform.localScale /= shockRange;
    }


    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * shockSpeed * Time.deltaTime;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            EnemyObject enemy = collision.GetComponent<EnemyObject>();
            if (enemy != null)
            {
                if (enemy.MakeEnemyShocked) //쇼크 상태일때 트리거 작동시 2배의 데미지를 줌
                {
                    enemy.EnemyDamaged(damage * 2, gameObject);
                }
                else
                {
                    enemy.EnemyDamaged(damage, gameObject);
                    if (enemy.enemyStat.type != 4)
                    {
                        StartCoroutine(SlowEnemy(collision));
                    }
                }
            }
        }
    }

    private IEnumerator SlowEnemy(Collider2D collision)
    {
        
        EnemyObject enemy = collision.GetComponent<EnemyObject>();
        SpriteRenderer enemySprite = collision.GetComponent<SpriteRenderer>();

       float originalSpeed = enemy.enemyStat.moveSpeed;

        enemy.MakeEnemyShocked = true;
        enemy.enemyStat.moveSpeed = enemy.enemyStat.moveSpeed *(1 - slowRate);

        yield return new WaitForSeconds(slowTime);

        if (enemy.gameObject.activeSelf != false && enemy.MakeEnemyShocked == true)
        {
            enemy.MakeEnemyShocked = false;
            enemy.enemyStat.moveSpeed = originalSpeed;
        }
    }
}
