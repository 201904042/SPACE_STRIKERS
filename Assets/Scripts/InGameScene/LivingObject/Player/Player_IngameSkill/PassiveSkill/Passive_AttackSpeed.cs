using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive_AttackSpeed : PassiveSkill
{
    public override void SkillReset()
    {
        base.SkillReset();
        SkillCode = 644;
        SetLevel();
    }

    public override void ApplyEffect()
    {
        base.ApplyEffect();
        pStat.IG_ASpd = CurrentEffectValue;
        pStat.ApplyStat();
    }

}

