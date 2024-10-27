using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissileLauncher : LauncherStat
{
    //todo  -> 레벨별 기본 데미지 비율, 크기 , 폭발 조절
    protected override void SetLauncher()
    {
        base.SetLauncher();
        projType = PlayerProjType.Player_Missile;
        projFireDelay = PlayerMain.MissileBaseInterval;
        projDamageRate = PlayerMain.MissileBaseDamageRate;
        projSpeed = PlayerMain.MissileBaseSpeed;
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
        PlayerMissile proj = GameManager.Instance.Pool.GetPlayerProj(projType, transform.position, transform.rotation).GetComponent<PlayerMissile>();
        proj.SetProjParameter(projSpeed, projDamageRate,  0, 0);
    }



    #region 디버깅 버튼

    #endregion
}
