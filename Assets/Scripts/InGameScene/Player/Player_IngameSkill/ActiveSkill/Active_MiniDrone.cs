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
            skill_MiniDrone proj = GameManager.Game.Pool.GetPlayerProj(projType, instantPoint.position, instantPoint.rotation).GetComponent<skill_MiniDrone>();
            proj.SetAddParameter(dAtkSpd,dAtkRange);
            proj.SetProjParameter(projSpd, dmgRate, liveTime, 0);
        }
    }
}
