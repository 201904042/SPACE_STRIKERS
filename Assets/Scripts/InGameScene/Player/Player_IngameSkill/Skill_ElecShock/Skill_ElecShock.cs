using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Skill_ElecShock : PlayerShoot
{

    private float damage;
    public float shockDamageRate;
    public float shockRange;
    private float shockSpeed;
    public float slowRate;
    public float slowTime;

    public bool isExtraDamageToSlowEnemyOn;

    public List<GameObject> hittedEnemy;
    private SkillElecShockLauncher elecShockLauncher;
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
        hittedEnemy= new List<GameObject>();

        launcher = GameObject.Find("skill_ElecShockLauncher");
        elecShockLauncher = launcher.GetComponent<SkillElecShockLauncher>();

        shockDamageRate = elecShockLauncher.damageRate;
        shockRange = elecShockLauncher.shockRange;
        slowRate = elecShockLauncher.slowRate;
        slowTime = elecShockLauncher.slowTime;
        isExtraDamageToSlowEnemyOn = elecShockLauncher.isExtraDamageToSlowEnemy;
        damage = playerStat.damage * shockDamageRate;
        transform.localScale *= shockRange;
    }

    private void OnDisable()
    {
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
            if (hittedEnemy.Contains(collision.gameObject) == false)
            {
                if (enemy != null)
                {
                    if (enemy.isEnemySlow)
                    {
                        enemy.EnemyDamaged(damage * 2, gameObject);
                    }
                    else
                    {
                        enemy.EnemyDamaged(damage, gameObject);
                    }
                    hittedEnemy.Add(collision.gameObject);
                }

                if (!enemy.isEnemySlow)
                {
                    StartCoroutine(SlowEnemy(collision));
                }
            }
        }
    }

    private IEnumerator SlowEnemy(Collider2D collision)
    {
        EnemyObject enemy = collision.GetComponent<EnemyObject>();
        SpriteRenderer enemySprite = collision.GetComponent<SpriteRenderer>();

        float originalSpeed = enemy.enemyStat.enemyMoveSpeed;

        enemy.isAttackReady = false;
        enemy.isEnemySlow = true;
        enemy.enemyStat.enemyMoveSpeed = enemy.enemyStat.enemyMoveSpeed *(1 - slowRate);
        enemySprite.color = new Color(0.5f, 0.5f, 1f, 1);

        yield return new WaitForSeconds(slowTime);

        if (enemy.gameObject.activeSelf != false && enemy.isEnemySlow == true)
        {
            enemy.isAttackReady = true;
            enemy.isEnemySlow = false;
            enemy.enemyStat.enemyMoveSpeed = originalSpeed;
            enemySprite.color = new Color(1, 1f, 1f, 1);
        }
    }
}
