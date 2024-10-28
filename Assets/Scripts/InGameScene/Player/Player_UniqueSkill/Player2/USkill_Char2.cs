using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class USkill_Char2 : UniqueSkill
{
    public override void Init()
    {
        base.Init();
        SkillCode = 692;
        useCharCode = 102;
        projType = PlayerProjType.Spcial_Player2;
        SetLevel();
        Debug.Log("USkill_Char2 �ʱ�ȭ �Ϸ�");
    }

    public override void LevelUp()
    {
        //������ ������ ����
    }

    public override void ActivateSkill(int level)
    {
        base.ActivateSkill(level);

        USkill_Bomber proj = GameManager.Instance.Pool.GetPlayerProj(projType, instantPoint.position, instantPoint.rotation).GetComponent<USkill_Bomber>();
        proj.SetProjParameter(projSpeed, dmgRate, liveTime, range); //�߻�ü�� �ӵ��� ������, ������ �ð��� ũ��
    }

    public override void SetLevel()
    {
        Skill_LevelValue lv1 = new Skill_LevelValue()
        {
            level = 1,
            ProjNum = 1,
            ProjSpeed = 5,
            LiveTime = 3,
            DamageRate = 150,
            Range = 2
        };

        SkillLevels.Add(lv1.level, lv1);

        Skill_LevelValue lv2 = new Skill_LevelValue()
        {
            level = 2,
            ProjNum = 1,
            ProjSpeed = 5,
            LiveTime = 5f,
            DamageRate = 300,
            Range = 4
        };
        SkillLevels.Add(lv2.level, lv2);

        Skill_LevelValue lv3 = new Skill_LevelValue()
        {
            level = 3,
            ProjNum = 1,
            ProjSpeed = 5,
            LiveTime = 8,
            DamageRate = 500,
            Range = 6
        };
        SkillLevels.Add(lv3.level, lv3);

        Debug.Log($"{SkillCode}�� ���� {SkillLevels.Count}�� ���");
    }
}
