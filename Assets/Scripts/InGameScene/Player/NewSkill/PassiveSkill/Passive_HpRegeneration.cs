using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive_HpRegeneration : NewPassiveSkill
{
    public override void Init()
    {
        base.Init();
        SkillCode = 645;
        SetLevel();
        currentLevel = 1; // ���� ���� �ʱ�ȭ
    }
    public override void SetLevel()
    {
        base.SetLevel();

        Skill_LevelValue hpRegen_lv1 = new Skill_LevelValue()
        {
            DamageRate = 10
        };
        SkillLevels.Add(hpRegen_lv1);

        Skill_LevelValue hpRegen_lv2 = new Skill_LevelValue()
        {
            DamageRate = 20
        };
        SkillLevels.Add(hpRegen_lv2);

        Skill_LevelValue hpRegen_lv3 = new Skill_LevelValue()
        {
            DamageRate = 30
        };
        SkillLevels.Add(hpRegen_lv3);

        Skill_LevelValue hpRegen_lv4 = new Skill_LevelValue()
        {
            DamageRate = 40
        };
        SkillLevels.Add(hpRegen_lv4);


        Skill_LevelValue hpRegen_lv5 = new Skill_LevelValue()
        {
            DamageRate = 50
        };
        SkillLevels.Add(hpRegen_lv5);

        Debug.Log($"{SkillCode}�� ���� {SkillLevels.Count}�� ���");
    }
}
