using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class PlayerBullet : PlayerShoot
{
    private bool hashit = false;

    [Header("±âº» ÃÑ¾Ë ½ºÅÝ")]
    [SerializeField]
    private float bullet_damage;
    [SerializeField]
    private float player_statDamage;
    [SerializeField]
    private float bullet_damageRate = 1f;

    protected override void Awake()
    {
        base.Awake ();
        player_statDamage = player_stat.damage;
        bullet_damage = player_statDamage * bullet_damageRate;
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
                collision.GetComponent<Enemy>().Enemydamaged(bullet_damage, gameObject);
            }
            
            hashit = true;
            Destroy(gameObject);
        }
    }
}
