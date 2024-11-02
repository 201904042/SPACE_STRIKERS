using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraSkill : InGameSkill
{
    protected int value;

    public override void SkillReset()
    {
        type = SkillType.Extra;
        curSkillLevel = 1;
    }

    public override void SetLevel()
    {
        //Debug.Log($"{SkillCode}의 레벨데이터 세팅");
        
        value = DataManager.skill.GetData(SkillCode).levels[0].DmgRate;
    }

    public override void LevelUp()
    {
        //레벨업 사용하지 않음
    }

    public virtual void ApplyEffect()
    {
        //스킬 효과 반영
    }
}
