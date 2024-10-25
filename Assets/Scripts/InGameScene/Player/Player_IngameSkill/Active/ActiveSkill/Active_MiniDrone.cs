using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active_MiniDrone : NewActiveSkill
{
    public bool isInit = false;

    public override void Init()
    {
        base.Init();
        SkillCode = 605;
        projType = PlayerProjType.Skill_MiniDrone;
        SetLevel();
        SkillParameterSet();
        // 스킬 초기화 코드 (예: 스킬 레벨 세팅)
        Debug.Log("Active_MiniDrone 초기화 완료");
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
            ProjSpeed = 15, //드론의 이동속도
            Cooldown = 30, //재생성 시간
            DamageRate = 50,
            Range = 3,  //드론의 감지범위
            LiveTime = 15, //드론의 생존시간
        };
        lv1.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.Drone, 1.2f));
        SkillLevels.Add(lv1);

        Skill_LevelValue lv2 = new Skill_LevelValue()
        {
            ProjNum = 1,
            ProjSpeed = 15, //드론의 이동속도
            Cooldown = 30, //재생성 시간
            DamageRate = 70,
            Range = 3,  //드론의 감지범위
            LiveTime = 15, //드론의 생존시간
        };
        lv2.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.Drone, 1.2f));
        SkillLevels.Add(lv2);

        Skill_LevelValue lv3 = new Skill_LevelValue()
        {
            ProjNum = 1,
            ProjSpeed = 15, //드론의 이동속도
            Cooldown = 25, //재생성 시간
            DamageRate = 70,
            Range = 3,  //드론의 감지범위
            LiveTime = 15, //드론의 생존시간
        };
        lv3.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.Drone, 0.9f));
        SkillLevels.Add(lv3);

        Skill_LevelValue lv4 = new Skill_LevelValue()
        {
            ProjNum = 2,
            ProjSpeed = 15, //드론의 이동속도
            Cooldown = 25, //재생성 시간
            DamageRate = 70,
            Range = 4,  //드론의 감지범위
            LiveTime = 15, //드론의 생존시간
        };
        lv4.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.Drone, 0.9f));
        SkillLevels.Add(lv4);

        Skill_LevelValue lv5 = new Skill_LevelValue()
        {
            ProjNum = 2,
            ProjSpeed = 15, //드론의 이동속도
            Cooldown = 25, //재생성 시간
            DamageRate = 100,
            Range = 4,  //드론의 감지범위
            LiveTime = 20, //드론의 생존시간
        };
        lv5.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.Drone, 0.9f));
        SkillLevels.Add(lv5);


        Skill_LevelValue lv6 = new Skill_LevelValue()
        {
            ProjNum = 2,
            ProjSpeed = 15, //드론의 이동속도
            Cooldown = 25, //재생성 시간
            DamageRate = 100,
            Range = 4,  //드론의 감지범위
            LiveTime = 20, //드론의 생존시간
        };
        lv6.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.Drone, 0.7f));
        SkillLevels.Add(lv6);

        Skill_LevelValue lv7 = new Skill_LevelValue()
        {
            ProjNum = 4,
            ProjSpeed = 15, //드론의 이동속도
            Cooldown = 20, //재생성 시간
            DamageRate = 150,
            Range = 5,  //드론의 감지범위
            LiveTime = 20, //드론의 생존시간
        };
        lv7.AdditionalEffects.Add(new S_EffectValuePair(SkillAddEffect.Drone, 0.5f));
        SkillLevels.Add(lv6);
        Debug.Log($"{SkillCode}의 레벨 {SkillLevels.Count}개 등록");
    }
}
