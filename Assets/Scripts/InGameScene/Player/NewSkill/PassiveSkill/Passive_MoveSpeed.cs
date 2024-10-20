using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive_MoveSpeed : NewPassiveSkill
{
    public override void Init()
    {
        base.Init();
        SkillCode = 643;
        SetLevel();
        currentLevel = 1; // 현재 레벨 초기화
    }
    public override void SetLevel()
    {
        base.SetLevel();

        Skill_LevelValue moveSpeed_lv1 = new Skill_LevelValue()
        {
            DamageRate = 10
        };
        SkillLevels.Add(moveSpeed_lv1);

        Skill_LevelValue moveSpeed_lv2 = new Skill_LevelValue()
        {
            DamageRate = 20
        };
        SkillLevels.Add(moveSpeed_lv2);

        Skill_LevelValue moveSpeed_lv3 = new Skill_LevelValue()
        {
            DamageRate = 30
        };
        SkillLevels.Add(moveSpeed_lv3);

        Skill_LevelValue moveSpeed_lv4 = new Skill_LevelValue()
        {
            DamageRate = 40
        };
        SkillLevels.Add(moveSpeed_lv4);


        Skill_LevelValue moveSpeed_lv5 = new Skill_LevelValue()
        {
            DamageRate = 50
        };
        SkillLevels.Add(moveSpeed_lv5);

        Debug.Log($"{SkillCode}의 레벨 {SkillLevels.Count}개 등록");
    }
}
