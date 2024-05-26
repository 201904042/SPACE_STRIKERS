using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser_Core : MonoBehaviour
{
    private float damage;

    private void Awake()
    {
        damage = GetComponentInParent<EnemyLaser>().damage;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.GetComponent<PlayerStat>().PlayerDamaged(damage, gameObject);
        }
    }
}
