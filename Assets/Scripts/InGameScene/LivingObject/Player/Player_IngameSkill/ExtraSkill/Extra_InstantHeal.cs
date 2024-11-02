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
        // ��ų �ʱ�ȭ �ڵ� (��: ��ų ���� ����)
        Debug.Log("Extra_InstantHeal �ʱ�ȭ �Ϸ�");
    }

    public override void ApplyEffect()
    {
        PlayerMain.Instance.HpRestore(value);
        Debug.Log($"Extra_InstantHeal : ü�� 30%ȸ��");
    }
}
