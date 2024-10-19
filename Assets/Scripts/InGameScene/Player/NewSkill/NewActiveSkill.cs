using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SkillLevel
{
    public int ProjectileCount; // ����ü ��
    public float Cooldown; // ��Ÿ��
    public float DamageMultiplier; // ������ ���
    public float Range; // ����
    public float AdditionalEffect; // �߰� ȿ�� (��ȭ��, ���� �ð� ��)
}

public class NewActiveSkill : InGameSkill
{
    public List<SkillLevel> SkillLevels = new List<SkillLevel>();
    private int currentLevel = 0; // ���� ����

    public override void LevelUp()
    {
        if (currentLevel < SkillLevels.Count - 1)
        {
            currentLevel++;
        }
    }

    public void Activate()
    {
        // �߻�ü ���� �ڵ�
        Debug.Log("��Ƽ�꽺ų {id}�� �߻�ü ���� �õ�");

        SkillLevel currentSkillLevel = SkillLevels[currentLevel];

        for (int i = 0; i < currentSkillLevel.ProjectileCount; i++)
        {
            // ����ü ���� ����
            //��ų�� �������� �÷��̾� ��ġ���� ���� �� �����տ� ���� �ο�

        }

        // ��Ÿ�� ����
        Cooldown = currentSkillLevel.Cooldown;

        // ������ ����
        float damage = /*baseDamage * */ currentSkillLevel.DamageMultiplier;
    }
}
