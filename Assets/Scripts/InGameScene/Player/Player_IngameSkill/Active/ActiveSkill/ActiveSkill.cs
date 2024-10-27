using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ActiveSkill : InGameSkill
{
    protected Skill_LevelValue CurSkillValue => SkillLevels[currentLevel];
    public Transform instantPoint;
    public Coroutine skillCoroutine;
    protected PlayerProjType projType;

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
    protected bool isShield;
    protected float shieldCount;

    public override void Init()
    {
        SkillLevels = new Dictionary<int, Skill_LevelValue>();
        instantPoint = PlayerMain.Instance.gameObject.transform;
        skillCoroutine = null;
        type = SkillType.Active;
        currentLevel = 1;
    }

    public override void SetLevel()
    {
        Debug.Log($"{SkillCode}의 레벨데이터 세팅");
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
        Skill_LevelValue skillData = SkillLevels[currentLevel];
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
        isShield = false;

        foreach (S_EffectValuePair effectValue in skillData.AdditionalEffects)
        {
            switch (effectValue.effect)
            {
                case SkillAddEffect.Slow: isSlow = true; slowRate = effectValue.value; break;
                case SkillAddEffect.SlowExtraDamage: isSlowExtraDmg = true; slowDmgRate = effectValue.value; break;
                case SkillAddEffect.Penetrate: isPenetrate = true; penetrateCount = effectValue.value; break;
                case SkillAddEffect.CycleDamage: isCycleDmg = true; cycleDelay = effectValue.value; break;
                case SkillAddEffect.Drone: isDrone = true; droneAtkSpd = effectValue.value; break;
                case SkillAddEffect.Shield: isShield = true; shieldCount = effectValue.value; break;
            }
        }
}

    //반복 스킬 발동 코루틴
    public virtual IEnumerator ActivateSkillCoroutine()
    {
        while (true)
        {
            ActivateSkill(); // 스킬 발동
            yield return new WaitForSeconds(CurSkillValue.Cooldown); // 쿨타임 동안 대기
        }
    }

    /// <summary>
    /// 각 스킬의 실질적인 수행
    /// </summary>
    public virtual void ActivateSkill()
    {
        // 발사체 생성 코드
        Debug.Log($"액티브스킬 {SkillCode}의 발사체 생성");
    }

    /// <summary>
    /// 발사체가 랜덤한 적을 겨냥하도록
    /// </summary>
    /// <returns></returns>
    protected Vector2 DirectionToRandomEnemy()
    {
        Debug.Log($"현재 활성화된 적의 수: {GameManager.Instance.Spawn.activeEnemyList.Count}");

        if (GameManager.Instance.Spawn.activeEnemyList.Count == 0)
        {
            return Vector2.up; // 적이 없을 경우 기본 방향
        }

        int randomIndex = Random.Range(0, GameManager.Instance.Spawn.activeEnemyList.Count);
        GameObject target = GameManager.Instance.Spawn.activeEnemyList[randomIndex];

        Vector2 direction = (target.transform.position - instantPoint.position).normalized;
        Debug.Log($"방향: {direction}");
        return direction;
    }

    
}
