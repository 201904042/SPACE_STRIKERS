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
        Debug.Log("USkill_Char3 초기화 완료");
    }

    public override void LevelUp()
    {
        //레벨업 개념이 없음
    }

    protected override void ActivateSkill(int level)
    {
        base.ActivateSkill(level);
        //에너지 필드 전개 및 player3의 실드 개수 맥스로 증가 => 실드 파괴 안됨

        USkill_EnergyField proj = GameManager.Game.Pool.GetPlayerProj(projType, instantPoint.position, instantPoint.rotation).GetComponent<USkill_EnergyField>();
        proj.SetAddParameter(cycleDelay);
        proj.SetProjParameter(projSpd, dmgRate, liveTime, size); //발사체의 속도와 데미지, 폭발의 시간과 크기
    }

    
}

