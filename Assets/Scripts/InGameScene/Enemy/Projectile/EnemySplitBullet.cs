using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySplitBullet : MonoBehaviour
{
    public GameObject enemyBullet;

    public int splitCount;
    public float damage;
    private float speed;

    private void Awake()
    {
        splitCount =5;
        speed = 5f;
    }

    private void SplitBullet()
    {
        for(int i = 0; i < splitCount; i++)
        {
            GameObject newBullet = Instantiate(enemyBullet, transform.position, Quaternion.identity);
            float angle = i * (360f / splitCount);
            Rigidbody2D rigid = newBullet.GetComponent<Rigidbody2D>();
            newBullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            rigid.velocity = newBullet.transform.up*speed;
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerStat>().PlayerDamaged(damage, gameObject);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Border"))
        {
            SplitBullet();
            Destroy(gameObject);
        }
    }
}
