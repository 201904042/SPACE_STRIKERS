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
        if (LauncherShootable)
        {
            delay += Time.deltaTime;
            if (delay > shootSpeed)
            {
                Fire();
                delay = 0;
            }
        }
    }

    private void Fire()
    {
        Vector2 fireDirection = transform.up;
        GameObject bullet = Instantiate(projObj, transform.position, transform.rotation);
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.velocity = fireDirection * bulletSpeed;
    }
}
