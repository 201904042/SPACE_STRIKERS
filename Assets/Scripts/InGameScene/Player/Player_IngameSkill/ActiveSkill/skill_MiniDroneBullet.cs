using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class skill_MiniDroneBullet : PlayerProjectile
{
    public override void SetProjParameter(int _projSpeed, int _dmgRate, float _liveTime, float _range)
    {
        base.SetProjParameter(_projSpeed, _dmgRate, 0, 0);
        Debug.Log("»ý¼ºµÊ");
    }

    protected override void TriggedEnemy(Collider2D collision)
    {
        base.TriggedEnemy(collision);

        SingleEnemyDamage();
    }
}
