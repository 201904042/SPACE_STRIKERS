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
        // ���� Ŭ�������� ������ ����ϴ� �޼��带 ȣ��
        InitializeAttackPatterns();
        yield return StartCoroutine(AppearMove()); //������ ������ ���� ���
        //StartCoroutine(RepeatMove()); //��ġ�� �����ϸ� �ݺ������� �翷�̵�

        isEliminatable = true; //������ �Ϸ��ϸ� ���Ű� ������
        isDamageable = true;

        while (curHp > 0)
        {
            if (!isAttack)
            {
                ExecuteNextPattern(); // ���� ���� ����
            }

            yield return new WaitForSeconds(1f);
        }
    }

    protected virtual IEnumerator AppearMove() //������ �����Ҷ� ������
    {
        yield return null;
    }

    protected virtual IEnumerator RepeatMove() //�������� ���������� �ݺ��ؼ� ������ ����
    {
        yield return null;
    }

    protected virtual void InitializeAttackPatterns()
    {
        //���� ���� (���� Ŭ�������� override �ؼ� ���ϸ���Ʈ�� ���� �ֱ�)
    }

    public override void EnemyDamaged(GameObject hitObject, int damage)
    {
        base.EnemyDamaged(hitObject, damage);

        if (curHp <= maxHp * 0.5f && phase == 1) //hp�� ���ݿ� �ٴٸ��� ���� ����� 1�̶�� ������ ��ȯ -> ���� ��ũ��Ʈ�� �̵�
        {
            ChangePhase(2);
        }
    }

    private void ChangePhase(int newPhase)
    {
        phase = newPhase;
        Debug.Log("������ ������ " + phase + "�� ��ȯ�Ǿ����ϴ�.");
    }

    private void ExecuteNextPattern()
    {
        if (attackPatterns.Count == 0) return;

        attackPatterns[currentPatternIndex].Invoke(); // ���� ���� ����
        currentPatternIndex = (currentPatternIndex + 1) % attackPatterns.Count; // ���� �������� �̵�
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerStat>().PlayerDamaged(damage, gameObject);
        }
    }
}
