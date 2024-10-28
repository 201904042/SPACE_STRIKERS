using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static UnityEditor.MaterialProperty;

public class Active_Missile : ActiveSkill
{
    public bool isInit = false;

    public override void Init()
    {
        base.Init();
        SkillCode = 606;
        projType = PlayerProjType.Skill_Missile;
        SetLevel();
        SkillParameterSet();
        // 스킬 초기화 코드 (예: 스킬 레벨 세팅)
        Debug.Log("Active_Missile 초기화 완료");
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
            level = 1,
            ProjNum = 1,
            ProjSpeed = 15,
            Cooldown = 3,
            DamageRate = 150,
            LiveTime = 1,
            Range = 5
        };
        SkillLevels.Add(lv1.level, lv1);

        Skill_LevelValue lv2 = new Skill_LevelValue()
        {
            level = 2,
            ProjNum = 2,
            ProjSpeed = 15,
            Cooldown = 3,
            DamageRate = 150,
            LiveTime = 1,
            Range = 5
        };
        SkillLevels.Add(lv2.level, lv2);

        Skill_LevelValue lv3 = new Skill_LevelValue()
        {
            level = 3,
            ProjNum = 2,
            ProjSpeed = 15,
            Cooldown = 3,
            DamageRate = 170,
            LiveTime = 1,
            Range = 5
        };
        SkillLevels.Add(lv3.level, lv3);

        Skill_LevelValue lv4 = new Skill_LevelValue()
        {
            level = 4,
            ProjNum = 2,
            ProjSpeed = 15,
            Cooldown = 2,
            DamageRate = 170,
            LiveTime = 1,
            Range = 5
        };
        SkillLevels.Add(lv4.level, lv4);

        Skill_LevelValue lv5 = new Skill_LevelValue()
        {
            level = 5,
            ProjNum = 4,
            ProjSpeed = 15,
            Cooldown = 2,
            DamageRate = 170,
            LiveTime = 1,
            Range = 5
        };
        SkillLevels.Add(lv5.level, lv5);

        Skill_LevelValue lv6 = new Skill_LevelValue()
        {
            level = 6,
            ProjNum = 4,
            ProjSpeed = 15,
            Cooldown = 1,
            DamageRate = 200,
            LiveTime = 1,
            Range = 5
        };
        SkillLevels.Add(lv6.level, lv6);

        Skill_LevelValue lv7 = new Skill_LevelValue()
        {
            level = 7,
            ProjNum = 4,
            ProjSpeed = 15,
            Cooldown = 1,
            DamageRate = 200,
            LiveTime = 1,
            Range = 10
        };
        SkillLevels.Add(lv7.level, lv7);
        //Debug.Log($"{SkillCode}의 레벨 {SkillLevels.Count}개 등록");
    }

}
