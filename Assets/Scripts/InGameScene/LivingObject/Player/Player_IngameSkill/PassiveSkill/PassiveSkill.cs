using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PassiveSkill : InGameSkill
{
    public float CurrentEffectValue => SkillLevels[curSkillLevel].DmgRate; // 현재 효과 값
    protected PlayerStat pStat => PlayerMain.pStat;
    public override void SkillReset()
    {
        SkillLevels = new Dictionary<int, Skill_LevelValue>();
        type = SkillType.Passive;
        curSkillLevel = 0;
    }

    //사용전 스킬코드가 정의되어있는지 체크
    public override void SetLevel()
    {
       // Debug.Log($"{SkillCode}의 레벨데이터 세팅");
        foreach (Skill_LevelValue skill in DataManager.skill.GetData(SkillCode).levels)
        {
            SkillLevels.Add(skill.level, skill);
        }
        description = SkillLevels[curSkillLevel + 1].Description;
    }

    public override void LevelUp()
    {
        if (curSkillLevel < SkillLevels.Count - 1)
        {
            curSkillLevel++;
            ApplyEffect();
        }

        description = SkillLevels[curSkillLevel + 1].Description;
    }

    public virtual void ApplyEffect()
    {
        Debug.Log(CurrentEffectValue);
        //각 스킬별로 증가치 반영
    }
}
