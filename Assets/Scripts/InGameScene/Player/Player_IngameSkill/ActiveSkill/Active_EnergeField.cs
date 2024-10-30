using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active_EnergeField : ActiveSkill
{
    public override void SkillReset()
    {
        base.SkillReset();
        SkillCode = 603;
        projType = PlayerProjType.Skill_EnergyField;
        SetLevel();
        // 스킬 초기화 코드 (예: 스킬 레벨 세팅)
        Debug.Log("Active_EnergeField 초기화 완료");
    }

    public override void LevelUp()
    {
        base.LevelUp(); // 부모 클래스의 LevelUp 호출
    }

    protected override void ActivateSkill(int level)
    {
        base.ActivateSkill(level);

        Skill_EnergyField energyField =
                GameManager.Instance.Pool.GetPlayerProj(projType, instantPoint.position, instantPoint.rotation).GetComponent<Skill_EnergyField>();
        energyField.SetAddParameter(cycleDelay);
        energyField.SetProjParameter(projSpd, dmgRate, liveTime, size);
    }


}