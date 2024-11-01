using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class USkill_TrackingMissile : PlayerProjectile
{

    private Coroutine moveToTargetBehavior;
    private GameObject target;

    protected override void ResetProj()
    {
        base.ResetProj();
        target = null;
        if (moveToTargetBehavior != null)
        {
            StopCoroutine(moveToTargetBehavior);
            moveToTargetBehavior = null;
        }
    }

    protected override void Update()
    {
        //부모의 Update를 사용하지 않음
    }

    public override void SetProjParameter(int _projSpeed, int _dmgRate, float _liveTime, float _range)
    {
        base.SetProjParameter(_projSpeed, _dmgRate, _liveTime, _range);
        if (moveToTargetBehavior == null)
        {
            moveToTargetBehavior = StartCoroutine(MoveToTargetEnemy());
        }
    }

    private IEnumerator MoveToTargetEnemy()
    {
        while (true)
        {
            if (target == null)
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
             MoveUp();

            yield return null;
        }
    }

    protected GameObject SetTarget()
    {
        Debug.Log($"현재 활성화된 적의 수: {GameManager.Game.Spawn.activeEnemyList.Count}");

        if (GameManager.Game.Spawn.activeEnemyList.Count == 0)
        {
            return null;
        }

        int randomIndex = UnityEngine.Random.Range(0, GameManager.Game.Spawn.activeEnemyList.Count);
        GameObject target = GameManager.Game.Spawn.activeEnemyList[randomIndex];

        return target;
    }

   

    protected override void TriggedEnemy(Collider2D collision)
    {
        base.TriggedEnemy(collision);
        SingleEnemyDamage();
    }

}

