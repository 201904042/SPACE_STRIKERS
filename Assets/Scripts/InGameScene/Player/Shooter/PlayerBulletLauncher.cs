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
        basic_speed = 1;
        shootSpeed = basic_speed - (player_stat.attack_speed/100);
        bulletSpeed = 10f;
    }
    protected override void Update()
    {
        base.Update();
        if (Launcher_shootable)
        {
            delay += Time.deltaTime;
            if (delay > shootSpeed)
            {
                Fire();
                delay = 0;
            }
        }
    }

    void Fire()
    {
        Vector2 fire_direction = player.transform.up;
        GameObject bullet = Instantiate(proj_obj, transform.position, transform.rotation);
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.velocity = fire_direction * bulletSpeed;
    }
}
