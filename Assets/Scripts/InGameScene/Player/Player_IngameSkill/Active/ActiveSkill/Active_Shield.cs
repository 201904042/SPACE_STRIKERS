using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active_Shield : ActiveSkill
{
    private Skill_Shield instantShield;

    public override void Init()
    {
        base.Init();
        SkillCode = 607;
        projType = PlayerProjType.Skill_Shield;
        SetLevel();
        SkillParameterSet();
        instantShield = null;
        // ��ų �ʱ�ȭ �ڵ� (��: ��ų ���� ����)
        Debug.Log("Active_Shield �ʱ�ȭ �Ϸ�");
    }

    //����� �ٸ� �Ͱ� �޸� 1���� �����ϸ� ���尡 �ı��Ǿ������� �������� ��Ÿ���� ���ư�

    public override void LevelUp()
    {
        base.LevelUp(); // �θ� Ŭ������ LevelUp ȣ��
        if (instantShield != null) {
            GameManager.Instance.Pool.ReleasePool(instantShield.gameObject); // �������� ���� ��ų �ı� �� �ٷ� �����
            instantShield = null;
        }
        
        ActivateSkill();
    }

    public override IEnumerator ActivateSkillCoroutine()
    {
        //ActivateSkill(); // ��ų �ߵ�
        while (true)
        {
            if (instantShield != null) 
            {
                yield return null;
                continue;
            }

            yield return new WaitForSeconds(CurSkillValue.Cooldown); // ��Ÿ�� ���� ���
            ActivateSkill(); // ��ų �ߵ�
        }
    }

    /// <summary>
    /// �� ��ų�� �������� ����
    /// </summary>
    public override void ActivateSkill()
    {
        // �߻�ü ���� �ڵ�
        if(instantShield != null)
        {
            return;
        }
        Skill_Shield proj = GameManager.Instance.Pool.GetPlayerProj(projType, instantPoint.position, instantPoint.rotation).GetComponent<Skill_Shield>();
        instantShield = proj;
        proj.SetProjParameter(0, dmgRate, 0, range);
    }


    public override void SetLevel()
    {
        Skill_LevelValue lv1 = new Skill_LevelValue()
        {
            level = 1,
            Cooldown = 20, //����� ��Ÿ��
            DamageRate = 200, //�浹�� ������
            Range = 1
        };
        SkillLevels.Add(lv1.level, lv1);

        Skill_LevelValue lv2 = new Skill_LevelValue()
        {
            level = 2,
            Cooldown = 18, //����� ��Ÿ��
            DamageRate = 200, //�浹�� ������
            Range = 1
        };
        SkillLevels.Add(lv2.level, lv2);

        Skill_LevelValue lv3 = new Skill_LevelValue()
        {
            level = 3,
            Cooldown = 16, //����� ��Ÿ��
            DamageRate = 300, //�浹�� ������
            Range = 1.2f
        };
        SkillLevels.Add(lv3.level, lv3);

        Skill_LevelValue lv4 = new Skill_LevelValue()
        {
            level = 4,
            Cooldown = 16, //����� ��Ÿ��
            DamageRate = 300, //�浹�� ������
            Range = 1.2f
        };
        SkillLevels.Add(lv4.level, lv4);

        Skill_LevelValue lv5 = new Skill_LevelValue()
        {
            level = 5,
            Cooldown = 14, //����� ��Ÿ��
            DamageRate = 400, //�浹�� ������
            Range = 1.5f
        };
        SkillLevels.Add(lv5.level, lv5);

        Skill_LevelValue lv6 = new Skill_LevelValue()
        {
            level = 6,
            Cooldown = 12, //����� ��Ÿ��
            DamageRate = 400, //�浹�� ������
            Range = 1.5f
        };
        SkillLevels.Add(lv6.level, lv6);

        Skill_LevelValue lv7 = new Skill_LevelValue()
        {
            level = 7,
            Cooldown = 10, //����� ��Ÿ��
            DamageRate = 500, //�浹�� ������
            Range = 2f
        };
        SkillLevels.Add(lv7.level, lv7);
        Debug.Log($"{SkillCode}�� ���� {SkillLevels.Count}�� ���");
    }
}