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
        // ��ų �ʱ�ȭ �ڵ� (��: ��ų ���� ����)
        Debug.Log("Active_EnergeField �ʱ�ȭ �Ϸ�");
    }

    public override void LevelUp()
    {
        base.LevelUp(); // �θ� Ŭ������ LevelUp ȣ��
    }

    protected override void ActivateSkill(int level)
    {
        base.ActivateSkill(level);

        Skill_EnergyField energyField =
                GameManager.Game.Pool.GetPlayerProj(projType, instantPoint.position, instantPoint.rotation).GetComponent<Skill_EnergyField>();
        energyField.SetAddParameter(cycleDelay);
        energyField.SetProjParameter(projSpd, dmgRate, liveTime, size);
    }


}