using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive_Defence : PassiveSkill
{
    public override void SkillReset()
    {
        base.SkillReset();
        SkillCode = 642;
        SetLevel();
    }

    public override void ApplyEffect()
    {
        base.ApplyEffect();
        pStat.PS_Dfs = CurrentEffectValue;
        pStat.SetPassiveStat();
    }

}
