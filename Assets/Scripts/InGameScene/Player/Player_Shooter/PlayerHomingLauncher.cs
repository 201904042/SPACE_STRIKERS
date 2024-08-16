using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHomingLauncher : LauncherStat
{
    [Header("호밍런쳐 스텟")]
    [SerializeField]
    private float delay;

    protected override void Awake()
    {
        base.Awake();
        projObj = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player/Player_Bullets/PlayerHoming.prefab");
        basicSpeed = 1.0f;
        shootSpeed = basicSpeed - (playerStat.attackSpeed/100) ;
    }
    protected override void Update()
    {
        base .Update();
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
        PoolManager.poolInstance.GetProj(ProjType.Player_Homing, transform.position, transform.rotation);
    }
}
