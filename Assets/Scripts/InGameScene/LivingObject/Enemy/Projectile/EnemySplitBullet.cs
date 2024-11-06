using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySplitBullet : EnemyProjectile
{
    private const int splitRate = 10;
    int splitCount;

    protected override void Awake()
    {
        base.Awake();
        speed = SplitBulletSpeed;
        defaultDmgRate = SplitBulletDmgRate;
    }
    private void SplitBullet()
    {
        splitCount = Mathf.Max(5,(finalDamage / splitRate)); //최소 5개의 분리를 보장
        

        for (int i = 0; i < splitCount; i++)
        {
            GameObject newBullet = GameManager.Game.Pool.GetOtherProj(OtherProjType.Enemy_Bullet, 
                transform.position, Quaternion.identity);
            float angle = i * (360f / splitCount);
            transform.up = Quaternion.Euler(0, 0, angle) * transform.up; //todo => 이부분 정상작동하는지 체크할것
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerStat>().PlayerDamaged(finalDamage, gameObject);
            GameManager.Game.Pool.ReleasePool(gameObject);
        }
        else if (collision.CompareTag("Border"))
        {
            SplitBullet();
            GameManager.Game.Pool.ReleasePool(gameObject);
        }
    }
}
