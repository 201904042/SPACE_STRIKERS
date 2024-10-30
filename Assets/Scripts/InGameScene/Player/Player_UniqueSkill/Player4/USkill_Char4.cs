using System.Collections;
using UnityEngine;

public class USkill_Char4 : UniqueSkill
{
    public override void SkillReset()
    {
        base.SkillReset();
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
        SkillParameterSet(level); //레벨을 받아온 순간 파라미터를 조정

        int projIndex = 0;
        float projDelay = liveTime / projCount;

        ActivateSkill(level); //초탄 발사
        projIndex++;
        while (projIndex != projCount)
        {
            yield return new WaitForSeconds(projDelay);
            ActivateSkill(level); // 스킬 발동
            projIndex++;
        }
        
    }

    protected override void ActivateSkill(int level)
    {
        base.ActivateSkill(level);

        USkill_TrackingMissile proj = GameManager.Instance.Pool.GetPlayerProj(projType, instantPoint.position, instantPoint.rotation).GetComponent<USkill_TrackingMissile>();
        proj.SetProjParameter(projSpd, dmgRate, 0, size); //여기선 라이브타임을 다른 방식으로 사용
    }

}

