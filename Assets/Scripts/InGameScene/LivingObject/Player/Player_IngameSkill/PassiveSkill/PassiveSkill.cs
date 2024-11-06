using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PassiveSkill : InGameSkill
{
    public float CurrentEffectValue => SkillLevels[curSkillLevel].DmgRate; // ���� ȿ�� ��
    protected PlayerStat pStat => PlayerMain.pStat;
    public override void SkillReset()
    {
        SkillLevels = new Dictionary<int, Skill_LevelValue>();
        type = SkillType.Passive;
        curSkillLevel = 0;
    }

    //����� ��ų�ڵ尡 ���ǵǾ��ִ��� üũ
    public override void SetLevel()
    {
       // Debug.Log($"{SkillCode}�� ���������� ����");
        foreach (Skill_LevelValue skill in DataManager.skill.GetData(SkillCode).levels)
        {
            SkillLevels.Add(skill.level, skill);
        }

    }

    public override void LevelUp()
    {
        if (curSkillLevel < SkillLevels.Count - 1)
        {
            curSkillLevel++;
            ApplyEffect();
        }
    }

    public virtual void ApplyEffect()
    {
        Debug.Log(CurrentEffectValue);
        //�� ��ų���� ����ġ �ݿ�
    }
}
