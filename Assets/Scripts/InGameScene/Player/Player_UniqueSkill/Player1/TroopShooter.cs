using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.WSA;

public class TroopShooter : LauncherStat
{
    //todo => 지금은 그냥 플레이어 불렛런쳐와 똑같지만 시간되면 트룹 전용 총알도 만들기

    private Troop userTroop;

    protected override void Awake()
    {
        base.Awake();
        userTroop = GetComponentInParent<Troop>();
    }

    private void OnEnable()
    {
        SetLauncher();
    }

    protected override void SetLauncher()
    {
        base.SetLauncher();
        
        projType = PlayerProjType.Player_Bullet;
        
        projFireDelay = userTroop.GetFireDelay();
        projDamageRate = userTroop.GetDamageRate();
        projSpeed = userTroop.GetProjSpeed();
        //최종 발사 주기(초) = 플레이어 공격속도 = 무기별 기본 공격속도 / (1 + (플레이어 공격속도 - 10(기준)) / 10(기준))
        attackInterval = projFireDelay / (1 + (float)(pAtkSpd - PlayerStat.basicStat) / PlayerStat.basicStat);

        isReadyToAttack = true;
        if (LaunchCoroutine == null)
        {
            LaunchCoroutine = StartCoroutine(AttackRoutine(attackInterval));
        }
    }

    protected override IEnumerator AttackRoutine(float delay)
    {
        while (true)
        {
            if (!pStat.CanAttack || !isReadyToAttack)
            {
                yield return null;
            }

            Fire();
            yield return new WaitForSeconds(delay);
        }
    }


    protected override void Fire()
    {
        base.Fire();
        PlayerBullet proj = GameManager.Instance.Pool.GetPlayerProj(projType, transform.position, transform.rotation).GetComponent<PlayerBullet>();
        proj.SetProjParameter(projSpeed, projDamageRate, 0, 0);
    }



    // 발사체의 위치와 각도를 알기 위함
    //Transform shooter_Transform;

    //public GameObject bulletobj;
    //[SerializeField]
    //private float shootSpeed;
    //[SerializeField]
    //private float bulletSpeed;
    //[SerializeField]
    //private float delay;

    //float basicSpeed = 1;
    //PlayerStat player_stat;

    //private Coroutine troopCoroutine;

    // void Awake()
    // {
    //     player_stat = GameManager.Instance.myPlayer.transform.GetComponent<PlayerStat>();
    //     shooter_Transform = GetComponentInParent<Transform>();
    // }

    // private void OnEnable()
    // {
    //     shootSpeed = basicSpeed * 5f;
    //     bulletSpeed = basicSpeed * 10f;
    //     troopCoroutine = StartCoroutine(FireCoroutine());
    // }

    // private void OnDisable()
    // {
    //     if (troopCoroutine != null) 
    //     {
    //         StopCoroutine(troopCoroutine);
    //     }
    // }

    //// 발사 기능을 코루틴으로 구현
    //IEnumerator FireCoroutine()
    // {
    //     while (true)
    //     {
    //         Fire();
    //         yield return new WaitForSeconds(1f);
    //     }
    // }

    // void Fire()
    // {
    //     Vector2 fire_direction = shooter_Transform.up;
    //     GameObject bullet = GameManager.Instance.Pool.GetPlayerProj(PlayerProjType.Player_Bullet, transform.position, transform.rotation);
    //     //GameObject bullet = Instantiate(bulletobj, transform.position, transform.rotation);
    //     Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
    //     rigid.velocity = fire_direction * bulletSpeed;
    // }
}
