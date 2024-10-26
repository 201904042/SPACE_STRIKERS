using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InGamePassiveSkill : InGameSkill
{
    public float CurrentEffectValue => SkillLevels[currentLevel-1].DamageRate; // ���� ȿ�� ��
    
    public override void Init()
    {
        SkillLevels = new List<Skill_LevelValue>();
        type = SkillType.Passive;
        currentLevel = 1;
    }

    public override void SetLevel()
    {
        Debug.Log($"{SkillCode}�� ���������� ����");
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
        PlayerStat playerStats =PlayerMain.pStat;
        // ���� ������ ȿ���� �÷��̾��� ���ȿ� �ݿ�
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
