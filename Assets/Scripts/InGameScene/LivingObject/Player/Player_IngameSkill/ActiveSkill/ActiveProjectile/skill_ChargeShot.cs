using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Android;

public class Skill_ChargeShot : PlayerProjectile
{
    protected bool isPenetrate;
    protected int penetrateCount;
    protected int damageCount;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void ResetProj()
    {
        base.ResetProj();
        isPenetrate = false;
        penetrateCount = 0;
        damageCount = 0;
        
    }

    public override void SetAddParameter(float value1, float value2 =0, float value3 = 0, float value4 = 0)
    {
        base.SetAddParameter(value1, value2, value3,value4);
        if(value1 == 0)
        {
            return;
        }

        isHitOnce = false; //이거 없으면 penetrate 작동안함
        isPenetrate = true;
        penetrateCount = (int)value1;
        Debug.Log($"penetrateCount = {(int)value1}");
    }

    
    protected override void TriggedEnemy(Collider2D collision)
    {
        base.TriggedEnemy(collision);

        SingleEnemyDamage();

        if (!isPenetrate)
        {
            GameManager.Game.Pool.ReleasePool(gameObject);
            return;
        }

        
        if (damageCount == penetrateCount)
        {
            GameManager.Game.Pool.ReleasePool(gameObject);
        }
        damageCount++;

    }
}
