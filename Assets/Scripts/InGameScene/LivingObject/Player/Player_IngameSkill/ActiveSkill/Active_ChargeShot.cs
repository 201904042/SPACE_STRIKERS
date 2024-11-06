using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Active_ChargeShot : ActiveSkill
{
    public override void SkillReset()
    {
        base.SkillReset();
        SkillCode = 601;
        projType = PlayerProjType.Skill_ChageShot;
        SetLevel();
        description = "";
        //Debug.Log("Active_ChargeShot 초기화 완료");
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
            Skill_ChargeShot proj = GameManager.Game.Pool.GetPlayerProj(projType, instantPoint.position, instantPoint.rotation).GetComponent<Skill_ChargeShot>();
            proj.SetAddParameter(penetrateCount);
            proj.SetProjParameter(projSpd, dmgRate, liveTime, size);
        }
    }

}
