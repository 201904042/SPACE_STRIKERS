using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class UniqueSkill : ActiveSkill
{
    //curSkillLevel을 사용하지 않음. PSpecial에서 사용할때 pstat의 파워레벨을 해당 스킬의 레벨로 설정
    public int useCharCode;

    public override void SkillReset()
    {
        SkillLevels = new Dictionary<int, Skill_LevelValue>();
        instantPoint = PlayerMain.Instance.transform;
        type = SkillType.Unique;
    }

    public override void SetLevel()
    {
        Debug.Log($"{SkillCode}의 레벨데이터 세팅");
    }

    public override void LevelUp()
    {
        //레벨업의 의미 없음
    }

    //pSpecial에 의해 사용됨
    public override IEnumerator ActivateSkillCoroutine(int level)
    {
        if (level != 0)
        {
            SkillParameterSet(level); //레벨을 받아온 순간 파라미터를 조정
            ActivateSkill(level); // 스킬 발동
                                  //스킬 발동중

            yield return new WaitForSeconds(liveTime); // 쿨타임 동안 대기 todo => 유효성 체크해보기
                                                       //스킬발동 종료
        }
    }

    /// <summary>
    /// 각 스킬의 실질적인 수행
    /// </summary>
    protected override void ActivateSkill(int level)
    {
        Debug.Log($"유니크 스킬 발동");
    }
}
