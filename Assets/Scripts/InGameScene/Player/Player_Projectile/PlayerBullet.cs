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

        isShootingObj = true;
    }


    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BulletBorder")
        {
            GameManager.Instance.Pool.ReleasePool(gameObject);
        }
        if (isHitOnce)
        {
            return;
        }

        if (collision.gameObject.tag == "Enemy")
        {
            if (collision.GetComponent<EnemyObject>() != null)
            {
                collision.GetComponent<EnemyObject>().EnemyDamaged(bulletDamage, gameObject);
            }

            isHitOnce = true;
            //Destroy(gameObject);
            GameManager.Instance.Pool.ReleasePool(gameObject);
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
    }
}
