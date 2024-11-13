using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class UniqueSkill : ActiveSkill
{
    //curSkillLevel�� ������� ����. PSpecial���� ����Ҷ� pstat�� �Ŀ������� �ش� ��ų�� ������ ����
    public int useCharCode;
    public bool isActive;
    public override void SkillReset()
    {
        SkillLevels = new Dictionary<int, Skill_LevelValue>();
        instantPoint = PlayerMain.Instance.transform;
        type = SkillType.Unique;
        isActive = false;
    }

    public override void SetLevel()
    {
        //Debug.Log($"{SkillCode}�� ���������� ����");
        if(DataManager.skill.GetData(SkillCode).levels.Count == 0)
        {
            Debug.LogError("��ų�����Ͱ� ����");
        }

        foreach (Skill_LevelValue skill in DataManager.skill.GetData(SkillCode).levels)
        {
            SkillLevels.Add(skill.level, skill);
        }
    }

    public override void LevelUp()
    {
        //�������� �ǹ� ����
    }

    //pSpecial�� ���� ����
    public override IEnumerator ActivateSkillCoroutine(int level)
    {
        if (level <= 0)
        {
            yield break;
        }
        isActive = true;
        SkillParameterSet(level); //������ �޾ƿ� ���� �Ķ���͸� ����
        ActivateSkill(level); // ��ų �ߵ�
        yield return new WaitForSeconds(liveTime); // ��Ÿ�� ���� ��� todo => ��ȿ�� üũ�غ���
        isActive = false;
    }

    /// <summary>
    /// �� ��ų�� �������� ����
    /// </summary>
    protected override void ActivateSkill(int level)
    {
        Debug.Log($"����ũ ��ų �ߵ�");
    }
}
