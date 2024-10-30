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

        Debug.Log("Active_ChargeShot �ʱ�ȭ �Ϸ�");
    }

    public override void LevelUp()
    {
        base.LevelUp(); // �θ� Ŭ������ LevelUp ȣ��
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

}
