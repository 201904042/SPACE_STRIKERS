using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class USkill_Char2 : UniqueSkill
{

    public override void SkillReset()
    {
        base.SkillReset();
        SkillCode = 692;
        useCharCode = 102;
        projType = PlayerProjType.Spcial_Player2;
        SetLevel();
        Debug.Log("USkill_Char2 초기화 완료");
    }

    public override void LevelUp()
    {
        //레벨업 개념이 없음
    }

    protected override void ActivateSkill(int level)
    {
        base.ActivateSkill(level);

        USkill_Bomber proj = GameManager.Instance.Pool.GetPlayerProj(projType, instantPoint.position, instantPoint.rotation).GetComponent<USkill_Bomber>();
        proj.SetAddParameter(expDmg, expLiveTime, expSize, cycleDelay);
        proj.SetProjParameter(projSpd, dmgRate, 0, 0); //발사체의 속도와 데미지, 폭발의 시간과 크기
    }

    public override void SetLevel()
    {
        Skill_LevelValue lv1 = new Skill_LevelValue()
        {
            level = 1,
            ProjCount = 1,
            ProjSpd = 5,
            DmgRate = 150
        };
        lv1.AddEffect.Add(new S_EffectValuePair(SkillAddEffect.Explosion, 50, 3, 2));
        lv1.AddEffect.Add(new S_EffectValuePair(SkillAddEffect.CycleDamage, 0.5f));
        SkillLevels.Add(lv1.level, lv1);

        Skill_LevelValue lv2 = new Skill_LevelValue()
        {
            level = 2,
            ProjCount = 1,
            ProjSpd = 5,
            DmgRate = 300
        };
        lv2.AddEffect.Add(new S_EffectValuePair(SkillAddEffect.Explosion, 100, 5, 4));
        lv2.AddEffect.Add(new S_EffectValuePair(SkillAddEffect.CycleDamage, 0.5f));
        SkillLevels.Add(lv2.level, lv2);

        Skill_LevelValue lv3 = new Skill_LevelValue()
        {
            level = 3,
            ProjCount = 1,
            ProjSpd = 5,
            DmgRate = 500
        };
        lv3.AddEffect.Add(new S_EffectValuePair(SkillAddEffect.Explosion, 150, 8, 6));
        lv3.AddEffect.Add(new S_EffectValuePair(SkillAddEffect.CycleDamage, 0.5f));
        SkillLevels.Add(lv3.level, lv3);

        Debug.Log($"{SkillCode}의 레벨 {SkillLevels.Count}개 등록");
    }
}
