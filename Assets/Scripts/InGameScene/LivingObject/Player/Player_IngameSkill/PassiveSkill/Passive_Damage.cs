using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive_Damage : PassiveSkill
{
    public override void SkillReset()
    {
        base.SkillReset();
        SkillCode = 641;
        SetLevel();
    }
    public override void ApplyEffect()
    {
        base.ApplyEffect();
        pStat.PS_Dmg = CurrentEffectValue;
        pStat.SetPassiveStat();
    }

}
