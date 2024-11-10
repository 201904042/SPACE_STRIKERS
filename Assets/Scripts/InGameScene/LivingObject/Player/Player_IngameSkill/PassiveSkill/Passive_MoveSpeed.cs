using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive_MoveSpeed : PassiveSkill
{
    public override void SkillReset()
    {
        base.SkillReset();
        SkillCode = 643;
        SetLevel();
    }

    public override void ApplyEffect()
    {
        base.ApplyEffect();
        pStat.PS_MSpd = CurrentEffectValue;
        pStat.SetPassiveStat();
    }
}
