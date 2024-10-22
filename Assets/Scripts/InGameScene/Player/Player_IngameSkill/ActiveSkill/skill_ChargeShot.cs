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


    public override void SetProjParameter(int _projSpeed, int _dmgRate, float _liveTime, float _range, float value1 =0 , float value2 = 0)
    {
        base.SetProjParameter(_projSpeed, _dmgRate, _liveTime, _range,value1,value2);

        isPenetrate = true;
        penetrateCount = (int)value1;
    }

    private void Update()
    {
        //todo -> 코루틴으로 작동?
        MoveUp();
    }

    //todo -> 트리거부분도 개선해볼것
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    protected override void TriggedEnemy(Collider2D collision)
    {
        base.TriggedEnemy(collision);

        if (!isPenetrate)
        {
            GameManager.Instance.Pool.ReleasePool(gameObject);
            return;
        }

        damageCount++;
        if (damageCount == penetrateCount)
        {
            GameManager.Instance.Pool.ReleasePool(gameObject);
        }
    }
}
