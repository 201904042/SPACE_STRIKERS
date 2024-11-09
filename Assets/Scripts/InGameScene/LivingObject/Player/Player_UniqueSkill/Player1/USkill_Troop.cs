using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Troop : PlayerProjectile
{
    GameObject player;
    private Coroutine behaviorCoroutine;
    private bool isSkillEnd;
    private bool isAttack;
    private float normalSpeed;
    private float slowSpeed = 0.1f;
    private float slowPointY = -2f;
    protected override void ResetProj()
    {
        base.ResetProj();

        player = PlayerMain.Instance.gameObject;
        isSkillEnd = false;
        normalSpeed = speed; // 초기 속도를 저장합니다.

        if (behaviorCoroutine != null)
        {
            StopCoroutine(behaviorCoroutine);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        //사용완료 슈터 제거
        Transform instantTransform = transform.GetChild(0);
        for (int i = instantTransform.childCount - 1; i >= 0; i--)
        {
            Destroy(instantTransform.GetChild(i).gameObject);
        }

    }

    public override void SetProjParameter(int _projSpeed, int _dmgRate, float _liveTime, float _range)
    {
        ResetProj();

        Debug.Log("드론 파라미터 세팅");
        isParameterSet = true;
        isShootingObj = true;
        speed = _projSpeed;
        normalSpeed = speed;

        if (_dmgRate == 0)
        {
            Debug.LogError("데미지가 설정되지 않음");
        }
        else
        {
            damageRate = _dmgRate;
        }

        if (_liveTime != 0)
        {
            liveTime = _liveTime;
            StartCoroutine(LiveTimer(liveTime));
        }

        behaviorCoroutine = StartCoroutine(TroopBehavior());
    }

    protected override IEnumerator LiveTimer(float activeTime)
    {
        yield return new WaitForSeconds(activeTime);
        isSkillEnd = true;
    }

    private IEnumerator TroopBehavior()
    {
        float speed = normalSpeed;
        while (true)
        {
            // 일정 위치(y = -2)에 도달하면 속도를 느리게 함
            if (transform.position.y >= slowPointY)
            {
                speed = slowSpeed;
            }

            if(isSkillEnd) //스킬이 끝나면 원래속도로
            {
                speed = normalSpeed;
            }

            transform.position += transform.up * speed * Time.deltaTime;

            yield return null;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "TroopBorder")
        {
            GameManager.Game.Pool.ReleasePool(gameObject);
        }

        if (collision.CompareTag("Enemy_Projectile"))
        {
            EnemyBullet enemyBullet = collision.GetComponent<EnemyBullet>();
            EnemySplitBullet enemySplitBullet = collision.GetComponent<EnemySplitBullet>();
            if (enemyBullet != null || enemySplitBullet != null)
            {
                GameManager.Game.Pool.ReleasePool(collision.gameObject);
            }
        }
    }

    

    protected override void Update()
    {
        //업데이트를 사용하지 않음
    }

    public int GetDamageRate()
    {
        return damageRate;
    }
}