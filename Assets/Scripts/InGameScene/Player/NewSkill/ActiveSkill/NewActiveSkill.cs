using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillAddEffect{
    None,
    Slow, //해당 발사체를 맞은 적은 느려짐, 느려지는 정도
    SlowExtraDamage, //해당 발사체를 맞은 적이 슬로우 상태라면 추가데미지, 추가뎀%
    Penetrate, //해당 발사체는 관통됨 , 몇마리 까지 관통할지
    CycleDamage, //이 객체는 충돌해도 사라지지 않으며 지속적으로 데미지를 줌 ,몇초마다 데미지를 줄지
    Drone , //드론을 소환하여 공격, 드론의 공격속도(시간초)
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
    public int ProjNum; // 투사체 수. 수치만큼 반복하여 생성
    public int ProjSpeed; //투사체의 속도. 기준 10(플레이어의 기본속도). 20이면 플레이어 2배속도 5면 플레이어보다 반배 느림
    public float Cooldown; // 쿨타임 (시간 초)
    public int DamageRate; // 데미지 배수 %. 플레이어의 데미지 * 데미지비율. 플레이어 데미지 10이고 데미지비율이 120%라면 최종뎀은 12
    public float Range; // 범위(발사체의 로컬 스케일), 주로 범위형 스킬에 쓰임
    public float LiveTime; //지속시간(시간 초). 이 지속시간이 끝나면 발사체는 파괴
    public List<S_EffectValuePair> AdditionalEffects; // 추가 효과 (둔화율, 관통, 조준 등)

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

    //반복 스킬 발동 코루틴
    public IEnumerator ActivateSkillCoroutine()
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
