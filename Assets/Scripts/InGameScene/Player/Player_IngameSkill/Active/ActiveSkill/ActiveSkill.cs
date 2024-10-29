using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ActiveSkill : InGameSkill
{
    protected Skill_LevelValue CurSkillValue => SkillLevels[curSkillLevel];
    public Transform instantPoint;
    protected PlayerProjType projType;

    //�⺻ �Ķ����
    protected int projCount;
    protected int projSpd;
    protected int dmgRate;
    protected float coolTime;
    protected float liveTime;
    protected float size;

    #region Ư�� �Ķ����
    protected bool isSlow;
    protected float slowRate;

    protected bool isSlowExtraDmg;
    protected float slowDmgRate;

    protected bool isPenetrate;
    protected float penetrateCount;

    protected bool isCycleDmg;
    protected float cycleDelay;

    protected bool isDrone;
    protected float dAtkSpd;
    protected float dAtkRange;

    protected bool isShield;
    protected float shieldCount;

    protected bool isExplosion;
    protected float expDmg;
    protected float expLiveTime;
    protected float expSize;
    #endregion

    public override void SkillReset()
    {
        SkillLevels = new Dictionary<int, Skill_LevelValue>();
        instantPoint = PlayerMain.Instance.gameObject.transform;
        type = SkillType.Active;
        curSkillLevel = 1;
    }

    public override void SetLevel()
    {
        Debug.Log($"{SkillCode}�� ���������� ����");
    }

    public override void LevelUp()
    {
        if (curSkillLevel < SkillLevels.Count)
        {
            curSkillLevel++;
        }

        SkillParameterSet(curSkillLevel);
    }

    //�⺻��ų������ �������� ����, ����ũ��ų�� �ߵ� �� ����
    protected virtual void SkillParameterSet(int level)
    {
        Skill_LevelValue skillData = SkillLevels[level];
        projCount = skillData.ProjCount;
        projSpd = skillData.ProjSpd;
        coolTime = skillData.CoolTime;
        dmgRate = skillData.DmgRate;
        size = skillData.Size;
        liveTime = skillData.LiveTime;

        isSlow = false;
        isSlowExtraDmg = false;
        isPenetrate = false;
        isCycleDmg = false;
        isDrone = false;
        isShield = false;
        isExplosion = false;

        foreach (S_EffectValuePair effectValue in skillData.AddEffect)
        {
            switch (effectValue.effect)
            {
                case SkillAddEffect.Slow: isSlow = true; 
                    slowRate = effectValue.value1; 
                    break;
                case SkillAddEffect.SlowExtraDamage: isSlowExtraDmg = true; 
                    slowDmgRate = effectValue.value1; 
                    break;
                case SkillAddEffect.Penetrate: isPenetrate = true; 
                    penetrateCount = effectValue.value1; 
                    break;
                case SkillAddEffect.CycleDamage: isCycleDmg = true; 
                    cycleDelay = effectValue.value1; 
                    break;
                case SkillAddEffect.Drone: isDrone = true; 
                    dAtkSpd = effectValue.value1;
                    dAtkRange = effectValue.value2;
                    break;
                case SkillAddEffect.Shield: isShield = true; 
                    shieldCount = effectValue.value1; 
                    break;
                case SkillAddEffect.Explosion: isExplosion = true; 
                    expDmg = effectValue.value1; 
                    expLiveTime = effectValue.value2; 
                    expSize = effectValue.value3; 
                    break;
            }
        }
}

    //�ݺ� ��ų �ߵ� �ڷ�ƾ. �⺻��ų�� �Ķ������ level�����ϸ� curSkillLevel�� ���
    public virtual IEnumerator ActivateSkillCoroutine(int level = 0)
    {
        while (true)
        {
            ActivateSkill(level); // ��ų �ߵ�
            yield return new WaitForSeconds(CurSkillValue.CoolTime); // ��Ÿ�� ���� ���
        }
    }

    /// <summary>
    /// �� ��ų�� �������� ����
    /// </summary>
    protected virtual void ActivateSkill(int level)
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
