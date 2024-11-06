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
        // ��ų �ʱ�ȭ �ڵ� (��: ��ų ���� ����)
        //Debug.Log("Extra_RewardUp �ʱ�ȭ �Ϸ�");
    }

    public override void ApplyEffect()
    {
        PlayerStat playerStats = PlayerMain.pStat;
        //// ���� ������ ȿ���� �÷��̾��� ���ȿ� �ݿ�
        //maxHp ���ȸ��, ��ȭ ������
        playerStats.IG_RewardRate += value;
        Debug.Log($"Extra_RewardUp : reward���� ���������� : {playerStats.IG_RewardRate}");
    }
}
