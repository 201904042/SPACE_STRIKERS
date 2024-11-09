using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LauncherLevel
{
    public int level; //현재 슈터의 레벨
    public int DamageRate; //슈터의 레벨별 데미지
    public float Delay; //슈터의 레벨별 대기시간
    public int ProjSpeed; //슈터가 발사하는 객체의 속도
}

public class LauncherStat : MonoBehaviour
{
    //플레이어의 기본 총알, 미사일, 호밍미사일
    protected PlayerStat pStat;
    protected PlayerControl pControl;
    protected PlayerProjType projType; //발사할 타입 -> 필수 상속

    protected float projFireDelay; //발사속도
    protected float attackInterval; //최종 발사 주기(초)  = 플레이어 공격속도 = 무기별 기본 공격속도 / (1+(플레이어 공격속도 - 10(기준)) / 10(기준))
    protected int projDamageRate; //발사체의 데미지 비율 100, 150, 50
    protected int projSpeed; //발사체의 속도. 총알 10, 미사일 5, 호밍15

  
    protected float pAtkSpd => pStat.IG_ASpd;
    protected bool playerReady => PlayerMain.Instance.isOnAttack; //플레이어에서 쏠 준비가 되었나
 
    protected bool isReadyToAttack; //런쳐가 쏠 준비가 되었나

    protected Coroutine LaunchCoroutine;

    protected virtual void Awake()
    {
        //컴포넌트 세팅
        pStat = PlayerMain.pStat;
        pControl = PlayerMain.pControl;
        projFireDelay = 0;
        attackInterval = 0;
        projSpeed = 0;
        projDamageRate = 0;

        isReadyToAttack = false;
        if (LaunchCoroutine != null)
        {
            LaunchCoroutine = null;
        }
    }

    protected virtual void Start()
    {
        SetLauncher();
    }

    //필수 상속
    protected virtual void SetLauncher()
    {
        //무기별 스텟 설정
    }

    protected virtual IEnumerator AttackRoutine(float delay)
    {
        while (true)
        {
            if (!PlayerMain.Instance.isOnAttack) 
            {
                yield return null;
            }

            Fire();
            yield return new WaitForSeconds(delay);
        }
    }

    //필수 상속
    protected virtual void Fire()
    {
        //Debug.Log($"{nameof(projType)}가 발사됨");
        //GameManager.Game.Pool.GetPlayerProj(projType, transform.position, transform.rotation);
    }

    

}
