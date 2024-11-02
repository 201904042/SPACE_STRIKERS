using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHomingLauncher : LauncherStat
{

    protected override void SetLauncher()
    {
        base.SetLauncher();
        projType = PlayerProjType.Player_Homing;
        projFireDelay = PlayerMain.HomingBaseInterval;
        projDamageRate = PlayerMain.HomingBaseDamageRate;
        projSpeed = PlayerMain.HomingBaseSpeed;
        //최종 발사 주기(초) = 플레이어 공격속도 = 무기별 기본 공격속도 / (1 + (플레이어 공격속도 - 10(기준)) / 10(기준))
        attackInterval = projFireDelay / (1 + (float)(pAtkSpd - PlayerStat.basicStat) / PlayerStat.basicStat);


        isReadyToAttack = true;
        if (LaunchCoroutine == null)
        {
            LaunchCoroutine = StartCoroutine(AttackRoutine(attackInterval));
        }
    }

    protected override void Fire()
    {
        base.Fire();
        
        PlayerHoming proj = GameManager.Game.Pool.GetPlayerProj(projType, transform.position, transform.rotation).GetComponent<PlayerHoming>();
        proj.SetProjParameter(projSpeed, projDamageRate, 0, 0);
    }
}
