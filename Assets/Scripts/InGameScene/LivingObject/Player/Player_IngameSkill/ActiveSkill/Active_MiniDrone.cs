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
        // ��ų �ʱ�ȭ �ڵ� (��: ��ų ���� ����)
        Debug.Log("Active_MiniDrone �ʱ�ȭ �Ϸ�");
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
            skill_MiniDrone proj = GameManager.Game.Pool.GetPlayerProj(projType, instantPoint.position, instantPoint.rotation).GetComponent<skill_MiniDrone>();
            proj.SetAddParameter(dAtkSpd,dAtkRange);
            proj.SetProjParameter(projSpd, dmgRate, liveTime, 0);
        }
    }
}
