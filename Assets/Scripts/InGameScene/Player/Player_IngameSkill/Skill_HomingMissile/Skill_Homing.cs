using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class Skill_Homing : PlayerShoot
{
    GameObject target;
    GameObject[] enemies;

    public float homingDamage;
    private float playerStatDamage;
    public float homingDamageRate;
    private float homingSpeed;

    protected override void Awake()
    {
        base.Awake();
        playerStatDamage = playerStat.damage;
        homingSpeed = 15;
        homingDamageRate = 0.8f;
    }

    protected override void OnEnable()
    {
        Init();

        SetTarget();
    }

    protected override void Init()
    {
        base.Init();
        homingDamage = playerStatDamage * homingDamageRate;
    }


    void Update()
    {
        if (target != null && target.activeSelf == true)
        {
            Vector2 direction = target.transform.position - transform.position;
            transform.up = direction;
            transform.position += transform.up * homingSpeed * Time.deltaTime;
        }
        else
        {
            transform.position += transform.up * homingSpeed * Time.deltaTime;
        }
    }

    private void SetTarget()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if(SpawnManager.spawnInstance.activeEnemyList.Count == 0)
        {
            return;
        }
        target = SpawnManager.spawnInstance.activeEnemyList[Random.Range(0, SpawnManager.spawnInstance.activeEnemyList.Count)];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "Enemy")
        {
            if (collision.GetComponent<EnemyObject>() != null)
            {
                collision.GetComponent<EnemyObject>().EnemyDamaged(homingDamage, gameObject);
            }
            PoolManager.poolInstance.ReleasePool(gameObject);
        }

    }
}
