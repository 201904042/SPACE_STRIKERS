using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissileLauncher : LauncherStat
{
    [Header("�̻��Ϸ��� ����")]
    [SerializeField]
    private float missileSpeed;
    [SerializeField]
    private float delay;


    protected override void Awake()
    {
        base.Awake();
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

    private void Fire()
    {
        GameObject missile = Instantiate(projObj, transform.position, transform.rotation);
        Rigidbody2D rigid = missile.GetComponent<Rigidbody2D>();
        rigid.AddForce(Vector2.up * missileSpeed, ForceMode2D.Impulse);
    }

}
