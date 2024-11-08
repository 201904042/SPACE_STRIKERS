using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class E_511 : E_TroopBase
{
    protected float curProjAngle;
    private float projAngleOffset;

    private void Awake() //���� ���̵� �ο�
    {
        id = 511;
    }

    private void OnEnable()
    {
        SetEnemy();
        SetStopLine(3);
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
            yield return StartCoroutine(base.EnemyBehavior()); // �θ� Ŭ������ �⺻ �ൿ ȣ��
            yield return StartCoroutine(StopEnemyPattern());   // �߰� �ൿ ���� ȣ��
        }
    }

    protected override void SetStat()
    {
        base.SetStat();
        curProjNum = E_DefaultProjNum * (increaseRate / 100);
        curAtkDelay = E_DefaultAtkDelay / attackSpeed;
        projAngleOffset = 5;
        curProjAngle = curProjNum * projAngleOffset;
        atkCount = 3; //�⺻ 3�� increaseRate�� ���� �� ������
    }


    protected override void FireProjectile()
    {
        EnemyProjectile[] proj = FireMulti(OtherProjType.Enemy_Bullet, curProjNum, curProjAngle);
        foreach(EnemyProjectile p in proj)
        {
            p.SetProjParameter(damage, 0);
        }
    }
}
