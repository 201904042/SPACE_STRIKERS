using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using static UnityEditor.MaterialProperty;

public class USkill_Char3 : UniqueSkill
{
    public override void SkillReset()
    {
        base.SkillReset();
        SkillCode = 693;
        useCharCode = 103;
        projType = PlayerProjType.Spcial_Player3;
        SetLevel();
        //Debug.Log("USkill_Char3 �ʱ�ȭ �Ϸ�");
    }

    public override void LevelUp()
    {
        //������ ������ ����
    }

    protected override void ActivateSkill(int level)
    {
        base.ActivateSkill(level);
        //������ �ʵ� ���� �� player3�� �ǵ� ���� �ƽ��� ���� => �ǵ� �ı� �ȵ�

        USkill_EnergyField proj = GameManager.Game.Pool.GetPlayerProj(projType, instantPoint.position, instantPoint.rotation).GetComponent<USkill_EnergyField>();
        proj.SetAddParameter(cycleDelay);
        proj.SetProjParameter(projSpd, dmgRate, liveTime, size); //�߻�ü�� �ӵ��� ������, ������ �ð��� ũ��
    }

    
}

