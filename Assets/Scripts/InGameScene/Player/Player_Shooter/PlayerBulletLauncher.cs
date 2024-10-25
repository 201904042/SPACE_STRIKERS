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
        projFireDelay = bulletBaseInterval;
        projDamageRate = bulletBaseDamageRate;
        projSpeed = bulletBaseSpeed;
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
        PlayerBullet proj = GameManager.Instance.Pool.GetPlayerProj(projType, transform.position, transform.rotation).GetComponent<PlayerBullet>();
        proj.SetProjParameter(projSpeed, projDamageRate, 0, 0);
    }

    //[Header("�� ����")]
    //[SerializeField]
    //private float bulletSpeed;
    //[SerializeField]
    //private float delay;

    //protected override void Awake()
    //{
    //    base.Awake();
    //    projObj = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player/Player_Bullets/PlayerBullet.prefab");
    //    basicSpeed = 1;
    //    shootSpeed = basicSpeed - (myPlayerStat.attackSpeed/100);
    //    bulletSpeed = 10f;
    //}
    //protected override void Update()
    //{
    //    base.Update();
    //    if (launcherCoroutine == null && LauncherShootable) {
    //        launcherCoroutine = StartCoroutine(FireCoroutine());
    //    }

    //    if(launcherCoroutine != null && !LauncherShootable)
    //    {
    //        StopCoroutine(launcherCoroutine);
    //    }
    //}

    //private IEnumerator FireCoroutine()
    //{
    //    while (true)
    //    {
    //        Fire();
    //        yield return new WaitForSeconds(shootSpeed);
    //    }
    //}

    //protected override void Fire()
    //{
    //    base.Fire();
    //    GameObject bullet =  GameManager.Instance.Pool.GetOtherProj(OtherProjType.Player_Bullet, transform.position, transform.rotation);
    //    Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
    //    rigid.velocity = fireDirection * bulletSpeed;
    //}

}
