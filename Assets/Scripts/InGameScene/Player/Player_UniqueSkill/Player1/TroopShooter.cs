using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopShooter : MonoBehaviour
{
    //발사체의 위치와 각도를 알기 위함
    Transform shooter_Transform;

    public GameObject bulletobj;
    [SerializeField]
    private float shootSpeed;
    [SerializeField]
    private float bulletSpeed;
    [SerializeField]
    private float delay;

    float basicSpeed = 1;
    GameObject player;
    PlayerStat player_stat;


    void Awake()
    {
        player = GameObject.Find("Player");
        player_stat = player.transform.GetComponent<PlayerStat>();
        shootSpeed =  basicSpeed * 5f;
        bulletSpeed = basicSpeed * 10f;
        shooter_Transform = GetComponentInParent<Transform>();
    }
    void Update()
    {
        if (delay > shootSpeed)
        {
            Fire();
            delay = 0;
        }
        delay += 0.1f;

    }

    void Fire()
    {
        Vector2 fire_direction = shooter_Transform.up;
        GameObject bullet = ObjectPool.poolInstance.GetProj(ProjType.Player_Bullet, transform.position, transform.rotation);
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.velocity = fire_direction * bulletSpeed;
    }

}
