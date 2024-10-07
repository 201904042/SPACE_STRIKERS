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
        missileSpeed = 5f;
    }

    protected override void Update()
    {
        base.Update();
        if (launcherCoroutine == null && LauncherShootable)
        {
            launcherCoroutine = StartCoroutine(FireCoroutine());
        }

        if (launcherCoroutine != null && !LauncherShootable)
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
        GameObject missile = Managers.Instance.Pool.GetProj(ProjType.Player_Missile, transform.position, transform.rotation);
        Rigidbody2D rigid = missile.GetComponent<Rigidbody2D>();
        rigid.AddForce(fireDirection * missileSpeed, ForceMode2D.Impulse);
    }

}
