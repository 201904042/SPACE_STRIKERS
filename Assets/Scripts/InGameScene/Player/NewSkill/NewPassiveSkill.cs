using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PassiveSkillLevel
{
    public float EffectValue; // 효과 값 (공격력, 방어력, 이동 속도 등)
}

public class NewPassiveSkill : InGameSkill
{
    public List<PassiveSkillLevel> SkillLevels = new List<PassiveSkillLevel>();
    private int currentLevel = 0; // 현재 레벨

    public float CurrentEffectValue => SkillLevels[currentLevel].EffectValue; // 현재 효과 값

    public override void LevelUp()
    {
        if (currentLevel < SkillLevels.Count - 1)
        {
            currentLevel++;
        }
    }

    public void ApplyEffect(PlayerStat playerStats)
    {
        // 현재 레벨의 효과를 플레이어의 스탯에 반영
        switch (SkillCode)
        {
            case 641:
                playerStats.damage += CurrentEffectValue;
                break;
            case 642:
                playerStats.defence += CurrentEffectValue;
                break;
            case 643:
                playerStats.moveSpeed += CurrentEffectValue;
                break;
            case 644:
                playerStats.attackSpeed += CurrentEffectValue;
                break;
            case 645:
                playerStats.hpRegenRate += CurrentEffectValue;
                break;
        }
    }
}
