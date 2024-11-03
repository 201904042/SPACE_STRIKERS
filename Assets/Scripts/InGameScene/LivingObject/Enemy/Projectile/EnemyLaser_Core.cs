using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser_Core : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject.CompareTag("Player"))
        //{
        //    collision.GetComponent<PlayerStat>().PlayerDamaged(GetComponentInParent<EnemyLaser>().damage, gameObject);
        //}
    }
}
