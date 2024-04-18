using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bullet : MonoBehaviour
{
    public float damage;
    private PlayerStat p_stat;

    private void Awake()
    {
        p_stat = GameObject.Find("Player").GetComponent<PlayerStat>();
    }

    public void setDamage(float e_damage)
    {
        damage = e_damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            p_stat.Player_damaged(damage, gameObject);
            Destroy(gameObject);
        }
        if (collision.transform.tag == "BulletBorder")
        {
            Destroy(gameObject);
        }
    }

}
