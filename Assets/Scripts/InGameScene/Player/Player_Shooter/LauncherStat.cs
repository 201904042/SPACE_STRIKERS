using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LauncherStat : MonoBehaviour
{
    public const float bulletBaseInterval = 2f;  // 각 기본 발사 주기
    public const float MissileBaseInterval = 3f; // 총알은 2초에 한번. 미사일은 3초에 한번. 호밍은 2초에 한번
    public const float HomingBaseInterval = 1f;  // 

    public const int bulletBaseDamageRate = 100;  // 각 기본 데미지 증가율
    public const int MissileBaseDamageRate = 150; // 최종 데미지  = 플레이어 공격력 + (플공 * 증가율)
    public const int ExplosionBaseDamageRate = 80;
    public const int HomingBaseDamageRate  = 30;

    public const int bulletBaseSpeed= 10;
    public const int MissileBaseSpeed = 5;
    public const int HomingBaseSpeed = 15;

    public const float ExplosionBaseLiveTime = 1; // 1초 고정일듯
    public const float ExplosionBaseRange = 1; //-> 크기 1 -> 1.5 -> 2

    //플레이어의 기본 총알, 미사일, 호밍미사일
    protected PlayerStat pStat;
    protected PlayerControl pControl;
    protected PlayerProjType projType; //발사할 타입 -> 필수 상속

    protected float projFireDelay; //발사속도
    protected float attackInterval; //최종 발사 주기(초)  = 플레이어 공격속도 = 무기별 기본 공격속도 / (1+(플레이어 공격속도 - 10(기준)) / 10(기준))
    protected int projDamageRate; //발사체의 데미지 비율 100, 150, 50
    protected int projSpeed; //발사체의 속도. 총알 10, 미사일 5, 호밍15

  
    protected float pAtkSpd => pStat.attackSpeed;
    protected bool playerReady => pStat.CanAttack; //플레이어에서 쏠 준비가 되었나
 
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
            if (!pStat.CanAttack || !isReadyToAttack) 
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
        Debug.Log($"{nameof(projType)}가 발사됨");
        //GameManager.Instance.Pool.GetPlayerProj(projType, transform.position, transform.rotation);
    }

    

}
