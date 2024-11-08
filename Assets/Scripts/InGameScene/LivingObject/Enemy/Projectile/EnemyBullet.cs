using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : EnemyProjectile
{
    protected override void OnEnable()
    {
        base.OnEnable();
        speed = BulletSpeed;
        defaultDmgRate = BulletDmgRate;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

}
