using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Timeline;

public class PlayerProjectile : MonoBehaviour
{
    public PlayerStat playerStat; //플레이어의 데미지를 불러올 스텟
    protected bool isParameterSet; //파라미터가 설정됨? 설정되어야 움직임
    protected bool isAlreadyHit; //동시에 여러객체가 충돌하게 될경우 한개의 객체만 데미지처리를 하도록 보장
    protected int finalDamage; //플레이어의 스텟과 발사체의 데미지증폭을 곱하여 최종적으로 적용할 데미지

    //생성하는 스크립트에서 받을(set) 변수
    protected int damageRate;
    protected int speed;
    protected float liveTime;
    protected float range;

    

    protected virtual void Awake()
    {
        playerStat = GameManager.Instance.myPlayer.GetComponent<PlayerStat>();
        isParameterSet = false;
        isAlreadyHit = false;
    }

    protected virtual void OnDisable()
    {
        //비활성화 시 초기화
        ResetProj();
        
    }

    protected virtual void ResetProj()
    {
        isParameterSet = false;
        isAlreadyHit = false;
        finalDamage = 0;

        damageRate = 0;
        speed = 0;
        liveTime = 0;
        range = 0;
    }

    protected void MoveUp()
    {
        if (isParameterSet)
        {
            transform.position += transform.up * speed * Time.deltaTime;
        }
    }

    public virtual void SetProjParameter(int projSpeed,int dmgRate, float liveTime, float range)
    {
        Debug.Log("메인 파라미터 세팅");
        isParameterSet = true;
    }

    public virtual void SetAddParameter(float value1, float value2 = 0, float value3 = 0)
    {
        Debug.Log("에디 파라미터 세팅");
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BulletBorder")
        {
            GameManager.Instance.Pool.ReleasePool(gameObject);
        }
    }
}
