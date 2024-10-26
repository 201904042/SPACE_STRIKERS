using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SkillAddEffect
{
    None,
    Slow, //해당 발사체를 맞은 적은 느려짐, 느려지는 정도
    SlowExtraDamage, //해당 발사체를 맞은 적이 슬로우 상태라면 추가데미지, 추가뎀%
    Penetrate, //해당 발사체는 관통됨 , 몇마리 까지 관통할지
    CycleDamage, //이 객체는 충돌해도 사라지지 않으며 지속적으로 데미지를 줌 ,몇초마다 데미지를 줄지
    Drone, //드론을 소환하여 공격, 드론의 공격속도(시간초)
    Shield //쉴드스킬전용, 쉴드의 중첩개수
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

//공용스킬 , 스페셜 스킬
public abstract class InGameSkill
{
    public int SkillCode { get; protected set; }
    public SkillType type { get; protected set; }
    public List<Skill_LevelValue> SkillLevels { get; protected set; }
    public int currentLevel;
    public string description;
    public abstract void Init();
    public abstract void LevelUp();
    public abstract void SetLevel();
}
