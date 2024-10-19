using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillAddEffect{
    None,
    Aim,
    Slow,
    Penetrate
}

public struct Skill_LevelValue
{
    public string Description; //��ų ����
    public int ProjNum; // ����ü ��
    public int ProjSpeed;
    public float Cooldown; // ��Ÿ��
    public float DamageRate; // ������ ���
    public float Range; // ����
    public float LiveTime; //���ӽð�
    public SkillAddEffect AdditionalEffect; // �߰� ȿ�� (��ȭ��, ����, ���� ��)
    public SkillProjType projType;
}

public class NewActiveSkill : InGameSkill
{
    public List<Skill_LevelValue> SkillLevels = new List<Skill_LevelValue>();
    public int currentLevel = 1; // ���� ����
    protected Skill_LevelValue CurSkillValue => SkillLevels[currentLevel-1];
    public Transform instantPoint;
    public Coroutine skillCoroutine;
    
    public override void Init()
    {
        instantPoint = GameManager.Instance.myPlayer.transform;
        skillCoroutine = null;
    }

    public override void SetLevel()
    {
        Debug.Log($"{SkillCode}�� ���������� ����");
    }

    public override void LevelUp()
    {
        if (currentLevel < SkillLevels.Count - 1)
        {
            currentLevel++;
        }
    }

    //��ų ȹ��� ���� ���� ���ᵿ�� ActivateSkill�� ����Ͽ� �ݺ�
    public IEnumerator ActivateSkillCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(CurSkillValue.Cooldown); // ��Ÿ�� ���� ���

            ActivateSkill(); // ��ų �ߵ�
        }
    }

    /// <summary>
    /// �� ��ų�� �������� ����
    /// </summary>
    public virtual void ActivateSkill()
    {
        // �߻�ü ���� �ڵ�
        Debug.Log($"��Ƽ�꽺ų {SkillCode}�� �߻�ü ����");
    }

    /// <summary>
    /// �߻�ü�� ������ ���� �ܳ��ϵ���
    /// </summary>
    /// <returns></returns>
    protected Vector2 DirectionToRandomEnemy()
    {
        Debug.Log($"���� Ȱ��ȭ�� ���� ��: {GameManager.Instance.Spawn.activeEnemyList.Count}");

        if (GameManager.Instance.Spawn.activeEnemyList.Count == 0)
        {
            return Vector2.up; // ���� ���� ��� �⺻ ����
        }

        int randomIndex = Random.Range(0, GameManager.Instance.Spawn.activeEnemyList.Count);
        GameObject target = GameManager.Instance.Spawn.activeEnemyList[randomIndex];

        Vector2 direction = (target.transform.position - instantPoint.position).normalized;
        Debug.Log($"����: {direction}");
        return direction;
    }

    
}
