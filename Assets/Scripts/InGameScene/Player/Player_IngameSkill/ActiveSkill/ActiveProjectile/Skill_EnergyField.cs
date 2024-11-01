using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.WSA;


public class Skill_EnergyField : PlayerProjectile
{
    [SerializeField] protected bool isCycleDamage; //������ �������� �ִ� ���� �ΰ� = ���ǽ�ų
    [SerializeField] protected float cycleRate;

    protected override void ResetProj()
    {
        base.ResetProj();
        isCycleDamage = false;
        cycleRate = 0;
    }

    public override void SetAddParameter(float value1, float value2 =0, float value3 = 0, float value4 = 0)
    {
        base.SetAddParameter(value1, value2, value3, value4);
        if (value1 == 0)
        {
            return;
        }
        isCycleDamage = true;
        cycleRate = value1;
    }

    public override void SetProjParameter(int _projSpeed, int _dmgRate, float _liveTime, float _range)
    {
        base.SetProjParameter(_projSpeed, _dmgRate, _liveTime, _range);
        isHitOnce = false;
        isShootingObj = false; 
    }

    protected override IEnumerator LiveTimer(float activeTime)
    {
        yield return new WaitForSeconds(activeTime);

        if (speed == 0)
        {
            GameManager.Game.Pool.ReleasePool(gameObject);
        }
        else
        {
            isShootingObj = true;
        }
    }

    

    protected override void TriggedEnemy(Collider2D collision)
    {
        base.TriggedEnemy(collision);

        if(damaging == null)
        {
            damaging = StartCoroutine(AreaDamageLogic(cycleRate));
        }
        
    }

}
