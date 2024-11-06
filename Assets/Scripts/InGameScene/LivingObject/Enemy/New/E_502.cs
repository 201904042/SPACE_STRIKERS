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

    public override void ResetObject()
    {
        base.ResetObject();
        curProjNum = C_DefaultProjNum;
        curAtkDelay = C_DefaultAtkDelay;
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
        curProjNum = C_DefaultProjNum * (increaseRate / 100);
        curAtkDelay = C_DefaultAtkDelay / attackSpeed;
        projAngleOffset = 5;
        curProjAngle = curProjNum * projAngleOffset;
        atkCount = 3; //기본 3번 increaseRate에 따라 더 많아짐
    }


    protected override void FireProjectile()
    {
        FireMulti(OtherProjType.Enemy_Bullet, damage, 0, 0, curProjNum, curProjAngle,true);
    }
}
