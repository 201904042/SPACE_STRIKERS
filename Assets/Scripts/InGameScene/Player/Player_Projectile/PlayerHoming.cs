using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHoming : PlayerProjectile
{
    private Coroutine homingCoroutine;
    private GameObject target;

    protected override void ResetProj()
    {
        base.ResetProj();
        target = null;
        if (homingCoroutine != null) 
        { 
            StopCoroutine(homingCoroutine);
            homingCoroutine = null;
        }
    }

    protected override void Update()
    {
        //�θ��� Update�� ������� ����
    }

    public override void SetProjParameter(int _projSpeed, int _dmgRate, float _liveTime, float _range)
    {
        base.SetProjParameter(_projSpeed, _dmgRate, _liveTime, _range);
        if (homingCoroutine == null)
        {
            homingCoroutine = StartCoroutine(MoveToTargetEnemy());
        }
    }

    private IEnumerator MoveToTargetEnemy()
    {
        while (true)
        {
            

            if(target == null)
            {
                target = SetTarget();
            }
            else
            {
                if (!target.activeSelf)
                {
                    target = SetTarget();
                }
            }

            Vector3 dir = transform.up;

            if (target != null)
            {
                dir = (target.transform.position - transform.position).normalized;
            }

            transform.up = dir;
            transform.position += transform.up * speed * Time.deltaTime;

            yield return null;
        }
    }

    protected GameObject SetTarget()
    {
        Debug.Log($"���� Ȱ��ȭ�� ���� ��: {GameManager.Instance.Spawn.activeEnemyList.Count}");

        if (GameManager.Instance.Spawn.activeEnemyList.Count == 0)
        {
            return null;
        }

        int randomIndex = UnityEngine.Random.Range(0, GameManager.Instance.Spawn.activeEnemyList.Count);
        GameObject target = GameManager.Instance.Spawn.activeEnemyList[randomIndex];

        return target;
    }

    protected override void TriggedEnemy(Collider2D collision)
    {
        base.TriggedEnemy(collision);
        SingleEnemyDamage();
    }
}
