using System.Collections;
using System.Security.Claims;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class E_TroopBase : EnemyObject
{
    //�⺻ �� : Ŀ�� Ÿ��, ����Ʈ Ÿ��
    [SerializeField] protected int stopLine; // ������ ����
    [SerializeField] protected int stopInstance; // ��ž���ο� ������ Ƚ��
    protected int atkCount; //���� ��ũ��Ʈ ����
    
    public void SetStopLine(int _stopLine)
    {
        stopLine = _stopLine;
        stopInstance = 0;
    }


    public override void ResetObject()
    {
        base.ResetObject();
        stopLine = 0;
        stopInstance = 0;
        isStop = false;
        isAttack = false;
    }

    //EnemyBehaviror�� ����Ͽ� �ݺ����� �ൿ�� �߰�
    protected virtual IEnumerator StopEnemyPattern()
    {
        // ��ž���α��� �̵�
        if (!isStop)
        {
            Move();
        }
        else
        {
            Debug.Log("���� �� ����");

            // ������ ���� Ƚ����ŭ ����
            int countInstance = atkCount;
            while (countInstance > 0)
            {
                yield return StartCoroutine(Attacking());
                countInstance--;
            }

            // ������ ���� �� �ٽ� �����̵��� �÷��׸� �����մϴ�.
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

    //�ʼ� ���
    protected virtual void FireProjectile()
    {
        //������ �߻� �޼��� �߰�
    }

    private void AddStopCount()
    {
        stopInstance++;
        Debug.Log($"stopLine{stopInstance}");
        if (stopLine == stopInstance)
        {
            isStop = true; // ������ ���ο��� ����
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
