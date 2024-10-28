using System.Collections;
using UnityEngine;

public class USkill_Char4 : UniqueSkill
{
    public override void Init()
    {
        base.Init();
        SkillCode = 694;
        useCharCode = 104;
        projType = PlayerProjType.Spcial_Player4;
        SetLevel();
        Debug.Log("USkill_Char4 초기화 완료");
    }

    public override void LevelUp()
    {
        //레벨업 개념이 없음
    }

    public override IEnumerator ActivateSkillCoroutine(int level)
    {
        isSkillActive = true;
        SkillParameterSet(level); //레벨을 받아온 순간 파라미터를 조정

        int projIndex = 0;
        float projDelay = liveTime / projNum;

        ActivateSkill(level); //초탄 발사
        projIndex++;
        while (projIndex != projNum)
        {
            yield return new WaitForSeconds(projDelay);
            ActivateSkill(level); // 스킬 발동
            projIndex++;
        }
        
        isSkillActive = false;
    }

    public override void ActivateSkill(int level)
    {
        base.ActivateSkill(level);

        USkill_TrackingMissile proj = GameManager.Instance.Pool.GetPlayerProj(projType, instantPoint.position, instantPoint.rotation).GetComponent<USkill_TrackingMissile>();
        proj.SetProjParameter(projSpeed, dmgRate, 0, range); //여기선 라이브타임을 다른 방식으로 사용
    }

    public override void SetLevel()
    {
        Skill_LevelValue lv1 = new Skill_LevelValue()
        {
            level = 1,
            ProjNum = 10,
            ProjSpeed = 10,
            DamageRate = 100,
            LiveTime = 5f //5초안에 전부발사
        };

        SkillLevels.Add(lv1.level, lv1);

        Skill_LevelValue lv2 = new Skill_LevelValue()
        {
            level = 2,
            ProjNum = 15,
            ProjSpeed = 10,
            DamageRate = 120,
            LiveTime = 5f
        };
        SkillLevels.Add(lv2.level, lv2);

        Skill_LevelValue lv3 = new Skill_LevelValue()
        {
            level = 3,
            ProjNum = 25,
            ProjSpeed = 10,
            DamageRate = 150,
            LiveTime = 5f
        };
        SkillLevels.Add(lv3.level, lv3);

        Debug.Log($"{SkillCode}의 레벨 {SkillLevels.Count}개 등록");
    }
}

