using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active_Shield : ActiveSkill
{
    private Skill_Shield instantShield;

    public override void Init()
    {
        base.Init();
        SkillCode = 607;
        projType = PlayerProjType.Skill_Shield;
        SetLevel();
        SkillParameterSet();
        instantShield = null;
        // 스킬 초기화 코드 (예: 스킬 레벨 세팅)
        Debug.Log("Active_Shield 초기화 완료");
    }

    //쉴드는 다른 것과 달리 1개만 생성하며 쉴드가 파괴되었을때를 기점으로 쿨타임이 돌아감

    public override void LevelUp()
    {
        base.LevelUp(); // 부모 클래스의 LevelUp 호출
        if (instantShield != null) {
            GameManager.Instance.Pool.ReleasePool(instantShield.gameObject); // 레벨업시 기존 스킬 파괴 및 바로 재생성
            instantShield = null;
        }
        
        ActivateSkill();
    }

    public override IEnumerator ActivateSkillCoroutine()
    {
        //ActivateSkill(); // 스킬 발동
        while (true)
        {
            if (instantShield != null) 
            {
                yield return null;
                continue;
            }

            yield return new WaitForSeconds(CurSkillValue.Cooldown); // 쿨타임 동안 대기
            ActivateSkill(); // 스킬 발동
        }
    }

    /// <summary>
    /// 각 스킬의 실질적인 수행
    /// </summary>
    public override void ActivateSkill()
    {
        // 발사체 생성 코드
        if(instantShield != null)
        {
            return;
        }
        Skill_Shield proj = GameManager.Instance.Pool.GetPlayerProj(projType, instantPoint.position, instantPoint.rotation).GetComponent<Skill_Shield>();
        instantShield = proj;
        proj.SetProjParameter(0, dmgRate, 0, range);
    }


    public override void SetLevel()
    {
        Skill_LevelValue lv1 = new Skill_LevelValue()
        {
            level = 1,
            Cooldown = 20, //재생성 쿨타임
            DamageRate = 200, //충돌시 데미지
            Range = 1
        };
        SkillLevels.Add(lv1.level, lv1);

        Skill_LevelValue lv2 = new Skill_LevelValue()
        {
            level = 2,
            Cooldown = 18, //재생성 쿨타임
            DamageRate = 200, //충돌시 데미지
            Range = 1
        };
        SkillLevels.Add(lv2.level, lv2);

        Skill_LevelValue lv3 = new Skill_LevelValue()
        {
            level = 3,
            Cooldown = 16, //재생성 쿨타임
            DamageRate = 300, //충돌시 데미지
            Range = 1.2f
        };
        SkillLevels.Add(lv3.level, lv3);

        Skill_LevelValue lv4 = new Skill_LevelValue()
        {
            level = 4,
            Cooldown = 16, //재생성 쿨타임
            DamageRate = 300, //충돌시 데미지
            Range = 1.2f
        };
        SkillLevels.Add(lv4.level, lv4);

        Skill_LevelValue lv5 = new Skill_LevelValue()
        {
            level = 5,
            Cooldown = 14, //재생성 쿨타임
            DamageRate = 400, //충돌시 데미지
            Range = 1.5f
        };
        SkillLevels.Add(lv5.level, lv5);

        Skill_LevelValue lv6 = new Skill_LevelValue()
        {
            level = 6,
            Cooldown = 12, //재생성 쿨타임
            DamageRate = 400, //충돌시 데미지
            Range = 1.5f
        };
        SkillLevels.Add(lv6.level, lv6);

        Skill_LevelValue lv7 = new Skill_LevelValue()
        {
            level = 7,
            Cooldown = 10, //재생성 쿨타임
            DamageRate = 500, //충돌시 데미지
            Range = 2f
        };
        SkillLevels.Add(lv7.level, lv7);
        Debug.Log($"{SkillCode}의 레벨 {SkillLevels.Count}개 등록");
    }
}