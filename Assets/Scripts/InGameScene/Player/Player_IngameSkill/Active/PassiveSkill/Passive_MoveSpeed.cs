using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive_MoveSpeed : InGamePassiveSkill
{
    public override void SkillReset()
    {
        base.SkillReset();
        SkillCode = 643;
        SetLevel();
        curSkillLevel = 1; // 현재 레벨 초기화
    }
    public override void SetLevel()
    {
        base.SetLevel();

        Skill_LevelValue lv1 = new Skill_LevelValue()
        {
            level = 1,
            DmgRate = 10
        };
        SkillLevels.Add(lv1.level,lv1);

        Skill_LevelValue lv2 = new Skill_LevelValue()
        {
            level = 2,
            DmgRate = 20
        };
        SkillLevels.Add(lv2.level, lv2);

        Skill_LevelValue lv3 = new Skill_LevelValue()
        {
            level = 3,
            DmgRate = 30
        };
        SkillLevels.Add(lv3.level, lv3);

        Skill_LevelValue lv4 = new Skill_LevelValue()
        {
            level = 4,
            DmgRate = 40
        };
        SkillLevels.Add(lv4.level, lv4);


        Skill_LevelValue lv5 = new Skill_LevelValue()
        {
            level = 5,
            DmgRate = 50
        };
        SkillLevels.Add(lv5.level, lv5);

        //Debug.Log($"{SkillCode}의 레벨 {SkillLevels.Count}개 등록");
    }
}
