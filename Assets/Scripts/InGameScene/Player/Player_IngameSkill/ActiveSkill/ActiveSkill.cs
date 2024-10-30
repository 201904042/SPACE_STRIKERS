using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ActiveSkill : InGameSkill
{
    protected Skill_LevelValue CurSkillValue => SkillLevels[curSkillLevel];
    public Transform instantPoint;
    protected PlayerProjType projType;

    //기본 파라미터
    protected int projCount;
    protected int projSpd;
    protected int dmgRate;
    protected float coolTime;
    protected float liveTime;
    protected float size;

    #region 특수 파라미터
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
        curSkillLevel = 0;
    }

    //사용전 스킬코드가 정의되어있는지 체크
    public override void SetLevel()
    {
        Debug.Log($"{SkillCode}의 레벨데이터 세팅");
        foreach (Skill_LevelValue skill in DataManager.skill.GetData(SkillCode).levels)
        {
            SkillLevels.Add(skill.level, skill);
        }

    }

    public override void LevelUp()
    {
        if (curSkillLevel < SkillLevels.Count)
        {
            curSkillLevel++;
        }

        SkillParameterSet(curSkillLevel);
    }

    //기본스킬에서는 레벨업시 적용, 유니크스킬은 발동 전 적용
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

    //반복 스킬 발동 코루틴. 기본스킬은 파라미터의 level사용안하며 curSkillLevel을 사용
    public virtual IEnumerator ActivateSkillCoroutine(int level = 0)
    {
        while (true)
        {
            ActivateSkill(level); // 스킬 발동
            yield return new WaitForSeconds(CurSkillValue.CoolTime); // 쿨타임 동안 대기
        }
    }

    /// <summary>
    /// 각 스킬의 실질적인 수행
    /// </summary>
    protected virtual void ActivateSkill(int level)
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
