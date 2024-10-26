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
        projFireDelay = MissileBaseInterval;
        projDamageRate = MissileBaseDamageRate;
        projSpeed = MissileBaseSpeed;
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

    //    [Header("미사일런쳐 스텟")]
    //    [SerializeField]
    //    private float missileSpeed;
    //    [SerializeField]
    //    private float delay;


    //    protected override void Awake()
    //    {
    //        base.Awake();
    //        projObj = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player/Player_Bullets/PlayerMissile.prefab");
    //        playerMoveSpeedBase = 3f;
    //        shootSpeed = playerMoveSpeedBase - (myPlayerStat.attackSpeed/100);
    //        missileSpeed = 5f;
    //    }

    //    protected override void Update()
    //    {
    //        base.Update();
    //        if (launcherCoroutine == null && LauncherShootable)
    //        {
    //            launcherCoroutine = StartCoroutine(FireCoroutine());
    //        }

    //        if (launcherCoroutine != null && !LauncherShootable)
    //        {
    //            StopCoroutine(launcherCoroutine);
    //        }
    //    }

    //    private IEnumerator FireCoroutine()
    //    {
    //        while (true)
    //        {
    //            Fire();
    //            yield return new WaitForSeconds(shootSpeed);
    //        }
    //    }

    //    protected override void Fire()
    //    {
    //        base.Fire();
    //        GameObject missile = GameManager.Instance.Pool.GetOtherProj(OtherProjType.Player_Missile, transform.position, transform.rotation);
    //        Rigidbody2D rigid = missile.GetComponent<Rigidbody2D>();
    //        rigid.AddForce(fireDirection * missileSpeed, ForceMode2D.Impulse);
    //    }

}
