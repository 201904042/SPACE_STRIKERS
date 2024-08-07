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
        projObj = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player/Player_Bullets/PlayerMissile.prefab");
        basicSpeed = 3f;
        shootSpeed = basicSpeed - (playerStat.attackSpeed/100);
        missileSpeed = 5f ;
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

    protected override void Fire()
    {
        base.Fire();
        GameObject missile = ObjectPool.poolInstance.GetProj(ProjType.Player_Missile, transform.position, transform.rotation);
        Rigidbody2D rigid = missile.GetComponent<Rigidbody2D>();
        rigid.AddForce(fireDirection * missileSpeed, ForceMode2D.Impulse);
    }

}
