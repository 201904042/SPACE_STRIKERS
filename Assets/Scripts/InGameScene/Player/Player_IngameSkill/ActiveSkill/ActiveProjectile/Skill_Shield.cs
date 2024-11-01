using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Skill_Shield : PlayerProjectile
{
    private PlayerStat pStat => PlayerMain.pStat;

    protected override void ResetProj()
    {
        base.ResetProj();
    }

    public override void SetProjParameter(int _projSpeed, int _dmgRate, float _liveTime, float _range)
    {
        base.SetProjParameter(_projSpeed, _dmgRate, _liveTime, _range);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (pStat.InvincibleState)
        {
            return;
        }
        if (collision.CompareTag("Enemy"))
        {
            pStat.PlayerKnockBack(collision); //���尡 �ջ�ɰ�� �÷��̾�� �˹�ȿ��
            TriggedEnemy(collision);
        }

        if (collision.CompareTag("Enemy_Projectile"))
        {
            pStat.PlayerKnockBack(collision); //���尡 �ջ�ɰ�� �÷��̾�� �˹�ȿ��
            GameManager.Game.Pool.ReleasePool(gameObject);
        }
    }

    protected override void TriggedEnemy(Collider2D collision)
    {
        base.TriggedEnemy(collision);
        SingleEnemyDamage();
    }
}
