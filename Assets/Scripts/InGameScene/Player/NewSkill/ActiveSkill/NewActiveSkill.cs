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
    public string Description; //스킬 설명
    public int ProjNum; // 투사체 수
    public int ProjSpeed;
    public float Cooldown; // 쿨타임
    public float DamageRate; // 데미지 배수
    public float Range; // 범위
    public float LiveTime; //지속시간
    public SkillAddEffect AdditionalEffect; // 추가 효과 (둔화율, 관통, 조준 등)
    public SkillProjType projType;
}

public class NewActiveSkill : InGameSkill
{
    
    protected Skill_LevelValue CurSkillValue => SkillLevels[currentLevel-1];
    public Transform instantPoint;
    public Coroutine skillCoroutine;
    
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
        if (currentLevel < SkillLevels.Count - 1)
        {
            currentLevel++;
        }
    }

    //스킬 획득시 최초 실행 종료동안 ActivateSkill을 계속하여 반복
    public IEnumerator ActivateSkillCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(CurSkillValue.Cooldown); // 쿨타임 동안 대기

            ActivateSkill(); // 스킬 발동
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
