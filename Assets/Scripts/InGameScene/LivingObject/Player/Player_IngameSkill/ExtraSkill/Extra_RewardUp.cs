using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.MaterialProperty;

public class Extra_RewardUp : ExtraSkill
{
    public override void SkillReset()
    {
        base.SkillReset();
        SkillCode = 662;
        SetLevel();
        // 스킬 초기화 코드 (예: 스킬 레벨 세팅)
        //Debug.Log("Extra_RewardUp 초기화 완료");
    }

    public override void ApplyEffect()
    {
        PlayerStat playerStats = PlayerMain.pStat;
        //// 현재 레벨의 효과를 플레이어의 스탯에 반영
        //maxHp 즉시회복, 재화 증가율
        playerStats.IG_RewardRate += value;
        Debug.Log($"Extra_RewardUp : reward증가 현재증가율 : {playerStats.IG_RewardRate}");
    }
}
