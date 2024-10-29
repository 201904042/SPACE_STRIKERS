using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active_ChargeShot : ActiveSkill
{
    public override void SkillReset()
    {
        base.SkillReset();
        SkillCode = 601;
        projType = PlayerProjType.Skill_ChageShot;
        SetLevel();
        SkillParameterSet(curSkillLevel);
        Debug.Log("Active_ChargeShot 초기화 완료");
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
            Skill_ChargeShot proj = GameManager.Instance.Pool.GetPlayerProj(projType, instantPoint.position, instantPoint.rotation).GetComponent<Skill_ChargeShot>();
            proj.SetAddParameter(penetrateCount);
            proj.SetProjParameter(projSpd, dmgRate, liveTime, size);
        }
    }


    public override void SetLevel()
    {
        Skill_LevelValue lv1 = new Skill_LevelValue()
        {
            level = 1,
            ProjCount = 1,
            ProjSpd = 10,
            CoolTime = 5,
            DmgRate = 150
        };
        SkillLevels.Add(lv1.level, lv1);
        Skill_LevelValue lv2 = new Skill_LevelValue()
        {
            level = 2,
            ProjCount = 1,
            ProjSpd = 10,
            CoolTime = 5,
            DmgRate = 170
        };
        SkillLevels.Add(lv2.level, lv2);

        Skill_LevelValue lv3 = new Skill_LevelValue()
        {
            level = 3,
            ProjCount = 1,
            ProjSpd = 10,
            CoolTime = 4,
            DmgRate = 190
        };
        lv3.AddEffect.Add(new S_EffectValuePair(SkillAddEffect.Penetrate, 1));
        SkillLevels.Add(lv3.level, lv3);

        Skill_LevelValue lv4 = new Skill_LevelValue()
        {
            level = 4,
            ProjCount = 1,
            ProjSpd = 10,
            CoolTime = 4,
            DmgRate = 210
        };
        lv4.AddEffect.Add(new S_EffectValuePair(SkillAddEffect.Penetrate, 1));
        SkillLevels.Add(lv4.level, lv4);

        Skill_LevelValue lv5 = new Skill_LevelValue()
        {
            level = 5,
            ProjCount = 1,
            ProjSpd = 10,
            CoolTime = 3,
            DmgRate = 230
        };
        lv5.AddEffect.Add(new S_EffectValuePair(SkillAddEffect.Penetrate, 2));
        SkillLevels.Add(lv5.level, lv5);

        Skill_LevelValue lv6 = new Skill_LevelValue()
        {
            level = 6,
            ProjCount = 1,
            ProjSpd = 10,
            CoolTime = 3,
            DmgRate = 250
        };
        lv6.AddEffect.Add(new S_EffectValuePair(SkillAddEffect.Penetrate, 2));
        SkillLevels.Add(lv6.level, lv6);

        Skill_LevelValue lv7 = new Skill_LevelValue()
        {
            level = 7,
            ProjCount = 1,
            ProjSpd = 10,
            CoolTime = 3,
            DmgRate = 300
        };
        lv7.AddEffect.Add(new S_EffectValuePair( SkillAddEffect.Penetrate, 3));
        
        SkillLevels.Add(lv7.level, lv7);
        //Debug.Log($"{SkillCode}의 레벨 {SkillLevels.Count}개 등록");
    }

}
