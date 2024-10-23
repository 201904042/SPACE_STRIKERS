using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillAddEffect{
    None,
    Slow, //�ش� �߻�ü�� ���� ���� ������, �������� ����
    SlowExtraDamage, //�ش� �߻�ü�� ���� ���� ���ο� ���¶�� �߰�������, �߰���%
    Penetrate, //�ش� �߻�ü�� ����� , ��� ���� ��������
    CycleDamage, //�� ��ü�� �浹�ص� ������� ������ ���������� �������� �� ,���ʸ��� �������� ����
    Drone , //����� ��ȯ�Ͽ� ����, ����� ���ݼӵ�(�ð���)
}

public class S_EffectValuePair
{
    public SkillAddEffect effect;
    public float value;

    public S_EffectValuePair(SkillAddEffect effect, float value)
    {
        this.effect = effect;
        this.value = value;
    }
}

public class Skill_LevelValue
{
    public int ProjNum; // ����ü ��. ��ġ��ŭ �ݺ��Ͽ� ����
    public int ProjSpeed; //����ü�� �ӵ�. ���� 10(�÷��̾��� �⺻�ӵ�). 20�̸� �÷��̾� 2��ӵ� 5�� �÷��̾�� �ݹ� ����
    public float Cooldown; // ��Ÿ�� (�ð� ��)
    public int DamageRate; // ������ ��� %. �÷��̾��� ������ * ����������. �÷��̾� ������ 10�̰� ������������ 120%��� �������� 12
    public float Range; // ����(�߻�ü�� ���� ������), �ַ� ������ ��ų�� ����
    public float LiveTime; //���ӽð�(�ð� ��). �� ���ӽð��� ������ �߻�ü�� �ı�
    public List<S_EffectValuePair> AdditionalEffects; // �߰� ȿ�� (��ȭ��, ����, ���� ��)

    public Skill_LevelValue()
    {
        AdditionalEffects = new List<S_EffectValuePair>();
    }
}

public class NewActiveSkill : InGameSkill
{
    protected Skill_LevelValue CurSkillValue => SkillLevels[currentLevel-1];
    public Transform instantPoint;
    public Coroutine skillCoroutine;
    protected SkillProjType projType;

    protected int projNum;
    protected int projSpeed;
    protected int dmgRate;
    protected float cooldown;
    protected float liveTime;
    protected float range;

    protected bool isSlow;
    protected float slowRate;
    protected bool isSlowExtraDmg;
    protected float slowDmgRate;
    protected bool isPenetrate;
    protected float penetrateCount;
    protected bool isCycleDmg;
    protected float cycleDelay;
    protected bool isDrone;
    protected float droneAtkSpd;

    public override void Init()
    {
        SkillLevels = new List<Skill_LevelValue>();
        instantPoint = GameManager.Instance.myPlayer.transform;
        skillCoroutine = null;
        type = SkillType.Active;
        currentLevel = 1;
    }

    public override void SetLevel()
    {
        Debug.Log($"{SkillCode}�� ���������� ����");
    }

    public override void LevelUp()
    {
        if (currentLevel < SkillLevels.Count)
        {
            currentLevel++;
        }

        SkillParameterSet();
    }

    protected void SkillParameterSet()
    {
        Skill_LevelValue skillData = SkillLevels[currentLevel - 1];
        projNum = skillData.ProjNum;
        projSpeed = skillData.ProjSpeed;
        cooldown = skillData.Cooldown;
        dmgRate = skillData.DamageRate;
        range = skillData.Range;
        liveTime = skillData.LiveTime;

        isSlow = false;
        isSlowExtraDmg = false;
        isPenetrate = false;
        isCycleDmg = false;
        isDrone = false;

        foreach (S_EffectValuePair effectValue in skillData.AdditionalEffects)
        {
            switch (effectValue.effect)
            {
                case SkillAddEffect.Slow: isSlow = true; slowRate = effectValue.value; break;
                case SkillAddEffect.SlowExtraDamage: isSlowExtraDmg = true; slowDmgRate = effectValue.value; break;
                case SkillAddEffect.Penetrate: isPenetrate = true; penetrateCount = effectValue.value; break;
                case SkillAddEffect.CycleDamage: isCycleDmg = true; cycleDelay = effectValue.value; break;
                case SkillAddEffect.Drone: isDrone = true; droneAtkSpd = effectValue.value; break;
            }
        }
}

    //�ݺ� ��ų �ߵ� �ڷ�ƾ
    public IEnumerator ActivateSkillCoroutine()
    {
        while (true)
        {
            ActivateSkill(); // ��ų �ߵ�
            yield return new WaitForSeconds(CurSkillValue.Cooldown); // ��Ÿ�� ���� ���
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
