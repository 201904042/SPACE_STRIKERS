using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active_EnergeField : ActiveSkill
{
    public override void Init()
    {
        base.Init();
        SkillCode = 603;
        projType = PlayerProjType.Skill_EnergyField;
        SetLevel();
        SkillParameterSet();
        // 스킬 초기화 코드 (예: 스킬 레벨 세팅)
        Debug.Log("Active_EnergeField 초기화 완료");
    }

    public override void LevelUp()
    {
        base.LevelUp(); // 부모 클래스의 LevelUp 호출
    }

    public override void ActivateSkill()
    {
        base.ActivateSkill();

        Skill_EnergyField energyField =
                GameManager.Instance.Pool.GetPlayerProj(projType, instantPoint.position, instantPoint.rotation).GetComponent<Skill_EnergyField>();
        energyField.SetAddParameter(cycleDelay);
        energyField.SetProjParameter(projSpeed, dmgRate, liveTime, range);
    }


    public override void SetLevel()
    {
        Skill_LevelValue lv1 = new Skill_LevelValue()
        {
            level = 1,
            Cooldown = 10,
            DamageRate = 20,
            Range = 2,
            LiveTime = 5
        };
        lv1.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.CycleDamage, 1f));
        SkillLevels.Add(lv1.level, lv1);

        Skill_LevelValue lv2 = new Skill_LevelValue()
        {
            level = 2,
            Cooldown = 10,
            DamageRate = 40,
            Range = 2,
            LiveTime = 5
        };
        lv2.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.CycleDamage, 1f));
        SkillLevels.Add(lv2.level, lv2);

        Skill_LevelValue lv3 = new Skill_LevelValue()
        {
            level = 3,
            Cooldown = 10,
            DamageRate = 40,
            Range = 4,
            LiveTime = 7
        };
        lv3.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.CycleDamage, 1f));
        SkillLevels.Add(lv3.level, lv3);

        Skill_LevelValue lv4 = new Skill_LevelValue()
        {
            level = 4,
            Cooldown = 10,
            DamageRate = 40,
            Range = 4,
            LiveTime = 7
        };
        lv4.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.CycleDamage, 0.5f));
        SkillLevels.Add(lv4.level, lv4);


        Skill_LevelValue lv5 = new Skill_LevelValue()
        {
            level = 5,
            Cooldown = 10,
            DamageRate = 70,
            Range = 6,
            LiveTime = 7
        };
        lv5.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.CycleDamage, 0.5f));
        SkillLevels.Add(lv5.level, lv5);

        Skill_LevelValue lv6 = new Skill_LevelValue()
        {
            level = 6,
            Cooldown = 10,
            DamageRate = 70,
            Range = 6,
            LiveTime = 9
        };
        lv6.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.CycleDamage, 0.5f));
        SkillLevels.Add(lv6.level, lv6);

        Skill_LevelValue lv7 = new Skill_LevelValue()
        {
            level = 7,
            ProjSpeed = 10,
            Cooldown = 10,
            DamageRate = 70,
            Range = 8,
            LiveTime = 9
        };
        lv7.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.CycleDamage, 0.5f));
        SkillLevels.Add(lv7.level, lv7);
        //Debug.Log($"{SkillCode}의 레벨 {SkillLevels.Count}개 등록");
    }

}