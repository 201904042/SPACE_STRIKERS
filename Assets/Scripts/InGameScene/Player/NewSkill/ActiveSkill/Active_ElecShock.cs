using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active_ElecShock : NewActiveSkill
{
    public bool isInit = false;

    public override void Init()
    {
        base.Init();
        SkillCode = 602;
        projType = SkillProjType.Skill_ElecShock;
        SetLevel();
        SkillParameterSet();
        // 스킬 초기화 코드 (예: 스킬 레벨 세팅)
        Debug.Log("Active_ElecShock 초기화 완료");
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
            Skill_ElecShock elecShock = GameManager.Instance.Pool.GetSkill(projType, instantPoint.position, instantPoint.rotation).GetComponent<Skill_ElecShock>();
            elecShock.SetAddParameter(cycleDelay, slowRate, slowDmgRate);
            elecShock.SetProjParameter(projSpeed, dmgRate, liveTime, range);
        }
    }


    public override void SetLevel()
    {
        Skill_LevelValue lv1 = new Skill_LevelValue()
        {
            ProjNum = 1,
            ProjSpeed = 1,
            Cooldown = 5,
            DamageRate = 30,
            Range = 1
        };
        lv1.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.CycleDamage, 3));
        lv1.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.Slow, 30));

        SkillLevels.Add(lv1);

        Skill_LevelValue lv2 = new Skill_LevelValue()
        {
            ProjNum = 1,
            ProjSpeed =1,
            Cooldown = 5,
            DamageRate = 60,
            Range = 1
        };
        lv2.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.CycleDamage, 3));
        lv2.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.Slow, 30));
        SkillLevels.Add(lv2);

        Skill_LevelValue lv3 = new Skill_LevelValue()
        {
            ProjNum = 1,
            ProjSpeed = 1,
            Cooldown = 5,
            DamageRate = 60,
            Range = 1.5f
        };
        lv3.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.CycleDamage, 2.5f));
        lv3.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.Slow, 50));
        SkillLevels.Add(lv3);

        Skill_LevelValue lv4 = new Skill_LevelValue()
        {
            ProjNum = 1,
            ProjSpeed = 1,
            Cooldown = 5,
            DamageRate = 90,
            Range = 1.5f
        };
        lv4.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.CycleDamage, 2.5f));
        lv4.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.Slow, 50));
        SkillLevels.Add(lv4);

        Skill_LevelValue lv5 = new Skill_LevelValue()
        {
            ProjNum = 1,
            ProjSpeed = 1,
            Cooldown = 5,
            DamageRate = 120,
            Range = 1.5f
        };
        lv5.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.CycleDamage, 2.5f));
        lv5.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.Slow, 50));
        SkillLevels.Add(lv5);

        Skill_LevelValue lv6 = new Skill_LevelValue()
        {
            ProjNum = 1,
            ProjSpeed = 1,
            Cooldown = 5,
            DamageRate = 120,
            Range = 1.5f
        };
        lv6.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.CycleDamage, 2f));
        lv6.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.Slow, 70));
        SkillLevels.Add(lv6);

        Skill_LevelValue lv7 = new Skill_LevelValue()
        {
            ProjNum = 1,
            ProjSpeed = 1,
            Cooldown = 5,
            DamageRate = 120,
            Range = 3f
        };
        lv6.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.CycleDamage, 2f));
        lv7.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.Slow, 70));
        lv7.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.SlowExtraDamage, 100));
        SkillLevels.Add(lv7);
        Debug.Log($"{SkillCode}의 레벨 {SkillLevels.Count}개 등록");
    }

}
