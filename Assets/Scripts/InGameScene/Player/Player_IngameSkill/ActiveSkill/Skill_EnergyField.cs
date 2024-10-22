using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.WSA;


public class Skill_EnergyField : PlayerProjectile
{
    protected bool isCycleDamage; //여러번 데미지를 주는 변수 인가 = 장판스킬
    protected int cycleRate;

    protected override void ResetProj()
    {
        base.ResetProj();
        isCycleDamage = false;
        cycleRate = 0;
    }


    public override void SetProjParameter(int _projSpeed, int _dmgRate, float _liveTime, float _range, float value1=0, float value2 = 0)
    {
        base.SetProjParameter(_projSpeed, _dmgRate, _liveTime, _range, value1, value2);
        isCycleDamage = true;
        cycleRate = (int)value1;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }


}
