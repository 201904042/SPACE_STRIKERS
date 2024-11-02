using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extra_InstantHeal : ExtraSkill
{
    public override void SkillReset()
    {
        base.SkillReset();
        SkillCode = 661;
        SetLevel();
        // 스킬 초기화 코드 (예: 스킬 레벨 세팅)
        Debug.Log("Extra_InstantHeal 초기화 완료");
    }

    public override void ApplyEffect()
    {
        PlayerMain.Instance.HpRestore(value);
        Debug.Log($"Extra_InstantHeal : 체력 30%회복");
    }
}
