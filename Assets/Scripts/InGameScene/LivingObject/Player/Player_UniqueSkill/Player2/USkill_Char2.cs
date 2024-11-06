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
        //Debug.Log("USkill_Char2 �ʱ�ȭ �Ϸ�");
    }

    public override void LevelUp()
    {
        //������ ������ ����
    }

    protected override void ActivateSkill(int level)
    {
        base.ActivateSkill(level);

        USkill_Bomber proj = GameManager.Game.Pool.GetPlayerProj(projType, instantPoint.position, instantPoint.rotation).GetComponent<USkill_Bomber>();
        proj.SetAddParameter(expDmg, expLiveTime, expSize, cycleDelay);
        proj.SetProjParameter(projSpd, dmgRate, 0, 0); //�߻�ü�� �ӵ��� ������, ������ �ð��� ũ��
    }

}
