using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using static UnityEditor.MaterialProperty;

public class USkill_Char3 : UniqueSkill
{
    float cycleDelay;
    public override void Init()
    {
        base.Init();
        SkillCode = 693;
        useCharCode = 103;
        projType = PlayerProjType.Spcial_Player3;
        SetLevel();
        Debug.Log("USkill_Char3 �ʱ�ȭ �Ϸ�");
    }

    public override void LevelUp()
    {
        //������ ������ ����
    }

    protected override void SkillParameterSet(int level)
    {
        base.SkillParameterSet(level);
        cycleDelay = CurSkillValue.AdditionalEffects[0].value;
    }


    public override void ActivateSkill(int level)
    {
        base.ActivateSkill(level);
        //������ �ʵ� ���� �� player3�� �ǵ� ���� �ƽ��� ���� => �ǵ� �ı� �ȵ�

        USkill_EnergyField proj = GameManager.Instance.Pool.GetPlayerProj(projType, instantPoint.position, instantPoint.rotation).GetComponent<USkill_EnergyField>();
        proj.SetAddParameter(cycleDelay);
        proj.SetProjParameter(projSpeed, dmgRate, liveTime, range); //�߻�ü�� �ӵ��� ������, ������ �ð��� ũ��
    }

    public override void SetLevel()
    {
        Skill_LevelValue lv1 = new Skill_LevelValue()
        {
            level = 1,
            ProjNum = 1,
            LiveTime = 5,
            DamageRate = 50,
            Range = 2
        };
        lv1.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.CycleDamage, 0.5f));
        SkillLevels.Add(lv1.level, lv1);

        Skill_LevelValue lv2 = new Skill_LevelValue()
        {
            level = 2,
            ProjNum = 1,
            LiveTime = 7.5f,
            DamageRate = 80,
            Range = 3.5f
        };
        lv2.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.CycleDamage, 0.5f));
        SkillLevels.Add(lv2.level, lv2);

        Skill_LevelValue lv3 = new Skill_LevelValue()
        {
            level = 3,
            ProjNum = 1,
            LiveTime = 10,
            DamageRate = 120,
            Range = 5
        };
        lv3.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.CycleDamage, 0.5f));
        SkillLevels.Add(lv3.level, lv3);

        Debug.Log($"{SkillCode}�� ���� {SkillLevels.Count}�� ���");
    }

    
}

