using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class E_BossBase : EnemyObject
{
    protected int phase = 1;
    protected int currentPatternIndex = 0;
    protected List<Action> attackPatterns = new List<Action>();

    protected override IEnumerator EnemyBehavior()
    {
        // 하위 클래스에서 패턴을 등록하는 메서드를 호출
        InitializeAttackPatterns();
        yield return StartCoroutine(AppearMove()); //등장이 끝날때 까지 대기
        //StartCoroutine(RepeatMove()); //위치에 도착하면 반복적으로 양옆이동

        isEliminatable = true; //등장이 완료하면 제거가 가능함
        isDamageable = true;

        while (curHp > 0)
        {
            if (!isAttack)
            {
                ExecuteNextPattern(); // 다음 패턴 실행
            }

            yield return new WaitForSeconds(1f);
        }
    }

    protected virtual IEnumerator AppearMove() //생성시 등장할때 움직임
    {
        yield return null;
    }

    protected virtual IEnumerator RepeatMove() //등장이후 보스전동안 반복해서 움직일 패턴
    {
        yield return null;
    }

    protected virtual void InitializeAttackPatterns()
    {
        //예시 패턴 (하위 클래스에서 override 해서 패턴리스트에 패턴 넣기)
    }

    public override void EnemyDamaged(GameObject hitObject, int damage)
    {
        base.EnemyDamaged(hitObject, damage);

        if (curHp <= maxHp * 0.5f && phase == 1) //hp가 절반에 다다르고 현재 페이즈가 1이라면 페이즈 전환 -> 하위 스크립트로 이동
        {
            ChangePhase(2);
        }
    }

    private void ChangePhase(int newPhase)
    {
        phase = newPhase;
        Debug.Log("보스가 페이즈 " + phase + "로 전환되었습니다.");
    }

    private void ExecuteNextPattern()
    {
        if (attackPatterns.Count == 0) return;

        attackPatterns[currentPatternIndex].Invoke(); // 현재 패턴 실행
        currentPatternIndex = (currentPatternIndex + 1) % attackPatterns.Count; // 다음 패턴으로 이동
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerStat>().PlayerDamaged(damage, gameObject);
        }
    }
}
