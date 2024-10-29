using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InGamePassiveSkill : InGameSkill
{
    public float CurrentEffectValue => SkillLevels[curSkillLevel].DmgRate; // ���� ȿ�� ��
    
    public override void SkillReset()
    {
        SkillLevels = new Dictionary<int, Skill_LevelValue>();
        type = SkillType.Passive;
        curSkillLevel = 1;
    }

    public override void SetLevel()
    {
        //Debug.Log($"{SkillCode}�� ���������� ����");
    }

    public override void LevelUp()
    {
        if (curSkillLevel < SkillLevels.Count - 1)
        {
            curSkillLevel++;
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
                playerStats.PS_Dmg += CurrentEffectValue;
                break;
            case 642:
                playerStats.PS_Dfs += CurrentEffectValue;
                break;
            case 643:
                playerStats.PS_MSpd += CurrentEffectValue;
                break;
            case 644:
                playerStats.PS_ASpd += CurrentEffectValue;
                break;
            case 645:
                playerStats.PS_HpRegen += CurrentEffectValue;
                break;
        }
        playerStats.ApplyStat();
    }
}
