using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UniqueSkill : InGameSkill
{
    protected Skill_LevelValue CurSkillValue => SkillLevels[currentLevel - 1];
    public Transform instantPoint;
    public Coroutine skillCoroutine;
    protected PlayerProjType projType;

    protected int projNum;  //발사체 개수
    protected int projSpeed; //발사체의 속도
    protected int dmgRate; //데미지율
    protected float liveTime; //발동 시간
    protected float range; //크기

    public override void Init()
    {
        SkillLevels = new List<Skill_LevelValue>();
        instantPoint = GameManager.Instance.myPlayer.transform;
        skillCoroutine = null;
        type = SkillType.Unique;
        currentLevel = 1;
    }

    public override void SetLevel()
    {
        Debug.Log($"{SkillCode}의 레벨데이터 세팅");
    }

    public override void LevelUp()
    {
        //레벨업의 의미 없음
    }

    protected void SkillParameterSet()
    {
        Skill_LevelValue skillData = SkillLevels[currentLevel - 1];
        projNum = skillData.ProjNum;
        projSpeed = skillData.ProjSpeed;
        liveTime = skillData.LiveTime;
        dmgRate = skillData.DamageRate;
        range = skillData.Range;
    }


    public virtual IEnumerator ActivateSkillCoroutine()
    {
        ActivateSkill(); // 스킬 발동
        //스킬 발동중
        yield return new WaitForSeconds(liveTime); // 쿨타임 동안 대기
        //스킬발동 종료
    }

    /// <summary>
    /// 각 스킬의 실질적인 수행
    /// </summary>
    public virtual void ActivateSkill()
    {
        // 발사체 생성 코드
        Debug.Log($"유니크 스킬 발동");
    }

    ///// <summary>
    ///// 발사체가 랜덤한 적을 겨냥하도록
    ///// </summary>
    ///// <returns></returns>
    //protected Vector2 DirectionToRandomEnemy()
    //{
    //    Debug.Log($"현재 활성화된 적의 수: {GameManager.Instance.Spawn.activeEnemyList.Count}");

    //    if (GameManager.Instance.Spawn.activeEnemyList.Count == 0)
    //    {
    //        return Vector2.up; // 적이 없을 경우 기본 방향
    //    }

    //    int randomIndex = Random.Range(0, GameManager.Instance.Spawn.activeEnemyList.Count);
    //    GameObject target = GameManager.Instance.Spawn.activeEnemyList[randomIndex];

    //    Vector2 direction = (target.transform.position - instantPoint.position).normalized;
    //    Debug.Log($"방향: {direction}");
    //    return direction;
    //}

}
