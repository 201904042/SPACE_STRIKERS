using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active_ElecShock : ActiveSkill
{
    public bool isInit = false;

    public override void Init()
    {
        base.Init();
        SkillCode = 602;
        projType = PlayerProjType.Skill_ElecShock;
        SetLevel();
        SkillParameterSet();
        // ��ų �ʱ�ȭ �ڵ� (��: ��ų ���� ����)
        Debug.Log("Active_ElecShock �ʱ�ȭ �Ϸ�");
        isInit = true;
    }

    public override void LevelUp()
    {
        base.LevelUp(); // �θ� Ŭ������ LevelUp ȣ��
    }

    public override void ActivateSkill()
    {
        base.ActivateSkill();

        for (int i = 0; i < CurSkillValue.ProjNum; i++)
        {
            Skill_ElecShock elecShock = GameManager.Instance.Pool.GetPlayerProj(projType, instantPoint.position, instantPoint.rotation).GetComponent<Skill_ElecShock>();
            elecShock.SetAddParameter(cycleDelay, slowRate, slowDmgRate);
            elecShock.SetProjParameter(projSpeed, dmgRate, liveTime, range);
        }
    }


    public override void SetLevel()
    {
        Skill_LevelValue lv1 = new Skill_LevelValue()
        {
            level = 1,
            ProjNum = 1,
            ProjSpeed = 1,
            Cooldown = 5,
            DamageRate = 30,
            Range = 1
        };
        lv1.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.CycleDamage, 3));
        lv1.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.Slow, 30));

        SkillLevels.Add(lv1.level, lv1);

        Skill_LevelValue lv2 = new Skill_LevelValue()
        {
            level = 2,
            ProjNum = 1,
            ProjSpeed =1,
            Cooldown = 5,
            DamageRate = 60,
            Range = 1
        };
        lv2.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.CycleDamage, 3));
        lv2.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.Slow, 30));
        SkillLevels.Add(lv2.level, lv2);

        Skill_LevelValue lv3 = new Skill_LevelValue()
        {
            level = 3,
            ProjNum = 1,
            ProjSpeed = 1,
            Cooldown = 5,
            DamageRate = 60,
            Range = 1.5f
        };
        lv3.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.CycleDamage, 2.5f));
        lv3.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.Slow, 50));
        SkillLevels.Add(lv3.level, lv3);

        Skill_LevelValue lv4 = new Skill_LevelValue()
        {
            level = 4,
            ProjNum = 1,
            ProjSpeed = 1,
            Cooldown = 5,
            DamageRate = 90,
            Range = 1.5f
        };
        lv4.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.CycleDamage, 2.5f));
        lv4.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.Slow, 50));
        SkillLevels.Add(lv4.level, lv4);

        Skill_LevelValue lv5 = new Skill_LevelValue()
        {
            level = 5,
            ProjNum = 1,
            ProjSpeed = 1,
            Cooldown = 5,
            DamageRate = 120,
            Range = 1.5f
        };
        lv5.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.CycleDamage, 2.5f));
        lv5.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.Slow, 50));
        SkillLevels.Add(lv5.level, lv5);

        Skill_LevelValue lv6 = new Skill_LevelValue()
        {
            level = 6,
            ProjNum = 1,
            ProjSpeed = 1,
            Cooldown = 5,
            DamageRate = 120,
            Range = 1.5f
        };
        lv6.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.CycleDamage, 2f));
        lv6.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.Slow, 70));
        SkillLevels.Add(lv6.level, lv6);

        Skill_LevelValue lv7 = new Skill_LevelValue()
        {
            level = 7,
            ProjNum = 1,
            ProjSpeed = 1,
            Cooldown = 5,
            DamageRate = 120,
            Range = 3f
        };
        lv7.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.CycleDamage, 2f));
        lv7.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.Slow, 70));
        lv7.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.SlowExtraDamage, 100));
        SkillLevels.Add(lv7.level, lv7);
        Debug.Log($"{SkillCode}�� ���� {SkillLevels.Count}�� ���");
    }

}
