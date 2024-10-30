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
        Debug.Log("USkill_Char2 초기화 완료");
    }

    public override void LevelUp()
    {
        //레벨업 개념이 없음
    }

    protected override void ActivateSkill(int level)
    {
        base.ActivateSkill(level);

        USkill_Bomber proj = GameManager.Instance.Pool.GetPlayerProj(projType, instantPoint.position, instantPoint.rotation).GetComponent<USkill_Bomber>();
        proj.SetAddParameter(expDmg, expLiveTime, expSize, cycleDelay);
        proj.SetProjParameter(projSpd, dmgRate, 0, 0); //발사체의 속도와 데미지, 폭발의 시간과 크기
    }

}
