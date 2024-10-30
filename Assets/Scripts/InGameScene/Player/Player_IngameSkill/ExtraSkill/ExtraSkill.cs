using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraSkill : InGameSkill
{
    protected int value;

    public override void SkillReset()
    {
        type = SkillType.Extra;
        curSkillLevel = 1;
    }

    public override void SetLevel()
    {
        //Debug.Log($"{SkillCode}�� ���������� ����");
        
        value = DataManager.skill.GetData(SkillCode).levels[0].DmgRate;
    }

    public override void LevelUp()
    {
        //������ ������� ����
    }

    public virtual void ApplyEffect()
    {
        //��ų ȿ�� �ݿ�
    }
}
