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

public class PlayerProjectile : MonoBehaviour
{
    public PlayerStat playerStat; //플레이어의 데미지를 불러올 스텟
    [SerializeField] protected bool isParameterSet; //파라미터가 설정됨? 설정되어야 움직임 : default =false


    [SerializeField] protected int damageRate;
    [SerializeField] protected int speed;
    [SerializeField] protected float liveTime;
    [SerializeField] protected float range;

    protected int finalDamage; //플레이어의 스텟과 발사체의 데미지증폭을 곱하여 최종적으로 적용할 데미지
    [SerializeField] protected bool isHitOnce;  //해당 발사체가 한번의 타격만을 다루는지 : default = true 기본적으로 한번의 데미지 처리 수행
    [SerializeField] protected List<GameObject> hittedEnemyList; //isHitOnce가 false라면 충돌한 적을 저장
    //todo -> 적리뉴얼후 적 스크립트로 바꿔보기

    protected bool isShootingObj; //발사되는지 아니면 플레이어를 따라다니는지

    protected Coroutine activated;
    protected Coroutine damaging;

    protected virtual void Awake()
    {
        playerStat = GameManager.Instance.myPlayer.GetComponent<PlayerStat>();
        ResetProj();
    }

    protected virtual void OnDisable()
    {
        //비활성화 시 초기화
        ResetProj();
    }

    protected virtual void ResetProj()
    {
        hittedEnemyList = new List<GameObject>();
        isParameterSet = false;

        damageRate = 0;
        speed = 0;
        liveTime = 0;
        range = 0;
        finalDamage = 0; 
        isHitOnce = true;
        isShootingObj = false;
        activated = null;
        damaging= null;
    }

    protected virtual void Update()
    {
        if (isShootingObj) 
        {
            MoveUp();
        }
        else
        {
            FollowPlayer();
        }
    }

    protected void FollowPlayer()
    {
        transform.position = GameManager.Instance.myPlayer.transform.position;
    }

    protected void MoveUp()
    {
        if (isParameterSet)
        {
            transform.position += transform.up * speed * Time.deltaTime;
        }
    }

    public virtual void SetAddParameter(float value1, float value2 =0, float value3 = 0)
    {
        Debug.Log("서브 파라미터 세팅");
    }

    // -> 필수 상속
    public virtual void SetProjParameter(int _projSpeed,int _dmgRate, float _liveTime, float _range)
    {
        Debug.Log("메인 파라미터 세팅");
        isParameterSet = true;

        if (_projSpeed == 0)
        {
            isShootingObj = false;
        }
        else
        {
            isShootingObj = true;
            speed = _projSpeed;
        }

        if (_dmgRate == 0)
        {
            Debug.LogError("데미지가 설정되지 않음");
        }
        else
        {
            damageRate = _dmgRate;
            finalDamage = finalDamage = (int)playerStat.damage * damageRate / 100; //기본 최종 데미지 구조. 수정사항은 개인 덮어쓰기로
        }

        if (_liveTime != 0)
        {
            liveTime = _liveTime; //생성된 발사체가 가 이 시간후에 자동으로 파괴됨
            StartCoroutine(LiveTimer(liveTime));
        }

        if (_range == 0)
        {
            Debug.Log("기본 크기");
        }
        else
        {
            range = _range;
            transform.localScale = new Vector3(range, range, 0);
        }
    }

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
        GameObject enemy = hittedEnemyList[0];
        enemy.GetComponent<EnemyObject>().EnemyDamaged(finalDamage, gameObject);
        hittedEnemyList.RemoveAt(0);
        if (isHitOnce)
        {
            GameManager.Instance.Pool.ReleasePool(gameObject);
        }
    }

    //범위 내 적 1회씩 타격
    protected virtual void MultiEnemyDamage()
    {
        for (int i = hittedEnemyList.Count - 1; i >= 0; i--)
        {
            GameObject enemy = hittedEnemyList[i];

            if (!enemy.activeSelf)
            {
                hittedEnemyList.RemoveAt(i); // 비활성화된 적 제거
            }
            else
            {
                enemy.GetComponent<EnemyObject>().EnemyDamaged(finalDamage, gameObject); // 적에게 데미지
                if (isHitOnce)
                {
                    hittedEnemyList.RemoveAt(i);
                }
            }
        }
    }


    protected virtual IEnumerator LiveTimer(float activeTime)
    {
        yield return new WaitForSeconds(activeTime);
        
        GameManager.Instance.Pool.ReleasePool(gameObject);
    }


    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BulletBorder")
        {
            GameManager.Instance.Pool.ReleasePool(gameObject);
        }

        if (collision.gameObject.tag == "Enemy")
        {
            TriggedEnemy(collision);
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

    protected virtual void OnTriggerExit2D(Collider2D collision) //영역 밖으로 나갔다면 리스트에서 적 제거
    {
        if (hittedEnemyList.Contains(collision.gameObject))
        {
            hittedEnemyList.Remove(collision.gameObject);
        }
        
    }
}
