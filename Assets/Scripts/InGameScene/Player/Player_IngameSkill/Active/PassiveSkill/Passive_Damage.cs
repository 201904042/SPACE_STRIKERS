using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive_Damage : InGamePassiveSkill
{
    public override void Init()
    {
        base.Init();
        SkillCode = 641;
        SetLevel();
        currentLevel = 1; // 현재 레벨 초기화
    }
    public override void SetLevel()
    {
        base.SetLevel();
        
        Skill_LevelValue damage_lv1 = new Skill_LevelValue()
        {
            DamageRate = 10
        };
        SkillLevels.Add(damage_lv1);

        Skill_LevelValue damage_lv2 = new Skill_LevelValue()
        {
            DamageRate = 20
        };
        SkillLevels.Add(damage_lv2);

        Skill_LevelValue damage_lv3 = new Skill_LevelValue()
        {
            DamageRate = 30
        };
        SkillLevels.Add(damage_lv3);

        Skill_LevelValue damage_lv4 = new Skill_LevelValue()
        {
            DamageRate = 40
        };
        SkillLevels.Add(damage_lv4);


        Skill_LevelValue damage_lv5 = new Skill_LevelValue()
        {
            DamageRate = 50
        };
        SkillLevels.Add(damage_lv5);

        Debug.Log($"{SkillCode}의 레벨 {SkillLevels.Count}개 등록");
    }
}
