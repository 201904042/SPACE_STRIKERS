using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class Skill_Homing : PlayerShoot
{
    GameObject target;
    GameObject[] enemies;

    public float homing_damage;
    private float player_statDamage;
    public float homing_damageRate;
    private float homing_speed;

    protected override void Awake()
    {
        base.Awake();
        player_statDamage = player_stat.damage;
        homing_speed = 15;
        homing_damageRate = 0.8f;
        setTarget();
    }
    void Update()
    {
        if(!is_firstSet)
        {
            homing_damage = player_statDamage * homing_damageRate;
            is_firstSet = true;
        }

        if (target != null)
        {
            Vector2 direction = target.transform.position - transform.position;
            transform.up = direction;
            transform.position += transform.up * homing_speed * Time.deltaTime;
        }
        else
        {
            transform.position += transform.up * homing_speed * Time.deltaTime;
        }
    }

    void setTarget()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if(enemies.Length == 0)
        {
            return;
        }
        target = enemies[Random.Range(0, enemies.Length)];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "Enemy")
        {
            if (collision.GetComponent<Enemy>() != null)
            {
                collision.GetComponent<Enemy>().Enemydamaged(homing_damage, gameObject);
            }
            Destroy(gameObject);
        }

    }
}
