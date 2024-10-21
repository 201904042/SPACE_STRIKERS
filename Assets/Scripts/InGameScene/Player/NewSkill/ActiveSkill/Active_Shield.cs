using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active_Shield : NewActiveSkill
{
    public bool isInit = false;

    public override void Init()
    {
        base.Init();
        SkillCode = 607;
        projType = SkillProjType.Skill_Missile;
        SetLevel();
        // ��ų �ʱ�ȭ �ڵ� (��: ��ų ���� ����)
        Debug.Log("Active_Shield �ʱ�ȭ �Ϸ�");
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
            ProjNum = 1, //��ø �ǵ差
            Cooldown = 20, //����� ��Ÿ��
            DamageRate = 100, //�浹�� ������
        };
        SkillLevels.Add(lv1);

        Skill_LevelValue lv2 = new Skill_LevelValue()
        {
            ProjNum = 1, //��ø �ǵ差
            Cooldown = 18, //����� ��Ÿ��
            DamageRate = 100, //�浹�� ������
        };
        SkillLevels.Add(lv2);

        Skill_LevelValue lv3 = new Skill_LevelValue()
        {
            ProjNum = 1, //��ø �ǵ差
            Cooldown = 16, //����� ��Ÿ��
            DamageRate = 100, //�浹�� ������
        };
        SkillLevels.Add(lv3);

        Skill_LevelValue lv4 = new Skill_LevelValue()
        {
            ProjNum = 2, //��ø �ǵ差
            Cooldown = 16, //����� ��Ÿ��
            DamageRate = 150, //�浹�� ������
        };
        SkillLevels.Add(lv4);

        Skill_LevelValue lv5 = new Skill_LevelValue()
        {
            ProjNum = 2, //��ø �ǵ差
            Cooldown = 14, //����� ��Ÿ��
            DamageRate = 150, //�浹�� ������
        };
        SkillLevels.Add(lv5);

        Skill_LevelValue lv6 = new Skill_LevelValue()
        {
            ProjNum = 2, //��ø �ǵ差
            Cooldown = 12, //����� ��Ÿ��
            DamageRate = 150, //�浹�� ������
        };
        SkillLevels.Add(lv6);

        Skill_LevelValue lv7 = new Skill_LevelValue()
        {
            ProjNum = 3, //��ø �ǵ差
            Cooldown = 10, //����� ��Ÿ��
            DamageRate = 200, //�浹�� ������
        };
        SkillLevels.Add(lv7);
        Debug.Log($"{SkillCode}�� ���� {SkillLevels.Count}�� ���");
    }
}