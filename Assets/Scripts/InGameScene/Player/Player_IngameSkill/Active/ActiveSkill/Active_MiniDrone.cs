using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active_MiniDrone : ActiveSkill
{
    public override void SkillReset()
    {
        base.SkillReset();
        SkillCode = 605;
        projType = PlayerProjType.Skill_MiniDrone;
        SetLevel();
        SkillParameterSet(curSkillLevel);
        // 스킬 초기화 코드 (예: 스킬 레벨 세팅)
        Debug.Log("Active_MiniDrone 초기화 완료");
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
            skill_MiniDrone proj = GameManager.Instance.Pool.GetPlayerProj(projType, instantPoint.position, instantPoint.rotation).GetComponent<skill_MiniDrone>();
            proj.SetAddParameter(dAtkSpd,dAtkRange);
            proj.SetProjParameter(projSpd, dmgRate, liveTime, 0);
        }
    }


    public override void SetLevel()
    {
        Skill_LevelValue lv1 = new Skill_LevelValue()
        {
            level = 1,
            ProjCount = 1,
            CoolTime = 30, //재생성 시간

            ProjSpd = 15, //드론의 이동속도
            DmgRate = 50,
            LiveTime = 15, //드론의 생존시간
        };
        lv1.AddEffect.Add(new S_EffectValuePair(SkillAddEffect.Drone, 1.2f, 3)); //드론 공격속도, 범위
        SkillLevels.Add(lv1.level, lv1);

        Skill_LevelValue lv2 = new Skill_LevelValue()
        {
            level = 2,
            ProjCount = 1,
            CoolTime = 30, //재생성 시간

            ProjSpd = 15, //드론의 이동속도
            DmgRate = 70,
            LiveTime = 15, //드론의 생존시간
        };
        lv2.AddEffect.Add(new S_EffectValuePair(SkillAddEffect.Drone, 1.2f,3));
        SkillLevels.Add(lv2.level, lv2);

        Skill_LevelValue lv3 = new Skill_LevelValue()
        {
            level = 3,
            ProjCount = 1,
            CoolTime = 25, //재생성 시간

            ProjSpd = 15, //드론의 이동속도
            DmgRate = 70,
            LiveTime = 15, //드론의 생존시간
        };
        lv3.AddEffect.Add(new S_EffectValuePair(SkillAddEffect.Drone, 0.9f,3));
        SkillLevels.Add(lv3.level, lv3);

        Skill_LevelValue lv4 = new Skill_LevelValue()
        {
            level = 4,
            ProjCount = 2,
            CoolTime = 25, //재생성 시간

            ProjSpd = 15, //드론의 이동속도
            DmgRate = 70,
            LiveTime = 15, //드론의 생존시간
        };
        lv4.AddEffect.Add(new S_EffectValuePair(SkillAddEffect.Drone, 0.9f,4));
        SkillLevels.Add(lv4.level, lv4);

        Skill_LevelValue lv5 = new Skill_LevelValue()
        {
            level = 5,
            ProjCount = 2,
            CoolTime = 25, //재생성 시간

            ProjSpd = 15, //드론의 이동속도
            DmgRate = 100,
            LiveTime = 20, //드론의 생존시간
        };
        lv5.AddEffect.Add(new S_EffectValuePair(SkillAddEffect.Drone, 0.9f,4));
        SkillLevels.Add(lv5.level, lv5);


        Skill_LevelValue lv6 = new Skill_LevelValue()
        {
            level = 6,
            ProjCount = 2,
            CoolTime = 25, //재생성 시간

            ProjSpd = 15, //드론의 이동속도
            DmgRate = 100,
            LiveTime = 20, //드론의 생존시간
        };
        lv6.AddEffect.Add(new S_EffectValuePair(SkillAddEffect.Drone, 0.7f,4));
        SkillLevels.Add(lv6.level, lv6);

        Skill_LevelValue lv7 = new Skill_LevelValue()
        {
            level = 7,
            ProjCount = 4,
            CoolTime = 20, //재생성 시간

            ProjSpd = 15, //드론의 이동속도
            DmgRate = 150,
            LiveTime = 20, //드론의 생존시간
        };
        lv7.AddEffect.Add(new S_EffectValuePair(SkillAddEffect.Drone, 0.5f,5));
        SkillLevels.Add(lv7.level, lv7);
        //Debug.Log($"{SkillCode}의 레벨 {SkillLevels.Count}개 등록");
    }
}
