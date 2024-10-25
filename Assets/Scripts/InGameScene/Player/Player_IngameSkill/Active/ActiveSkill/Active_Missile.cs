using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static UnityEditor.MaterialProperty;

public class Active_Missile : NewActiveSkill
{
    public bool isInit = false;

    public override void Init()
    {
        base.Init();
        SkillCode = 606;
        projType = PlayerProjType.Skill_Missile;
        SetLevel();
        SkillParameterSet();
        // ��ų �ʱ�ȭ �ڵ� (��: ��ų ���� ����)
        Debug.Log("Active_Missile �ʱ�ȭ �Ϸ�");
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
            Skill_Missile proj = GameManager.Instance.Pool.GetPlayerProj(projType, instantPoint.position, instantPoint.rotation).GetComponent<Skill_Missile>();
            Vector2 RandomDir = DirectionToRandomEnemy();
            proj.transform.up = RandomDir;

            proj.SetProjParameter(projSpeed, dmgRate, liveTime, range);
        }
    }


    public override void SetLevel()
    {
        Skill_LevelValue lv1 = new Skill_LevelValue()
        {
            ProjNum = 1,
            ProjSpeed = 15,
            Cooldown = 3,
            DamageRate = 150,
            Range = 5
        };
        SkillLevels.Add(lv1);

        Skill_LevelValue lv2 = new Skill_LevelValue()
        {
            ProjNum = 2,
            ProjSpeed = 15,
            Cooldown = 3,
            DamageRate = 150,
            Range = 5
        };
        SkillLevels.Add(lv2);

        Skill_LevelValue lv3 = new Skill_LevelValue()
        {
            ProjNum = 2,
            ProjSpeed = 15,
            Cooldown = 3,
            DamageRate = 170,
            Range = 5
        };
        SkillLevels.Add(lv3);

        Skill_LevelValue lv4 = new Skill_LevelValue()
        {
            ProjNum = 2,
            ProjSpeed = 15,
            Cooldown = 2,
            DamageRate = 170,
            Range = 5
        };
        SkillLevels.Add(lv4);

        Skill_LevelValue lv5 = new Skill_LevelValue()
        {
            ProjNum = 4,
            ProjSpeed = 15,
            Cooldown = 2,
            DamageRate = 170,
            Range = 5
        };
        SkillLevels.Add(lv5);

        Skill_LevelValue lv6 = new Skill_LevelValue()
        {
            ProjNum = 4,
            ProjSpeed = 15,
            Cooldown = 1,
            DamageRate = 200,
            Range = 5
        };
        SkillLevels.Add(lv6);

        Skill_LevelValue lv7 = new Skill_LevelValue()
        {
            ProjNum = 4,
            ProjSpeed = 15,
            Cooldown = 1,
            DamageRate = 200,
            Range = 10
        };
        SkillLevels.Add(lv7);
        Debug.Log($"{SkillCode}�� ���� {SkillLevels.Count}�� ���");
    }

}
