using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Skill_ElecShock : PlayerProjectile
{

    [SerializeField] protected bool isCycleDamage; //������ �������� �ִ� ���� �ΰ� = ���ǽ�ų
    [SerializeField] protected float cycleRate;

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void ResetProj()
    {
        base.ResetProj();
        isCycleDamage = false;
        cycleRate = 0;
        isSlow = false;
        slowRate = 0;
        isSlowExtraDamage = false;
        extraDamageRate = 0;
    }

    private bool isSlow;
    private int slowRate;
    private bool isSlowExtraDamage;
    private int extraDamageRate;

    public override void SetAddParameter(float value1, float value2 =0, float value3 = 0, float value4 = 0)
    {
        base.SetAddParameter(value1, value2, value3, value4);
        if (value1 == 0)
        {
            return;
        }
        isCycleDamage = true;
        cycleRate = value1;

        if (value2 == 0)
        {
            return;
        }
        isSlow = true;
        slowRate = (int)value2;

        if (value3 == 0)
        {
            return;
        }
        isSlowExtraDamage = true;
        extraDamageRate = (int)value3;
    }

    public override void SetProjParameter(int _projSpeed, int _dmgRate, float _liveTime, float _range)
    {
        base.SetProjParameter(_projSpeed, _dmgRate, _liveTime, _range);
        isHitOnce = false;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        
        if (collision.gameObject.tag == "Enemy")
        {
            //�� ������ �� ���ο� ȿ�� �߰�
        }
    }

    protected override void TriggedEnemy(Collider2D collision)
    {
        base.TriggedEnemy(collision);
        

        if (damaging == null)
        {
            damaging = StartCoroutine(AreaDamageLogic(cycleRate));
        }
    }

}
