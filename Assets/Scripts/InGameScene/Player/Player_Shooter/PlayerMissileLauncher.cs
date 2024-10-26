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
        projFireDelay = MissileBaseInterval;
        projDamageRate = MissileBaseDamageRate;
        projSpeed = MissileBaseSpeed;
        //���� �߻� �ֱ�(��) = �÷��̾� ���ݼӵ� = ���⺰ �⺻ ���ݼӵ� / (1 + (�÷��̾� ���ݼӵ� - 10(����)) / 10(����))
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

    //    [Header("�̻��Ϸ��� ����")]
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
