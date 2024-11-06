using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class E_502 : E_TroopBase
{
    protected float curProjAngle;
    private float projAngleOffset;

    private void Awake() //���� ���̵� �ο�
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
            yield return StartCoroutine(base.EnemyBehavior()); // �θ� Ŭ������ �⺻ �ൿ ȣ��
            yield return StartCoroutine(NonStopEnemyPattern()); // �߰� �ൿ ���� ȣ��
        }
    }

    protected override void SetStat()
    {
        base.SetStat();
        curProjNum = C_DefaultProjNum * (increaseRate / 100);
        curAtkDelay = C_DefaultAtkDelay / attackSpeed;
        projAngleOffset = 5;
        curProjAngle = curProjNum * projAngleOffset;
        atkCount = 3; //�⺻ 3�� increaseRate�� ���� �� ������
    }


    protected override void FireProjectile()
    {
        FireMulti(OtherProjType.Enemy_Bullet, damage, 0, 0, curProjNum, curProjAngle,true);
    }
}
