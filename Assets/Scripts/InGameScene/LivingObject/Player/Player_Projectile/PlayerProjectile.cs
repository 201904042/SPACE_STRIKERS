using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Timeline;

public class PlayerProjectile : Projectile 
{
    public PlayerStat playerStat => PlayerMain.pStat; //플레이어의 데미지를 불러올 스텟
    [SerializeField] protected List<GameObject> hittedEnemyList; //isHitOnce가 false라면 충돌한 적을 저장
    protected Coroutine damaging;

    [SerializeField] protected bool isHitOnce;  //해당 발사체가 한번의 타격만을 다루는지 : default = true 기본적으로 한번의 데미지 처리 수행
    protected bool isShootingObj; //발사되는지 아니면 플레이어를 따라다니는지


    protected virtual void Awake()
    {
        projScaleInstance = transform.localScale; //발사체의 원래 크기를 저장
        
        ResetProj();
    }

    protected virtual void OnDisable()
    {
        //비활성화 시 초기화
        ResetProj();
    }
    protected virtual void Update()
    {
        if (!isParameterSet)
        {
            Debug.Log("파라미터가 설정되지 않음");
            return;
        }

        if (isShootingObj)
        {
            MoveUp();
        }
        else
        {
            FollowPlayer();
        }
    }

    #region 설정 및 초기화
    protected override void ResetProj()
    {
        hittedEnemyList = new List<GameObject>();

        damageRate = 0;
        speed = 0;
        liveTime = 0;
        range = 0;
        finalDamage = 0;

        if(activated != null)
        {
            StopCoroutine(activated);
            activated = null;
        }
        if (damaging != null)
        {
            StopCoroutine(damaging);
            damaging = null;
        }

        transform.localScale = projScaleInstance;

        isParameterSet = false;
        isHitOnce = true;
        isShootingObj = true; 
    }

    public virtual void SetAddParameter(float value1, float value2 = 0, float value3 = 0, float value4 = 0)
    {
        //Debug.Log("서브 파라미터 세팅");
    }

    // -> 필수 상속
    public virtual void SetProjParameter(int _projSpeed, int _dmgRate, float _liveTime, float _range)
    {
        //Debug.Log("메인 파라미터 세팅");

        if (_projSpeed == 0)
        {
            isShootingObj = false;
        }
        else
        {
            isShootingObj = true;
            speed = _projSpeed;
        }

        //발사체의 데미지 : 필수 파라미터. 없으면 해당 발사체는 성립되지 않음. 추후 예외 : 플레이어 디버프로 dmg감소가 들어왔다
        if (_dmgRate <= 0)
        {
            Debug.Log("데미지가 0임");
            finalDamage = 0;
        }
        else
        {
            damageRate = _dmgRate;
            finalDamage = finalDamage = (int)playerStat.IG_Dmg * damageRate / 100; //기본 최종 데미지 구조. 수정사항은 개인 덮어쓰기로
        }

        //발사체의 생성시간 : 발사체는 생성시간(초) 후엔 삭제됨. 없다면 발사체는 특정 조건까지 사라지지 않음
        if (_liveTime != 0)
        {
            liveTime = _liveTime; //생성된 발사체가 가 이 시간후에 자동으로 파괴됨
            StartCoroutine(LiveTimer(liveTime));
        }

        //발사체의 크기 : 없다면 프리팹의 기본 크기. 주로 필드에 쓰임. x,y크기가 같은 객체. 예외상황에 대한 조치가 미흡. todo=> 프리팹크기 * range로 바꾸기
        if (_range == 0)
        {
            range = 1;
        }
        else
        {
            range = _range;
            transform.localScale = projScaleInstance * range;
        }

        isParameterSet = true;
    }

    protected virtual IEnumerator LiveTimer(float activeTime)
    {
        yield return new WaitForSeconds(activeTime);

        GameManager.Game.Pool.ReleasePool(gameObject);
    }
    #endregion

    #region 발사체 이동
    protected void FollowPlayer()
    {
        transform.position = PlayerMain.Instance.transform.position;
    }

    protected void MoveUp()
    {
        if (isParameterSet)
        {
            transform.position += transform.up * speed * Time.deltaTime;
        }
    }
    #endregion

    #region 타격 방식
    //범위형 데미지 로직. 범위내 적 1회씩 타격을 사이클 시간마다 반복
    protected virtual IEnumerator AreaDamageLogic(float _cycleRate)
    {
        while (true)
        {
            if(hittedEnemyList.Count == 0)
            {
                yield return new WaitForSeconds(_cycleRate);
                continue;
            }
            MultiEnemyDamage(); // 기존 데미지 로직 호출

            yield return new WaitForSeconds(_cycleRate); // 주기적으로 데미지 적용
        }
    }

    //충돌한 적 1체만 데미지 이후 파괴로직 수행
    protected virtual void SingleEnemyDamage()
    {
        if(hittedEnemyList.Count <= 0)
        {
            return;
        }

        GameObject enemy = hittedEnemyList[0];
        enemy.GetComponent<EnemyObject>().EnemyDamaged(gameObject, (int)finalDamage);
        if (isHitOnce)
        {
            GameManager.Game.Pool.ReleasePool(gameObject);
        }
    }

    //범위 내 적 1회씩 타격
    protected virtual void MultiEnemyDamage()
    {
        for (int i = hittedEnemyList.Count - 1; i >= 0; i--)
        {
            if (hittedEnemyList.Count == 0)
            {
                break; // 리스트가 비어 있으면 반복문 종료
            }

            GameObject enemy = hittedEnemyList[i];

            if (!enemy.activeSelf)
            {
                continue;
            }
            else
            {
                enemy.GetComponent<EnemyObject>().EnemyDamaged(gameObject, finalDamage); // 적에게 데미지
            }
        }
    }


    //실행할 데미지루틴의 코루틴을 시작시킬것 -> 필수 상속
    protected virtual void TriggedEnemy(Collider2D collision)
    {
        //기본 데미지의 실행로직
        if (collision.GetComponent<EnemyObject>() == null)
        {
            Debug.Log("적 스크립트가 맞지 유효하지 않음");
            return;
        }
        if (!hittedEnemyList.Contains(collision.gameObject))
        {
            hittedEnemyList.Add(collision.gameObject);
        }

    }
    #endregion




    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BulletBorder")
        {
            GameManager.Game.Pool.ReleasePool(gameObject);
        }

        if (collision.gameObject.tag == "Enemy")
        {
            TriggedEnemy(collision);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision) //영역 밖으로 나갔다면 리스트에서 적 제거
    {
        if (hittedEnemyList.Contains(collision.gameObject))
        {
            hittedEnemyList.Remove(collision.gameObject);
        }
        
    }
}
