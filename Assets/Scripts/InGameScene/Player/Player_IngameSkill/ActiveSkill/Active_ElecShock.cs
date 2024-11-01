using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active_ElecShock : ActiveSkill
{
    public bool isInit = false;

    public override void SkillReset()
    {
        base.SkillReset();
        SkillCode = 602;
        projType = PlayerProjType.Skill_ElecShock;
        SetLevel();
        // 스킬 초기화 코드 (예: 스킬 레벨 세팅)
        Debug.Log("Active_ElecShock 초기화 완료");
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
            Skill_ElecShock elecShock = GameManager.Game.Pool.GetPlayerProj(projType, instantPoint.position, instantPoint.rotation).GetComponent<Skill_ElecShock>();
            elecShock.SetAddParameter(cycleDelay, slowRate, slowDmgRate);
            elecShock.SetProjParameter(projSpd, dmgRate, 0, size);
        }
    }

}
