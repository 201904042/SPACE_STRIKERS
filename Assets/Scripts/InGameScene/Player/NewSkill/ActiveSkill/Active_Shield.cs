using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active_Shield : NewActiveSkill
{
    public bool isInit = false;

    public override void Init()
    {
        base.Init();
        SkillCode = 607;
        projType = SkillProjType.Skill_Missile;
        SetLevel();
        // 스킬 초기화 코드 (예: 스킬 레벨 세팅)
        Debug.Log("Active_Shield 초기화 완료");
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
            Skill_Missile missile = GameManager.Instance.Pool.GetSkill(projType, instantPoint.position, instantPoint.rotation).GetComponent<Skill_Missile>();
            Vector2 RandomDir = DirectionToRandomEnemy();
            missile.transform.up = RandomDir;
        }
    }

    public override void SetLevel()
    {
        Skill_LevelValue lv1 = new Skill_LevelValue()
        {
            ProjNum = 1, //중첩 실드량
            Cooldown = 20, //재생성 쿨타임
            DamageRate = 100, //충돌시 데미지
        };
        SkillLevels.Add(lv1);

        Skill_LevelValue lv2 = new Skill_LevelValue()
        {
            ProjNum = 1, //중첩 실드량
            Cooldown = 18, //재생성 쿨타임
            DamageRate = 100, //충돌시 데미지
        };
        SkillLevels.Add(lv2);

        Skill_LevelValue lv3 = new Skill_LevelValue()
        {
            ProjNum = 1, //중첩 실드량
            Cooldown = 16, //재생성 쿨타임
            DamageRate = 100, //충돌시 데미지
        };
        SkillLevels.Add(lv3);

        Skill_LevelValue lv4 = new Skill_LevelValue()
        {
            ProjNum = 2, //중첩 실드량
            Cooldown = 16, //재생성 쿨타임
            DamageRate = 150, //충돌시 데미지
        };
        SkillLevels.Add(lv4);

        Skill_LevelValue lv5 = new Skill_LevelValue()
        {
            ProjNum = 2, //중첩 실드량
            Cooldown = 14, //재생성 쿨타임
            DamageRate = 150, //충돌시 데미지
        };
        SkillLevels.Add(lv5);

        Skill_LevelValue lv6 = new Skill_LevelValue()
        {
            ProjNum = 2, //중첩 실드량
            Cooldown = 12, //재생성 쿨타임
            DamageRate = 150, //충돌시 데미지
        };
        SkillLevels.Add(lv6);

        Skill_LevelValue lv7 = new Skill_LevelValue()
        {
            ProjNum = 3, //중첩 실드량
            Cooldown = 10, //재생성 쿨타임
            DamageRate = 200, //충돌시 데미지
        };
        SkillLevels.Add(lv7);
        Debug.Log($"{SkillCode}의 레벨 {SkillLevels.Count}개 등록");
    }
}