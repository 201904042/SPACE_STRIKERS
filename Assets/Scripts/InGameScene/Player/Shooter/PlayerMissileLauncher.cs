using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissileLauncher : LauncherStat
{
    [Header("미사일런쳐 스텟")]
    [SerializeField]
    private float missileSpeed;
    [SerializeField]
    private float delay;


    protected override void Awake()
    {
        base.Awake();
        basic_speed = 3f;
        shootSpeed = basic_speed - (player_stat.attack_speed/100);
        missileSpeed = 5f ;
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
        GameObject missile = Instantiate(proj_obj, transform.position, transform.rotation);
        Rigidbody2D rigid = missile.GetComponent<Rigidbody2D>();
        rigid.AddForce(Vector2.up * missileSpeed, ForceMode2D.Impulse);
    }

}
