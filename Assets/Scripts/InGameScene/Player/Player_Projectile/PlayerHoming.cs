using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class PlayerHoming : PlayerShoot
{

    [Header("±âº» ÃÑ¾Ë ½ºÅÝ")]
    [SerializeField]
    private float homingDamage;
    private float homingSpeed;
    [SerializeField]
    private float playerStatDamage;
    [SerializeField]
    private float homingDamageRate = 0.5f;

    private Transform target;
    private bool targetSet;

    protected override void Awake()
    {
        base.Awake();
        targetSet = false;
        playerStatDamage = playerStat.damage;
        homingSpeed = 15;
        homingDamage = playerStatDamage * homingDamageRate;
    }

    protected override void OnEnable()
    {
        Init();
    }

    protected override void Init()
    {
        base.Init();

    }


    private void FindEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closeastDistance = Mathf.Infinity;
        Transform neareastEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < closeastDistance)
            {
                closeastDistance = distanceToEnemy;
                neareastEnemy = enemy.transform;
            }
        }
        target = neareastEnemy;
        targetSet = true;
    }

    private void Update()
    {
        if (targetSet != true)
        {
            FindEnemy();
        }

        if(target != null)
        {
            Vector2 direction = target.position - transform.position;
            transform.up = direction;

            Rigidbody2D rigid = transform.GetComponent<Rigidbody2D>();
            rigid.velocity = direction * homingSpeed;
        }
        else
        {
            PoolManager.poolInstance.ReleasePool(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHit)
        {
            return;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            if (collision.GetComponent<EnemyObject>() != null)
            {
                collision.GetComponent<EnemyObject>().EnemyDamaged(homingDamage, gameObject);
            }
            hasHit = true;
            PoolManager.poolInstance.ReleasePool(gameObject);
        }
    }
}
