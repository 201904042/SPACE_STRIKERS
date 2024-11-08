using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser_Core : MonoBehaviour
{
    private float damage => GetComponentInParent<EnemyLaser>().finalDamage;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.GetComponent<PlayerStat>().PlayerDamaged(damage, gameObject);
        }
    }
}
