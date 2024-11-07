using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Active_Shield : ActiveSkill
{
    private Skill_Shield instantShield;

    public override void SkillReset()
    {
        base.SkillReset();
        SkillCode = 607;
        projType = PlayerProjType.Skill_Shield;
        SetLevel(); 
        instantShield = null;
    }

    //쉴드는 다른 것과 달리 1개만 생성하며 쉴드가 파괴되었을때를 기점으로 쿨타임이 돌아감

    public override void LevelUp()
    {
        base.LevelUp(); // 부모 클래스의 LevelUp 호출
        if (instantShield != null) {
            GameManager.Game.Pool.ReleasePool(instantShield.gameObject); // 레벨업시 기존 스킬 파괴 및 바로 재생성
            instantShield = null;
        }
        
        ActivateSkill(curSkillLevel);
    }

    public override IEnumerator ActivateSkillCoroutine(int level)
    {
        //ActivateSkill(curSkillLevel); // 스킬 발동
        while (true)
        {
            if (instantShield != null) 
            {
                yield return null;
                continue;
            }

            yield return new WaitForSeconds(CurSkillValue.CoolTime); // 쿨타임 동안 대기
            ActivateSkill(curSkillLevel); // 스킬 발동
        }
    }

    /// <summary>
    /// 각 스킬의 실질적인 수행
    /// </summary>
    protected override void ActivateSkill(int level)
    {
        // 발사체 생성 코드
        if(instantShield != null)
        {
            return;
        }
        Skill_Shield proj = GameManager.Game.Pool.GetPlayerProj(projType, instantPoint.position, instantPoint.rotation).GetComponent<Skill_Shield>();
        instantShield = proj;
        proj.SetProjParameter(0, dmgRate, 0, size);
    }


    public override void SetLevel()
    {
        foreach(Skill_LevelValue skill in DataManager.skill.GetData(SkillCode).levels)
        {
            SkillLevels.Add(skill.level, skill);
        }

    }
}