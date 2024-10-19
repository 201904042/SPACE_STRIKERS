using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySplitBullet : EnemyProjectile
{
    public GameObject enemyBullet;

    public int splitCount;
    private float speed;

    protected override void Awake()
    {
        splitCount =5;
        speed = 5f;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

    }


    private void SplitBullet()
    {
        for(int i = 0; i < splitCount; i++)
        {
            GameObject newBullet = GameManager.Instance.Pool.GetProj(ProjType.Enemy_Bullet, 
                transform.position, Quaternion.identity);
            float angle = i * (360f / splitCount);
            Rigidbody2D rigid = newBullet.GetComponent<Rigidbody2D>();
            newBullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            rigid.velocity = newBullet.transform.up*speed;
            
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerStat>().PlayerDamaged(damage, gameObject);
            GameManager.Instance.Pool.ReleasePool(gameObject);
        }
        else if (collision.CompareTag("Border"))
        {
            SplitBullet();
            GameManager.Instance.Pool.ReleasePool(gameObject);
        }
    }
}
