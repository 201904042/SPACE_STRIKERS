using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive_MoveSpeed : InGamePassiveSkill
{
    public override void Init()
    {
        base.Init();
        SkillCode = 643;
        SetLevel();
        currentLevel = 1; // ���� ���� �ʱ�ȭ
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

        Debug.Log($"{SkillCode}�� ���� {SkillLevels.Count}�� ���");
    }
}
