using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active_ChargeShot : NewActiveSkill
{
    public override void Init()
    {
        base.Init();
        SkillCode = 601;
        projType = SkillProjType.Skill_ChageShot;
        SetLevel();
        SkillParameterSet();
        Debug.Log("Active_ChargeShot 초기화 완료");
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
            Skill_ChargeShot proj = GameManager.Instance.Pool.GetSkill(projType, instantPoint.position, instantPoint.rotation).GetComponent<Skill_ChargeShot>();
            proj.SetAddParameter(penetrateCount);
            proj.SetProjParameter(projSpeed, dmgRate, liveTime, range);
        }
    }


    public override void SetLevel()
    {
        Skill_LevelValue lv1 = new Skill_LevelValue()
        {
            ProjNum = 1,
            ProjSpeed = 10,
            Cooldown = 5,
            DamageRate = 150
        };
        
        SkillLevels.Add(lv1);

        Skill_LevelValue lv2 = new Skill_LevelValue()
        {
            ProjNum = 1,
            ProjSpeed = 10,
            Cooldown = 5,
            DamageRate = 170
        };
        SkillLevels.Add(lv2);

        Skill_LevelValue lv3 = new Skill_LevelValue()
        {
            ProjNum = 1,
            ProjSpeed = 10,
            Cooldown = 4,
            DamageRate = 190
        };
        lv3.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.Penetrate, 1));
        SkillLevels.Add(lv3);

        Skill_LevelValue lv4 = new Skill_LevelValue()
        {
            ProjNum = 1,
            ProjSpeed = 10,
            Cooldown = 4,
            DamageRate = 210
        };
        lv4.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.Penetrate, 1));
        SkillLevels.Add(lv4);

        Skill_LevelValue lv5 = new Skill_LevelValue()
        {
            ProjNum = 1,
            ProjSpeed = 10,
            Cooldown = 3,
            DamageRate = 230
        };
        lv5.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.Penetrate, 2));
        SkillLevels.Add(lv5);

        Skill_LevelValue lv6 = new Skill_LevelValue()
        {
            ProjNum = 1,
            ProjSpeed = 10,
            Cooldown = 3,
            DamageRate = 250
        };
        lv6.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.Penetrate, 2));
        SkillLevels.Add(lv6);

        Skill_LevelValue lv7 = new Skill_LevelValue()
        {
            ProjNum = 1,
            ProjSpeed = 10,
            Cooldown = 3,
            DamageRate = 300
        };
        lv7.AdditionalEffects.Add(new S_EffectValuePair( SkillAddEffect.Penetrate, 3));
        
        SkillLevels.Add(lv7);
        Debug.Log($"{SkillCode}의 레벨 {SkillLevels.Count}개 등록");
    }

}
