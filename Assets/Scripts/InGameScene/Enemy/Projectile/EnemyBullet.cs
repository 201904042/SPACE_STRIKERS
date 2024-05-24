using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float damage;
    private PlayerStat playerStat;

    private void Awake()
    {
        playerStat = GameObject.Find("Player").GetComponent<PlayerStat>();
    }

    public void setDamage(float e_damage)
    {
        damage = e_damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            playerStat.PlayerDamaged(damage, gameObject);
            Destroy(gameObject);
        }
        if (collision.transform.tag == "BulletBorder")
        {
            Destroy(gameObject);
        }
    }

}
