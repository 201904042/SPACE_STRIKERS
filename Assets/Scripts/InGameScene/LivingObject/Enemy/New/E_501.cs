using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class E_501 : E_TroopBase
{
    protected float curProjAngle;
    private float projAngleOffset;

    private void Awake() //���� ���̵� �ο�
    {
        id = 501;
    }
    
    private void OnEnable()
    {
        SetEnemy();
        SetStopLine(3);
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
        projAngleOffset = 5;
        atkCount = 3; //�⺻ 3�� increaseRate�� ���� �� ������
    }


    protected override void FireProjectile()
    {
        curProjAngle = curProjNum * projAngleOffset;
        FireMulti(OtherProjType.Enemy_Bullet, damage, 0, 0, curProjNum, curProjAngle);
    }
}
