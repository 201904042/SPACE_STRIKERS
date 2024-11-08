using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySplitBullet : EnemyProjectile
{
    private const int defaultSplitCount = 3;
    int splitCount;
    protected override void OnEnable()
    {
        base.OnEnable();
        speed = SplitBulletSpeed;
        defaultDmgRate = SplitBulletDmgRate;
        splitCount = defaultSplitCount;
    }

    public override void SetSplitCount(int count)
    {
        splitCount = count;
    }

    private void SplitBullet()
    {
        for (int i = 0; i < splitCount; i++)
        {
            EnemyProjectile proj = GameManager.Game.Pool.GetOtherProj(OtherProjType.Enemy_Bullet, 
                transform.position, Quaternion.identity).GetComponent<EnemyProjectile>();
            float angle = i * (360f / splitCount);
            proj.transform.up = Quaternion.Euler(0, 0, angle) * transform.up;
            proj.SetProjParameter(damageRate, 0);
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
