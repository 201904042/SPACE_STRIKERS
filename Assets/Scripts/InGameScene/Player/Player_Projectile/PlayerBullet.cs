using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class PlayerBullet : PlayerProjectile
{
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

    protected override void OnEnable()
    {
        Init();
    }

    protected override void Init()
    {
        base.Init();

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
                collision.GetComponent<EnemyObject>().EnemyDamaged(bulletDamage, gameObject);
            }

            hasHit = true;
            //Destroy(gameObject);
            PoolManager.poolInstance.ReleasePool(gameObject);
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
    }
}
