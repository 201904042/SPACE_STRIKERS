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
    protected bool isParameterSet; //파라미터가 설정됨? 설정되어야 움직임 : default =false

    //protected bool isSlow;
    //protected int isSlowCount;
    //protected bool isSlowExtraDamage;
    //protected int slowExtraDamageRate;

    //protected bool isPenetrate;
    //protected int penetrateCount;
    //protected int damageCount;

    //protected bool isCycleDamage; //여러번 데미지를 주는 변수 인가 = 장판스킬
    //protected int cycleRate;

    //protected bool isShootable; //해당 스킬이 발사 가능한 스킬인가

    //protected bool isDrone;
    //protected int droneAtkSpd;

    //생성하는 스크립트에서 받을(set) 변수
    protected int damageRate;
    protected int speed;
    protected float liveTime;
    protected float range;

    protected int finalDamage; //플레이어의 스텟과 발사체의 데미지증폭을 곱하여 최종적으로 적용할 데미지
    protected bool isHitOnce;  //해당 발사체가 한번의 타격만을 다루는지 : default = true 기본적으로 한번의 데미지 처리 수행
    protected List<GameObject> hittedEnemyList; //isHitOnce가 false라면 충돌한 적을 저장
    //todo -> 적리뉴얼후 적 스크립트로 바꿔보기



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

        //isSlow = false;
        //isSlowCount = 0;
        //isSlowExtraDamage = false; 
        //slowExtraDamageRate = 0;
        //isPenetrate = false; 
        //penetrateCount = 0;
        //damageCount = 0;
        //isCycleDamage = false;
        //cycleRate = 0;
        //isShootable = false;
        //isDrone = false; ;
        //droneAtkSpd = 0;

        damageRate = 0;
        speed = 0;
        liveTime = 0;
        range = 0;
        finalDamage = 0; 
        isHitOnce = false; 
        hittedEnemyList = new List<GameObject>(); 
    }

    protected void MoveLogic()
    {

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

    protected bool ableToMove;
    protected bool isAreaSkill;
    public virtual void SetProjParameter(int _projSpeed,int _dmgRate, float _liveTime, float _range, float value1 = 0, float value2 = 0)
    {
        Debug.Log("메인 파라미터 세팅");
        isParameterSet = true;

        if (_projSpeed == 0)
        {
            ableToMove = false;
        }
        else
        {
            ableToMove = true;
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
            isAreaSkill = false;
        }
        else
        {
            range = _range;
            transform.localScale = new Vector3(range, range, 0);
            isAreaSkill = true;
        }
    }

    //범위형 데미지 로직. 
    protected virtual IEnumerator AreaDamageLogic()
    {
        while (isCycleDamage)
        {
            MultiEnemyDamage(); // 기존 데미지 로직 호출

            yield return new WaitForSeconds(cycleRate); // 주기적으로 데미지 적용
        }
    }

    //리스트의 한마리의 적만 타격
    protected virtual void SingleEnemyDamage()
    {
        GameObject enemy = hittedEnemyList[0];
        enemy.GetComponent<EnemyObject>().EnemyDamaged(finalDamage, gameObject);
    }

    //리스트에 들어온 여러마리의 적 타격. 사라지지 않으니 라이브 타임으로 조절
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

                // isHitOnce가 true일 경우, 타격 후 적을 리스트에서 제거
                if (isHitOnce)
                {
                    hittedEnemyList.RemoveAt(i);
                }
            }
        }
    }


    protected IEnumerator LiveTimer(float activeTime)
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

    //penetrate필요시 여기에 추가
    protected virtual void TriggedEnemy(Collider2D collision)
    {
        //기본 데미지의 실행로직
        if (collision.GetComponent<EnemyObject>() == null)
        {
            Debug.Log("적 스크립트가 맞지 유효하지 않음");
            return;
        }

        hittedEnemyList.Add(collision.gameObject);
    }

    protected virtual void OnTriggerExit2D(Collider2D collision) //영역 밖으로 나갔다면 리스트에서 적 제거
    {
        if (hittedEnemyList.Contains(collision.gameObject))
        {
            hittedEnemyList.Remove(collision.gameObject);
        }
        
    }
}
