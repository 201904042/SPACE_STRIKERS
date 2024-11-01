using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.WSA;

public class TroopShooter : LauncherStat
{
    //todo => 지금은 그냥 플레이어 불렛런쳐와 똑같지만 시간되면 트룹 전용 총알도 만들기
    private Troop userTroop;

    protected override void Awake()
    {
        base.Awake();
        userTroop = GetComponentInParent<Troop>();
    }

    private void OnEnable()
    {
        SetLauncher();
    }

    protected override void SetLauncher()
    {
        base.SetLauncher();
        
        projType = PlayerProjType.Player_Bullet;
        
        //projFireDelay = TroopBaseInterval; 안씀
        projDamageRate = PlayerMain.TroopBaseDamageRate;
        projSpeed = PlayerMain.TroopBaseSpeed;
        //최종 발사 주기(초) = 플레이어 공격속도 = 무기별 기본 공격속도 / (1 + (플레이어 공격속도 - 10(기준)) / 10(기준))
        attackInterval = PlayerMain.TroopBaseInterval;

        isReadyToAttack = true;
        if (LaunchCoroutine == null)
        {
            LaunchCoroutine = StartCoroutine(AttackRoutine(attackInterval));
        }
    }

    protected override IEnumerator AttackRoutine(float delay)
    {
        while (true)
        {
            if (!pStat.CanAttack || !isReadyToAttack)
            {
                yield return null;
            }

            Fire();
            yield return new WaitForSeconds(delay);
        }
    }


    protected override void Fire()
    {
        base.Fire();
        PlayerBullet proj = GameManager.Game.Pool.GetPlayerProj(projType, transform.position, transform.rotation).GetComponent<PlayerBullet>();
        proj.SetProjParameter(projSpeed, projDamageRate, 0, 0);
    }

}
