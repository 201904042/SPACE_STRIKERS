using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Unique_Char1 : UniqueSkill
{
    public override void Init()
    {
        base.Init();
        SkillCode = 691;
        projType = PlayerProjType.Spcial_Player1;
        SetLevel();
        currentLevel = PlayerMain.pStat.powerLevel;
        SkillParameterSet();
        Debug.Log("Active_ChargeShot 초기화 완료");
    }

    public override void LevelUp()
    {
        base.LevelUp(); // 부모 클래스의 LevelUp 호출
    }

    private float space = 0.75f;

    public override void ActivateSkill()
    {
        base.ActivateSkill();
        float spawnXpos = GetInitialSpawnXpos();
        float spawnYpos = -4.5f;

        for (int i = 0; i < projNum; i++)
        {
            Vector3 spawnPosition = new Vector3(spawnXpos + (i * space), spawnYpos, 0f);
            Troop troop = GameManager.Instance.Pool.GetPlayerProj(PlayerProjType.Spcial_Player1, spawnPosition, instantPoint.rotation).GetComponent<Troop>();
            troop.SetProjParameter(projSpeed, dmgRate, liveTime, range);
        }
    }

    // 초기 spawnXpos를 계산하는 메서드
    private float GetInitialSpawnXpos()
    {
        float spawnXpos = instantPoint.position.x - (currentLevel - 1) * space;

        // X 위치를 -2.5에서 2.5 사이로 제한
        if (spawnXpos < -2.5f)
        {
            spawnXpos = -2.5f;
        }
        else if (instantPoint.position.x + (currentLevel - 1) * space > 2.5f)
        {
            spawnXpos = 3.25f - space * projNum;
        }

        return spawnXpos;
    }


    public override void SetLevel()
    {
        Skill_LevelValue lv1 = new Skill_LevelValue()
        {
            ProjNum = 1,
            ProjSpeed = 5,
            LiveTime = 10,
            DamageRate = 50
        };

        SkillLevels.Add(lv1);

        Skill_LevelValue lv2 = new Skill_LevelValue()
        {
            ProjNum = 3,
            ProjSpeed = 5,
            Cooldown = 10,
            DamageRate = 75
        };
        SkillLevels.Add(lv2);

        Skill_LevelValue lv3 = new Skill_LevelValue()
        {
            ProjNum = 5,
            ProjSpeed = 5,
            Cooldown = 10,
            DamageRate = 100
        };
        SkillLevels.Add(lv3);

        Debug.Log($"{SkillCode}의 레벨 {SkillLevels.Count}개 등록");
    }

}
