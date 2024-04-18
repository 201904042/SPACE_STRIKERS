using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class PlayerHoming : PlayerShoot
{
    private bool hashit = false;

    [Header("±âº» ÃÑ¾Ë ½ºÅÝ")]
    [SerializeField]
    private float homing_damage;
    private float homingSpeed;
    [SerializeField]
    private float player_statDamage;
    [SerializeField]
    private float homing_damageRate = 0.5f;

    private Transform target;
    private bool target_set;

    protected override void Awake()
    {
        base.Awake();
        target_set = false;
        player_statDamage = player_stat.damage;
        homingSpeed = 15;
        homing_damage = player_statDamage * homing_damageRate;
    }

    private void Update()
    {
        if (target_set != true)
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
            Destroy(gameObject);
        }
    }

    private void FindEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closeastDistance = Mathf.Infinity;
        Transform neareastEnemy = null;

        foreach(GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position,enemy.transform.position);
            if(distanceToEnemy < closeastDistance)
            {
                closeastDistance = distanceToEnemy;
                neareastEnemy = enemy.transform;
            }
        }
        target = neareastEnemy;
        target_set = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hashit)
        {
            return;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            if (collision.GetComponent<Enemy>() != null)
            {
                collision.GetComponent<Enemy>().Enemydamaged(homing_damage, gameObject);
            }
            hashit = true;
            Destroy(gameObject);
        }

    }
}
