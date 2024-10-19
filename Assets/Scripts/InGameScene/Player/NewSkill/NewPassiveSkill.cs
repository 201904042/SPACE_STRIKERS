using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PassiveSkillLevel
{
    public float EffectValue; // ȿ�� �� (���ݷ�, ����, �̵� �ӵ� ��)
}

public class NewPassiveSkill : InGameSkill
{
    public List<PassiveSkillLevel> SkillLevels = new List<PassiveSkillLevel>();
    private int currentLevel = 0; // ���� ����

    public float CurrentEffectValue => SkillLevels[currentLevel].EffectValue; // ���� ȿ�� ��

    public override void LevelUp()
    {
        if (currentLevel < SkillLevels.Count - 1)
        {
            currentLevel++;
        }
    }

    public void ApplyEffect(PlayerStat playerStats)
    {
        // ���� ������ ȿ���� �÷��̾��� ���ȿ� �ݿ�
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
