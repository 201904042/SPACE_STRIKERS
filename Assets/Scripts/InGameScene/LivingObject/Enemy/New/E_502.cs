using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class E_502 : E_TroopBase
{
    protected float curProjAngle;
    private float projAngleOffset;

    private void Awake() //최초 아이디 부여
    {
        id = 502;
    }

    private void OnEnable()
    {
        SetEnemy();
    }

    protected override IEnumerator EnemyBehavior()
    {
        while (true)
        {
            yield return StartCoroutine(base.EnemyBehavior()); // 부모 클래스의 기본 행동 호출
            yield return StartCoroutine(NonStopEnemyPattern()); // 추가 행동 패턴 호출
        }
    }

    protected override void SetStat()
    {
        base.SetStat();
        projAngleOffset = 5;
        atkCount = 3; //기본 3번 increaseRate에 따라 더 많아짐
    }


    protected override void FireProjectile()
    {
        curProjAngle = curProjNum * projAngleOffset;
        FireMulti(OtherProjType.Enemy_Bullet, damage, 0, 0, curProjNum, curProjAngle,true);
    }
}
