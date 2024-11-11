using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissileLauncher : LauncherStat
{
    //todo  -> ������ �⺻ ������ ����, ũ�� , ���� ����
    protected override void SetLauncher()
    {
        base.SetLauncher();
        projType = PlayerProjType.Player_Missile;
        projFireDelay = PlayerMain.MissileBaseInterval;
        projDamageRate = PlayerMain.MissileBaseDamageRate;
        projSpeed = PlayerMain.MissileBaseSpeed;
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
        PlayerMissile proj = GameManager.Game.Pool.GetPlayerProj(projType, transform.position, transform.rotation).GetComponent<PlayerMissile>();
        proj.SetAddParameter(50, 1, 1);
        proj.SetProjParameter(projSpeed, projDamageRate,  0, 0);
    }

}
