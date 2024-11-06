using System.Collections;
using System.Security.Claims;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class E_TroopBase : EnemyObject
{
    //기본 적 : 커먼 타입, 엘리트 타입
    [SerializeField] protected int stopLine; // 지정된 라인
    [SerializeField] protected int stopInstance; // 스탑라인에 도달한 횟수
    protected int atkCount; //하위 스크립트 설정
    
    public void SetStopLine(int _stopLine)
    {
        stopLine = _stopLine;
        stopInstance = 0;
    }


    public override void ResetObject()
    {
        base.ResetObject();
        curProjNum = defaultProjNum;
        curAtkDelay = defaultAtkDelay;
        stopLine = 0;
        stopInstance = 0;
        isStop = false;
        isAttack = false;
    }

    //EnemyBehaviror을 상속하여 반복문에 행동을 추가
    protected virtual IEnumerator StopEnemyPattern()
    {
        // 스탑라인까지 이동
        if (!isStop)
        {
            Move();
        }
        else
        {
            Debug.Log("멈춘 적 공격");

            // 지정된 공격 횟수만큼 공격
            int countInstance = atkCount;
            while (countInstance > 0)
            {
                yield return StartCoroutine(Attacking());
                countInstance--;
            }

            // 공격이 끝난 후 다시 움직이도록 플래그를 리셋합니다.
            isStop = false;
        }
        
    }

    protected virtual IEnumerator NonStopEnemyPattern()
    {
        Move();
        if (!isAttack)
        {
            StartCoroutine(Attacking());
        }

        yield return null;
    }

    protected virtual IEnumerator Attacking()
    {
        isAttack = true;
        FireProjectile();
        yield return new WaitForSeconds(curAtkDelay);
        isAttack = false;
    }

    //필수 상속
    protected virtual void FireProjectile()
    {
        //적별로 발사 메서드 추가
    }

    private void AddStopCount()
    {
        stopInstance++;
        Debug.Log($"stopLine{stopInstance}");
        if (stopLine == stopInstance)
        {
            isStop = true; // 지정된 라인에서 멈춤
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Enemy_StopZone"))
        {
            AddStopCount();
        }

    }
}
