using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopShooter : MonoBehaviour
{
    //�߻�ü�� ��ġ�� ������ �˱� ����
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
        GameObject bullet = Instantiate(bulletobj, transform.position, transform.rotation);
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.velocity = fire_direction * bulletSpeed;
    }

}
