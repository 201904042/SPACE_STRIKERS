using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active_ElecShock : ActiveSkill
{
    public bool isInit = false;

    public override void SkillReset()
    {
        base.SkillReset();
        SkillCode = 602;
        projType = PlayerProjType.Skill_ElecShock;
        SetLevel();
        SkillParameterSet(curSkillLevel);
        // 스킬 초기화 코드 (예: 스킬 레벨 세팅)
        Debug.Log("Active_ElecShock 초기화 완료");
        isInit = true;
    }

    public override void LevelUp()
    {
        base.LevelUp(); // 부모 클래스의 LevelUp 호출
    }

    protected override void ActivateSkill(int level)
    {
        base.ActivateSkill(level);

        for (int i = 0; i < CurSkillValue.ProjCount; i++)
        {
            Skill_ElecShock elecShock = GameManager.Instance.Pool.GetPlayerProj(projType, instantPoint.position, instantPoint.rotation).GetComponent<Skill_ElecShock>();
            elecShock.SetAddParameter(cycleDelay, slowRate, slowDmgRate);
            elecShock.SetProjParameter(projSpd, dmgRate, 0, size);
        }
    }


    public override void SetLevel()
    {
        Skill_LevelValue lv1 = new Skill_LevelValue()
        {
            level = 1,
            ProjCount = 1,
            ProjSpd = 1,
            CoolTime = 5,
            DmgRate = 30,
            Size = 1
        };
        lv1.AddEffect.Add(new S_EffectValuePair(SkillAddEffect.CycleDamage, 1));
        lv1.AddEffect.Add(new S_EffectValuePair(SkillAddEffect.Slow, 30));
        SkillLevels.Add(lv1.level, lv1);

        Skill_LevelValue lv2 = new Skill_LevelValue()
        {
            level = 2,
            ProjCount = 1,
            ProjSpd =1,
            CoolTime = 5,
            DmgRate = 60,
            Size = 1
        };
        lv2.AddEffect.Add(new S_EffectValuePair(SkillAddEffect.CycleDamage, 1));
        lv2.AddEffect.Add(new S_EffectValuePair(SkillAddEffect.Slow, 30));
        SkillLevels.Add(lv2.level, lv2);

        Skill_LevelValue lv3 = new Skill_LevelValue()
        {
            level = 3,
            ProjCount = 1,
            ProjSpd = 1,
            CoolTime = 5,
            DmgRate = 60,
            Size = 1.5f
        };
        lv3.AddEffect.Add(new S_EffectValuePair(SkillAddEffect.CycleDamage, 0.75f));
        lv3.AddEffect.Add(new S_EffectValuePair(SkillAddEffect.Slow, 50));
        SkillLevels.Add(lv3.level, lv3);

        Skill_LevelValue lv4 = new Skill_LevelValue()
        {
            level = 4,
            ProjCount = 1,
            ProjSpd = 1,
            CoolTime = 5,
            DmgRate = 90,
            Size = 1.5f
        };
        lv4.AddEffect.Add(new S_EffectValuePair(SkillAddEffect.CycleDamage, 0.75f));
        lv4.AddEffect.Add(new S_EffectValuePair(SkillAddEffect.Slow, 50));
        SkillLevels.Add(lv4.level, lv4);

        Skill_LevelValue lv5 = new Skill_LevelValue()
        {
            level = 5,
            ProjCount = 1,
            ProjSpd = 1,
            CoolTime = 5,
            DmgRate = 120,
            Size = 1.5f
        };
        lv5.AddEffect.Add(new S_EffectValuePair(SkillAddEffect.CycleDamage, 0.5f));
        lv5.AddEffect.Add(new S_EffectValuePair(SkillAddEffect.Slow, 70));
        SkillLevels.Add(lv5.level, lv5);

        Skill_LevelValue lv6 = new Skill_LevelValue()
        {
            level = 6,
            ProjCount = 1,
            ProjSpd = 1,
            CoolTime = 5,
            DmgRate = 120,
            Size = 1.5f
        };
        lv6.AddEffect.Add(new S_EffectValuePair(SkillAddEffect.CycleDamage, 0.5f));
        lv6.AddEffect.Add(new S_EffectValuePair(SkillAddEffect.Slow, 70));
        SkillLevels.Add(lv6.level, lv6);

        Skill_LevelValue lv7 = new Skill_LevelValue()
        {
            level = 7,
            ProjCount = 1,
            ProjSpd = 1,
            CoolTime = 5,
            DmgRate = 120,
            Size = 3f
        };
        lv7.AddEffect.Add(new S_EffectValuePair(SkillAddEffect.CycleDamage, 0.25f));
        lv7.AddEffect.Add(new S_EffectValuePair(SkillAddEffect.Slow, 70));
        lv7.AddEffect.Add(new S_EffectValuePair(SkillAddEffect.SlowExtraDamage, 100));
        SkillLevels.Add(lv7.level, lv7);
        //Debug.Log($"{SkillCode}의 레벨 {SkillLevels.Count}개 등록");
    }

}
