using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float damage;

    protected virtual void Awake()
    {
        
    }

    public void setDamage(float e_damage)
    {
        damage = e_damage;
    }

    public void SetDirection(float angle)
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    protected virtual void OnEnable()
    {
        
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            GameManager.gameInstance.myPlayer.GetComponent<PlayerStat>().PlayerDamaged(damage, gameObject);
            PoolManager.poolInstance.ReleasePool(gameObject);
        }
        if (collision.transform.tag == "BulletBorder")
        {
            PoolManager.poolInstance.ReleasePool(gameObject);
        }
    }
}
