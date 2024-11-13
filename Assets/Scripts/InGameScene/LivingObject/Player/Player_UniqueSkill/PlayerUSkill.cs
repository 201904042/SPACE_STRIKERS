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
  
    public bool isSkillActivating; //현재 스패셜 공격이 실행중인가
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
        Debug.Log("유니크 설정 완료");
    }

    public UniqueSkill GetUniqueSkill(int charId)
    {
        UniqueSkill targetSkill;
        uniqueSkills.TryGetValue(charId, out targetSkill);
        if (targetSkill == null) {
            Debug.LogError("해당 캐릭터의 id가 설정된 스킬이 없음");
        }

        return targetSkill;

    }

    public IEnumerator SpecialFire(int charId,int powLevel) //컨트롤러의 키입력함수에 사용
    {
        Debug.Log($"{pStat.curPlayerID}의 {powLevel}레벨 특수 스킬");

        UniqueSkill targetSkill = GetUniqueSkill(charId);
        isSkillActivating = true;
        yield return StartCoroutine(targetSkill.ActivateSkillCoroutine(powLevel));
        isSkillActivating = false;
    }


}
