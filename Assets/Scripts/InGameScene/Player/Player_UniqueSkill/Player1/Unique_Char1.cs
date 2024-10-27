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
        useCharCode = 101;
        projType = PlayerProjType.Spcial_Player1;
        SetLevel();
        Debug.Log("Unique_Char1 �ʱ�ȭ �Ϸ�");
    }

    public override void LevelUp()
    {
        //������ ������ ����
    }

    private float space = 0.75f;

    public override void ActivateSkill()
    {
        base.ActivateSkill();
        currentLevel = PlayerMain.pStat.IG_curPowerLevel;
        SkillParameterSet();

        float spawnXpos = GetInitialSpawnXpos();
        float spawnYpos = -4.5f;

        for (int i = 0; i < projNum; i++)
        {
            Vector3 spawnPosition = new Vector3(spawnXpos + (i * space), spawnYpos, 0f);
            Troop troop = GameManager.Instance.Pool.GetPlayerProj(PlayerProjType.Spcial_Player1, spawnPosition, instantPoint.rotation).GetComponent<Troop>();
            troop.SetProjParameter(projSpeed, dmgRate, liveTime, range);
        }
    }

    // �ʱ� spawnXpos�� ����ϴ� �޼���
    private float GetInitialSpawnXpos()
    {
        float spawnXpos = instantPoint.position.x - (currentLevel - 1) * space;

        // X ��ġ�� -2.5���� 2.5 ���̷� ����
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
            level = 1,
            ProjNum = 1,
            ProjSpeed = 5,
            LiveTime = 10,
            DamageRate = 50
        };

        SkillLevels.Add(lv1.level, lv1);

        Skill_LevelValue lv2 = new Skill_LevelValue()
        {
            level = 2,
            ProjNum = 3,
            ProjSpeed = 5,
            Cooldown = 10,
            DamageRate = 75
        };
        SkillLevels.Add(lv2.level, lv2);

        Skill_LevelValue lv3 = new Skill_LevelValue()
        {
            level = 3,
            ProjNum = 5,
            ProjSpeed = 5,
            Cooldown = 10,
            DamageRate = 100
        };
        SkillLevels.Add(lv3.level, lv3);

        Debug.Log($"{SkillCode}�� ���� {SkillLevels.Count}�� ���");
    }

}
