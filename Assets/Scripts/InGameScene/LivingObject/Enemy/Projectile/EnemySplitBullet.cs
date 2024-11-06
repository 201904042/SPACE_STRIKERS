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
        splitCount = Mathf.Max(5,(finalDamage / splitRate)); //�ּ� 5���� �и��� ����
        

        for (int i = 0; i < splitCount; i++)
        {
            GameObject newBullet = GameManager.Game.Pool.GetOtherProj(OtherProjType.Enemy_Bullet, 
                transform.position, Quaternion.identity);
            float angle = i * (360f / splitCount);
            transform.up = Quaternion.Euler(0, 0, angle) * transform.up; //todo => �̺κ� �����۵��ϴ��� üũ�Ұ�
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
