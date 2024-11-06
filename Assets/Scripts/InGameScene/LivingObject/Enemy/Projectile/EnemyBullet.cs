using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : EnemyProjectile
{
   
    protected override void Awake()
    {
        base.Awake();
        
    }

    public override void SetProjParameter(int _dmgRate, float _liveTime = 0, float _range = 0)
    {
        base.SetProjParameter(_dmgRate, _liveTime, _range);
        speed = BulletSpeed;
        defaultDmgRate = BulletDmgRate;
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

}
