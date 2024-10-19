using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NewPassiveSkill : InGameSkill
{
    public List<Skill_LevelValue> SkillLevels = new List<Skill_LevelValue>();
    public int currentLevel = 0; // 현재 레벨

    public float CurrentEffectValue => SkillLevels[currentLevel-1].DamageRate; // 현재 효과 값
    
    public override void Init()
    {
        
    }

    public override void SetLevel()
    {
        Debug.Log($"{SkillCode}의 레벨데이터 세팅");
    }

    public override void LevelUp()
    {
        if (currentLevel < SkillLevels.Count - 1)
        {
            currentLevel++;
            ApplyEffect();
        }
    }

    public void ApplyEffect()
    {
        PlayerStat playerStats = GameManager.Instance.myPlayer.GetComponent<PlayerStat>();
        // 현재 레벨의 효과를 플레이어의 스탯에 반영
        switch (SkillCode)
        {
            case 641:
                playerStats.damageIncreaseRate += CurrentEffectValue;
                break;
            case 642:
                playerStats.defenceIncreaseRate += CurrentEffectValue;
                break;
            case 643:
                playerStats.moveSpeedIncreaseRate += CurrentEffectValue;
                break;
            case 644:
                playerStats.attackSpeedIncreaseRate += CurrentEffectValue;
                break;
            case 645:
                playerStats.hpRegenRate += CurrentEffectValue;
                break;
        }
        playerStats.ApplyStat();
    }
}
