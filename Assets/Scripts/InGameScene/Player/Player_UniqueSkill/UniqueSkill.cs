using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class UniqueSkill : InGameSkill
{
    public Skill_LevelValue CurSkillValue;
    public Transform instantPoint;
    protected PlayerProjType projType;

    public int useCharCode; //사용하는 캐릭터의 id

    protected int projNum;  //발사체 개수
    protected int projSpeed; //발사체의 속도
    protected int dmgRate; //데미지율
    protected float liveTime; //발동 시간
    protected float range; //크기

    public bool isSkillActive;

    //다른 스킬과 달리 스킬 레벨을 외부에서 받아옴 (주의)

    public override void Init()
    {
        SkillLevels = new Dictionary<int, Skill_LevelValue>();
        instantPoint = PlayerMain.Instance.transform;
        type = SkillType.Unique;
        isSkillActive = false;
    }

    public override void SetLevel()
    {
        Debug.Log($"{SkillCode}의 레벨데이터 세팅");
    }

    public override void LevelUp()
    {
        //레벨업의 의미 없음
    }

    protected virtual void SkillParameterSet(int level)
    {
        CurSkillValue = SkillLevels[level];
        projNum = CurSkillValue.ProjNum;
        projSpeed = CurSkillValue.ProjSpeed;
        liveTime = CurSkillValue.LiveTime;
        dmgRate = CurSkillValue.DamageRate;
        range = CurSkillValue.Range;
        
    }


    public virtual IEnumerator ActivateSkillCoroutine(int level)
    {
        SkillParameterSet(level); //레벨을 받아온 순간 파라미터를 조정
        ActivateSkill(level); // 스킬 발동
        //스킬 발동중
        isSkillActive = true;
        yield return new WaitForSeconds(liveTime); // 쿨타임 동안 대기
        //스킬발동 종료

        isSkillActive = false;
    }

    /// <summary>
    /// 각 스킬의 실질적인 수행
    /// </summary>
    public virtual void ActivateSkill(int level)
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
