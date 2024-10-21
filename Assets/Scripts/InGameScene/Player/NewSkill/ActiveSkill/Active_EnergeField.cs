using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active_EnergeField : NewActiveSkill
{
    public bool isInit = false;

    public override void Init()
    {
        base.Init();
        SkillCode = 603;
        projType = SkillProjType.Skill_EnergyField;
        SetLevel();
        // 스킬 초기화 코드 (예: 스킬 레벨 세팅)
        Debug.Log("Active_EnergeField 초기화 완료");
        isInit = true;
    }

    public override void LevelUp()
    {
        base.LevelUp(); // 부모 클래스의 LevelUp 호출
    }

    public override void ActivateSkill()
    {
        base.ActivateSkill();

        for (int i = 0; i < CurSkillValue.ProjNum; i++)
        {
            skill_Missile missile = GameManager.Instance.Pool.GetSkill(projType, instantPoint.position, instantPoint.rotation).GetComponent<skill_Missile>();
            Vector2 RandomDir = DirectionToRandomEnemy();
            missile.transform.up = RandomDir;
        }
    }


    public override void SetLevel()
    {
        Skill_LevelValue lv1 = new Skill_LevelValue()
        {
            Cooldown = 10,
            DamageRate = 20,
            Range = 8,
            LiveTime = 5
        };
        lv1.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.CycleDamage, 1f));
        SkillLevels.Add(lv1);

        Skill_LevelValue lv2 = new Skill_LevelValue()
        {
            Cooldown = 10,
            DamageRate = 40,
            Range = 8,
            LiveTime = 5
        };
        lv2.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.CycleDamage, 1f));
        SkillLevels.Add(lv2);

        Skill_LevelValue lv3 = new Skill_LevelValue()
        {
            Cooldown = 10,
            DamageRate = 40,
            Range = 8,
            LiveTime = 7
        };
        lv3.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.CycleDamage, 1f));
        SkillLevels.Add(lv3);

        Skill_LevelValue lv4 = new Skill_LevelValue()
        {
            Cooldown = 10,
            DamageRate = 40,
            Range = 10,
            LiveTime = 7
        };
        lv4.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.CycleDamage, 0.5f));
        SkillLevels.Add(lv4);

        Skill_LevelValue lv5 = new Skill_LevelValue()
        {
            Cooldown = 10,
            DamageRate = 70,
            Range = 10,
            LiveTime = 7
        };
        lv5.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.CycleDamage, 0.5f));
        SkillLevels.Add(lv5);

        Skill_LevelValue lv6 = new Skill_LevelValue()
        {
            Cooldown = 10,
            DamageRate = 70,
            Range = 10,
            LiveTime = 9
        };
        lv6.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.CycleDamage, 0.5f));
        SkillLevels.Add(lv6);

        Skill_LevelValue lv7 = new Skill_LevelValue()
        {
            ProjSpeed = 10,
            Cooldown = 10,
            DamageRate = 70,
            Range = 12,
            LiveTime = 9
        };
        lv7.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.CycleDamage, 0.5f));
        SkillLevels.Add(lv7);
        Debug.Log($"{SkillCode}의 레벨 {SkillLevels.Count}개 등록");
    }

}