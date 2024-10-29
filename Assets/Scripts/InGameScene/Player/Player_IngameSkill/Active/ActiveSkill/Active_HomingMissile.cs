using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Active_HomingMissile : ActiveSkill
{
    public bool isInit = false;

    public override void SkillReset()
    {
        base.SkillReset();
        SkillCode = 604;
        projType = PlayerProjType.Skill_Homing;
        SetLevel();
        SkillParameterSet(curSkillLevel);
        // 스킬 초기화 코드 (예: 스킬 레벨 세팅)
        Debug.Log("Active_HomingMissile 초기화 완료");
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
            Skill_Homing proj = GameManager.Instance.Pool.GetPlayerProj(projType, instantPoint.position, instantPoint.rotation).GetComponent<Skill_Homing>();
            Vector2 RandomDir = DirectionToRandomEnemy();
            proj.transform.up = RandomDir;
            proj.SetProjParameter(projSpd, dmgRate, liveTime, size);
        }
    }


    public override void SetLevel()
    {
        Skill_LevelValue lv1 = new Skill_LevelValue()
        {
            level = 1,
            ProjCount = 2,
            ProjSpd = 20,
            CoolTime = 3,
            DmgRate = 80
        };
        SkillLevels.Add(lv1.level, lv1);

        Skill_LevelValue lv2 = new Skill_LevelValue()
        {
            level = 2,
            ProjCount = 4,
            ProjSpd = 20,
            CoolTime = 3,
            DmgRate = 80
        };
        SkillLevels.Add(lv2.level, lv2);

        Skill_LevelValue lv3 = new Skill_LevelValue()
        {
            level = 3,
            ProjCount = 4,
            ProjSpd = 20,
            CoolTime = 2f,
            DmgRate = 80,
        };
        SkillLevels.Add(lv3.level, lv3);

        Skill_LevelValue lv4 = new Skill_LevelValue()
        {
            level = 4,
            ProjCount = 6,
            ProjSpd = 20,
            CoolTime = 2f,
            DmgRate = 80,
        };
        SkillLevels.Add(lv4.level, lv4);

        Skill_LevelValue lv5 = new Skill_LevelValue()
        {
            level = 5,
            ProjCount = 6,
            ProjSpd = 20,
            CoolTime = 1f,
            DmgRate = 80,
        };
        SkillLevels.Add(lv5.level, lv5);

        Skill_LevelValue lv6 = new Skill_LevelValue()
        {
            level = 6,
            ProjCount = 8,
            ProjSpd = 20,
            CoolTime = 1f,
            DmgRate = 80,
        };
        SkillLevels.Add(lv6.level, lv6);

        Skill_LevelValue lv7 = new Skill_LevelValue()
        {
            level = 7,
            ProjCount = 16,
            ProjSpd = 20,
            CoolTime = 0.5f,
            DmgRate = 80,
        };
        SkillLevels.Add(lv7.level, lv7);
        //Debug.Log($"{SkillCode}의 레벨 {SkillLevels.Count}개 등록");
    }
}
