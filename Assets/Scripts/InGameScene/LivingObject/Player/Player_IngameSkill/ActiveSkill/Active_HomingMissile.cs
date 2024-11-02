using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Active_HomingMissile : ActiveSkill
{
    public bool isInit = false;

    public override void SkillReset()
    {
        base.SkillReset();
        SkillCode = 604;
        projType = PlayerProjType.Skill_Homing;
        SetLevel();
        // 스킬 초기화 코드 (예: 스킬 레벨 세팅)
        Debug.Log("Active_HomingMissile 초기화 완료");
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
            Skill_Homing proj = GameManager.Game.Pool.GetPlayerProj(projType, instantPoint.position, instantPoint.rotation).GetComponent<Skill_Homing>();
            Vector2 RandomDir = DirectionToRandomEnemy();
            proj.transform.up = RandomDir;
            proj.SetProjParameter(projSpd, dmgRate, liveTime, size);
        }
    }
}
