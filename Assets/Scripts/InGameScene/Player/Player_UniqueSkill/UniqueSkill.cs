using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class UniqueSkill : ActiveSkill
{
    //curSkillLevel�� ������� ����. PSpecial���� ����Ҷ� pstat�� �Ŀ������� �ش� ��ų�� ������ ����
    public int useCharCode;

    public override void SkillReset()
    {
        SkillLevels = new Dictionary<int, Skill_LevelValue>();
        instantPoint = PlayerMain.Instance.transform;
        type = SkillType.Unique;
    }

    public override void SetLevel()
    {
        Debug.Log($"{SkillCode}�� ���������� ����");
    }

    public override void LevelUp()
    {
        //�������� �ǹ� ����
    }

    //pSpecial�� ���� ����
    public override IEnumerator ActivateSkillCoroutine(int level)
    {
        if (level != 0)
        {
            SkillParameterSet(level); //������ �޾ƿ� ���� �Ķ���͸� ����
            ActivateSkill(level); // ��ų �ߵ�
                                  //��ų �ߵ���

            yield return new WaitForSeconds(liveTime); // ��Ÿ�� ���� ��� todo => ��ȿ�� üũ�غ���
                                                       //��ų�ߵ� ����
        }
    }

    /// <summary>
    /// �� ��ų�� �������� ����
    /// </summary>
    protected override void ActivateSkill(int level)
    {
        Debug.Log($"����ũ ��ų �ߵ�");
    }
}
