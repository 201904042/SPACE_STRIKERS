using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHomingLauncher : LauncherStat
{
    [Header("호밍런쳐 스텟")]
    [SerializeField]
    private float delay;

    protected override void Awake()
    {
        base.Awake();
        basic_speed = 1.0f;
        shootSpeed = basic_speed - (player_stat.attack_speed/100) ;
    }
    protected override void Update()
    {
        base .Update();
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
        Instantiate(proj_obj, transform.position, transform.rotation);
    }
}
