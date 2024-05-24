using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class PlayerBullet : PlayerShoot
{
    private bool hashit = false;

    [Header("±âº» ÃÑ¾Ë ½ºÅÝ")]
    [SerializeField]
    private float bulletDamage;
    [SerializeField]
    private float playerStatDamage;
    [SerializeField]
    private float bulletDamageRate = 1f;

    protected override void Awake()
    {
        base.Awake ();
        playerStatDamage = playerStat.damage;
        bulletDamage = playerStatDamage * bulletDamageRate;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hashit)
        {
            return;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            if (collision.GetComponent<EnemyObject>() != null)
            {
                collision.GetComponent<EnemyObject>().EnemyDamaged(bulletDamage, gameObject);
            }
            
            hashit = true;
            Destroy(gameObject);
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
    }
}
