using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active_MiniDrone : ActiveSkill
{
    public bool isInit = false;

    public override void Init()
    {
        base.Init();
        SkillCode = 605;
        projType = PlayerProjType.Skill_MiniDrone;
        SetLevel();
        SkillParameterSet();
        // ��ų �ʱ�ȭ �ڵ� (��: ��ų ���� ����)
        Debug.Log("Active_MiniDrone �ʱ�ȭ �Ϸ�");
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
            skill_MiniDrone proj = GameManager.Instance.Pool.GetPlayerProj(projType, instantPoint.position, instantPoint.rotation).GetComponent<skill_MiniDrone>();
            proj.SetAddParameter(droneAtkSpd);
            proj.SetProjParameter(projSpeed, dmgRate, liveTime, range);
        }
    }


    public override void SetLevel()
    {
        Skill_LevelValue lv1 = new Skill_LevelValue()
        {
            ProjNum = 1,
            ProjSpeed = 15, //����� �̵��ӵ�
            Cooldown = 30, //����� �ð�
            DamageRate = 50,
            Range = 3,  //����� ��������
            LiveTime = 15, //����� �����ð�
        };
        lv1.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.Drone, 1.2f));
        SkillLevels.Add(lv1);

        Skill_LevelValue lv2 = new Skill_LevelValue()
        {
            ProjNum = 1,
            ProjSpeed = 15, //����� �̵��ӵ�
            Cooldown = 30, //����� �ð�
            DamageRate = 70,
            Range = 3,  //����� ��������
            LiveTime = 15, //����� �����ð�
        };
        lv2.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.Drone, 1.2f));
        SkillLevels.Add(lv2);

        Skill_LevelValue lv3 = new Skill_LevelValue()
        {
            ProjNum = 1,
            ProjSpeed = 15, //����� �̵��ӵ�
            Cooldown = 25, //����� �ð�
            DamageRate = 70,
            Range = 3,  //����� ��������
            LiveTime = 15, //����� �����ð�
        };
        lv3.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.Drone, 0.9f));
        SkillLevels.Add(lv3);

        Skill_LevelValue lv4 = new Skill_LevelValue()
        {
            ProjNum = 2,
            ProjSpeed = 15, //����� �̵��ӵ�
            Cooldown = 25, //����� �ð�
            DamageRate = 70,
            Range = 4,  //����� ��������
            LiveTime = 15, //����� �����ð�
        };
        lv4.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.Drone, 0.9f));
        SkillLevels.Add(lv4);

        Skill_LevelValue lv5 = new Skill_LevelValue()
        {
            ProjNum = 2,
            ProjSpeed = 15, //����� �̵��ӵ�
            Cooldown = 25, //����� �ð�
            DamageRate = 100,
            Range = 4,  //����� ��������
            LiveTime = 20, //����� �����ð�
        };
        lv5.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.Drone, 0.9f));
        SkillLevels.Add(lv5);


        Skill_LevelValue lv6 = new Skill_LevelValue()
        {
            ProjNum = 2,
            ProjSpeed = 15, //����� �̵��ӵ�
            Cooldown = 25, //����� �ð�
            DamageRate = 100,
            Range = 4,  //����� ��������
            LiveTime = 20, //����� �����ð�
        };
        lv6.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.Drone, 0.7f));
        SkillLevels.Add(lv6);

        Skill_LevelValue lv7 = new Skill_LevelValue()
        {
            ProjNum = 4,
            ProjSpeed = 15, //����� �̵��ӵ�
            Cooldown = 20, //����� �ð�
            DamageRate = 150,
            Range = 5,  //����� ��������
            LiveTime = 20, //����� �����ð�
        };
        lv7.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.Drone, 0.5f));
        SkillLevels.Add(lv6);
        Debug.Log($"{SkillCode}�� ���� {SkillLevels.Count}�� ���");
    }
}
