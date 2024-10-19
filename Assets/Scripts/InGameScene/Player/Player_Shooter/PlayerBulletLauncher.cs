using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerBulletLauncher : LauncherStat
{
    [Header("°Ç ½ºÅÝ")]
    [SerializeField]
    private float bulletSpeed;
    [SerializeField]
    private float delay;

    protected override void Awake()
    {
        base.Awake();
        projObj = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player/Player_Bullets/PlayerBullet.prefab");
        basicSpeed = 1;
        shootSpeed = basicSpeed - (playerStat.attackSpeed/100);
        bulletSpeed = 10f;
    }
    protected override void Update()
    {
        base.Update();
        if (launcherCoroutine == null && LauncherShootable) {
            launcherCoroutine = StartCoroutine(FireCoroutine());
        }
        
        if(launcherCoroutine != null && !LauncherShootable)
        {
            StopCoroutine(launcherCoroutine);
        }
    }

    private IEnumerator FireCoroutine()
    {
        while (true)
        {
            Fire();
            yield return new WaitForSeconds(shootSpeed);
        }
    }

    protected override void Fire()
    {
        base.Fire();
        GameObject bullet =  GameManager.Instance.Pool.GetProj(ProjType.Player_Bullet, transform.position, transform.rotation);
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.velocity = fireDirection * bulletSpeed;
    }

}
