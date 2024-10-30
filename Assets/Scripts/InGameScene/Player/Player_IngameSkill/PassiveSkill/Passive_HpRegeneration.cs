using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive_HpRegeneration : PassiveSkill
{
    public override void SkillReset()
    {
        base.SkillReset();
        SkillCode = 645;
        SetLevel();
    }

    public override void ApplyEffect()
    {
        base.ApplyEffect();
        pStat.PS_HpRegen = CurrentEffectValue;
        pStat.ApplyStat();
    }

}
