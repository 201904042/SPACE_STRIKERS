using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.MaterialProperty;


public class PlayerBulletLauncher : LauncherStat
{

    protected override void SetLauncher()
    {
        base.SetLauncher();
        projType = PlayerProjType.Player_Bullet;
        projFireDelay = PlayerMain.bulletBaseInterval;
        projDamageRate = PlayerMain.bulletBaseDamageRate;
        projSpeed = PlayerMain.bulletBaseSpeed;
        //���� �߻� �ֱ�(��) = �÷��̾� ���ݼӵ� = ���⺰ �⺻ ���ݼӵ� / (1 + (�÷��̾� ���ݼӵ� - 10(����)) / 10(����))
        attackInterval = projFireDelay / (1 + (float)(pAtkSpd - PlayerStat.BasicStat) / PlayerStat.BasicStat);
        

        isReadyToAttack = true;
        if (LaunchCoroutine == null)
        {
            LaunchCoroutine = StartCoroutine(AttackRoutine(attackInterval));
        }
    }

    protected override void Fire()
    {
        base.Fire();
        PlayerBullet proj = GameManager.Game.Pool.GetPlayerProj(projType, transform.position, transform.rotation).GetComponent<PlayerBullet>();
        proj.SetProjParameter(projSpeed, projDamageRate, 0, 0);
    }
}