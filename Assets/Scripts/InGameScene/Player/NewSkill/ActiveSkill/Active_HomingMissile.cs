using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Active_HomingMissile : NewActiveSkill
{
    public bool isInit = false;

    public override void Init()
    {
        base.Init();
        SkillCode = 604;
        projType = SkillProjType.Skill_Homing;
        SetLevel();
        // ��ų �ʱ�ȭ �ڵ� (��: ��ų ���� ����)
        Debug.Log("Active_HomingMissile �ʱ�ȭ �Ϸ�");
        isInit = true;
    }

    public override void LevelUp()
    {
        base.LevelUp(); // �θ� Ŭ������ LevelUp ȣ��
    }

    public override void ActivateSkill()
    {
        base.ActivateSkill();

        for (int i = 0; i < CurSkillValue.ProjNum; i++)
        {
            skill_Missile missile = GameManager.Instance.Pool.GetSkill(projType, instantPoint.position, instantPoint.rotation).GetComponent<skill_Missile>();
            Vector2 RandomDir = DirectionToRandomEnemy();
            missile.transform.up = RandomDir;
        }
    }


    public override void SetLevel()
    {
        Skill_LevelValue lv1 = new Skill_LevelValue()
        {
            ProjNum = 2,
            ProjSpeed = 20,
            Cooldown = 3,
            DamageRate = 80,
        };
        SkillLevels.Add(lv1);

        Skill_LevelValue lv2 = new Skill_LevelValue()
        {
            ProjNum = 4,
            ProjSpeed = 20,
            Cooldown = 3,
            DamageRate = 80,
        };
        SkillLevels.Add(lv2);

        Skill_LevelValue lv3 = new Skill_LevelValue()
        {
            ProjNum = 4,
            ProjSpeed = 20,
            Cooldown = 2f,
            DamageRate = 80,
        };
        SkillLevels.Add(lv3);

        Skill_LevelValue lv4 = new Skill_LevelValue()
        {
            ProjNum = 6,
            ProjSpeed = 20,
            Cooldown = 2f,
            DamageRate = 80,
        };
        SkillLevels.Add(lv4);

        Skill_LevelValue lv5 = new Skill_LevelValue()
        {
            ProjNum = 6,
            ProjSpeed = 20,
            Cooldown = 1f,
            DamageRate = 80,
        };
        SkillLevels.Add(lv5);

        Skill_LevelValue lv6 = new Skill_LevelValue()
        {
            ProjNum = 8,
            ProjSpeed = 20,
            Cooldown = 1f,
            DamageRate = 80,
        };
        SkillLevels.Add(lv6);

        Skill_LevelValue lv7 = new Skill_LevelValue()
        {
            ProjNum = 16,
            ProjSpeed = 20,
            Cooldown = 0.5f,
            DamageRate = 80,
        };
        SkillLevels.Add(lv6);
        Debug.Log($"{SkillCode}�� ���� {SkillLevels.Count}�� ���");
    }
}
