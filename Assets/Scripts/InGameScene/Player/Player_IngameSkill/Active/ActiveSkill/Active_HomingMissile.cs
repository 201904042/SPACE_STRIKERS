using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Active_HomingMissile : ActiveSkill
{
    public bool isInit = false;

    public override void Init()
    {
        base.Init();
        SkillCode = 604;
        projType = PlayerProjType.Skill_Homing;
        SetLevel();
        SkillParameterSet();
        // 스킬 초기화 코드 (예: 스킬 레벨 세팅)
        Debug.Log("Active_HomingMissile 초기화 완료");
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
            Skill_Homing proj = GameManager.Instance.Pool.GetPlayerProj(projType, instantPoint.position, instantPoint.rotation).GetComponent<Skill_Homing>();
            Vector2 RandomDir = DirectionToRandomEnemy();
            proj.transform.up = RandomDir;
            proj.SetProjParameter(projSpeed, dmgRate, liveTime, range);
        }
    }


    public override void SetLevel()
    {
        Skill_LevelValue lv1 = new Skill_LevelValue()
        {
            ProjNum = 2,
            ProjSpeed = 20,
            Cooldown = 3,
            DamageRate = 80,
            Range = 0.1f
        };
        SkillLevels.Add(lv1);

        Skill_LevelValue lv2 = new Skill_LevelValue()
        {
            ProjNum = 4,
            ProjSpeed = 20,
            Cooldown = 3,
            DamageRate = 80,
            Range = 0.1f
        };
        SkillLevels.Add(lv2);

        Skill_LevelValue lv3 = new Skill_LevelValue()
        {
            ProjNum = 4,
            ProjSpeed = 20,
            Cooldown = 2f,
            DamageRate = 80,
        };
        SkillLevels.Add(lv3);

        Skill_LevelValue lv4 = new Skill_LevelValue()
        {
            ProjNum = 6,
            ProjSpeed = 20,
            Cooldown = 2f,
            DamageRate = 80,
        };
        SkillLevels.Add(lv4);

        Skill_LevelValue lv5 = new Skill_LevelValue()
        {
            ProjNum = 6,
            ProjSpeed = 20,
            Cooldown = 1f,
            DamageRate = 80,
        };
        SkillLevels.Add(lv5);

        Skill_LevelValue lv6 = new Skill_LevelValue()
        {
            ProjNum = 8,
            ProjSpeed = 20,
            Cooldown = 1f,
            DamageRate = 80,
        };
        SkillLevels.Add(lv6);

        Skill_LevelValue lv7 = new Skill_LevelValue()
        {
            ProjNum = 16,
            ProjSpeed = 20,
            Cooldown = 0.5f,
            DamageRate = 80,
        };
        SkillLevels.Add(lv7);
        Debug.Log($"{SkillCode}의 레벨 {SkillLevels.Count}개 등록");
    }
}
