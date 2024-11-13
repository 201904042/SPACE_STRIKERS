using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUSkill : MonoBehaviour
{
    Dictionary<int, UniqueSkill> uniqueSkills;

    private PlayerStat pStat => PlayerMain.pStat;
    [HideInInspector]
    public int playerId => pStat.curPlayerID;
    public int powLv => pStat.IG_curPowerLevel;
  
    public bool isSkillActivating; //���� ���м� ������ �������ΰ�
    public void Init()
    {
        isSkillActivating = false;
        uniqueSkills = new Dictionary<int, UniqueSkill>();
        SetUniques();
    }

    private void SetUniques()
    {
        USkill_Char1 char1 = new USkill_Char1();
        char1.SkillReset();
        uniqueSkills.Add(char1.useCharCode, char1);
        USkill_Char2 char2 = new USkill_Char2();
        char2.SkillReset();
        uniqueSkills.Add(char2.useCharCode, char2);
        USkill_Char3 char3 = new USkill_Char3();
        char3.SkillReset();
        uniqueSkills.Add(char3.useCharCode, char3);
        USkill_Char4 char4 = new USkill_Char4();
        char4.SkillReset();
        uniqueSkills.Add(char4.useCharCode, char4);
        Debug.Log("����ũ ���� �Ϸ�");
    }

    public UniqueSkill GetUniqueSkill(int charId)
    {
        UniqueSkill targetSkill;
        uniqueSkills.TryGetValue(charId, out targetSkill);
        if (targetSkill == null) {
            Debug.LogError("�ش� ĳ������ id�� ������ ��ų�� ����");
        }

        return targetSkill;

    }

    public IEnumerator SpecialFire(int charId,int powLevel) //��Ʈ�ѷ��� Ű�Է��Լ��� ���
    {
        Debug.Log($"{pStat.curPlayerID}�� {powLevel}���� Ư�� ��ų");

        UniqueSkill targetSkill = GetUniqueSkill(charId);
        isSkillActivating = true;
        yield return StartCoroutine(targetSkill.ActivateSkillCoroutine(powLevel));
        isSkillActivating = false;
    }


}
