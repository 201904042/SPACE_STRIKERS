using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive_Defence : InGamePassiveSkill
{
    public override void Init()
    {
        base.Init();
        SkillCode = 642;
        SetLevel();
        currentLevel = 1; // 현재 레벨 초기화
    }
    public override void SetLevel()
    {
        base.SetLevel();

        Skill_LevelValue lv1 = new Skill_LevelValue()
        {
            level = 1,
            DamageRate = 10
        };
        SkillLevels.Add(lv1.level, lv1);

        Skill_LevelValue lv2 = new Skill_LevelValue()
        {
            level = 2,
            DamageRate = 20
        };
        SkillLevels.Add(lv2.level, lv2);

        Skill_LevelValue lv3 = new Skill_LevelValue()
        {
            level = 3,
            DamageRate = 30
        };
        SkillLevels.Add(lv3.level, lv3);

        Skill_LevelValue lv4 = new Skill_LevelValue()
        {
            level = 4,
            DamageRate = 40
        };
        SkillLevels.Add(lv4.level, lv4);


        Skill_LevelValue lv5 = new Skill_LevelValue()
        {
            level = 5,
            DamageRate = 50
        };
        SkillLevels.Add(lv5.level, lv5);

        Debug.Log($"{SkillCode}의 레벨 {SkillLevels.Count}개 등록");
    }
}
