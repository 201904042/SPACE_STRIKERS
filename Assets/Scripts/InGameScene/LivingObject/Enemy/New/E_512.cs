using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class E_512 : E_TroopBase
{
    protected float curProjAngle;
    private float projAngleOffset;

    private void Awake() //최초 아이디 부여
    {
        id = 512;
    }

    private void OnEnable()
    {
        SetEnemy();
    }

    public override void ResetObject()
    {
        base.ResetObject();
        curProjNum = E_DefaultProjNum;
        curAtkDelay = E_DefaultAtkDelay;
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
        curProjNum = E_DefaultProjNum * (increaseRate / 100);
        curAtkDelay = E_DefaultAtkDelay / attackSpeed;
        projAngleOffset = 5;
        curProjAngle = curProjNum * projAngleOffset;
        atkCount = 3; //기본 3번 increaseRate에 따라 더 많아짐
    }


    protected override void FireProjectile()
    {
        EnemyProjectile[] proj = FireMulti(OtherProjType.Enemy_Bullet, curProjNum, curProjAngle);
        foreach (EnemyProjectile p in proj)
        {
            p.SetProjParameter(damage, 0);
        }
    }
}
