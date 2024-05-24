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
        basicSpeed = 1.0f;
        shootSpeed = basicSpeed - (playerStat.attackSpeed/100) ;
    }
    protected override void Update()
    {
        base .Update();
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
        Instantiate(projObj, transform.position, transform.rotation);
    }
}
