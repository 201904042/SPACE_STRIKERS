using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TroopShooter : MonoBehaviour
{
    // 발사체의 위치와 각도를 알기 위함
    Transform shooter_Transform;

    public GameObject bulletobj;
    [SerializeField]
    private float shootSpeed;
    [SerializeField]
    private float bulletSpeed;
    [SerializeField]
    private float delay;

    float basicSpeed = 1;
    PlayerStat player_stat;

    private Coroutine troopCoroutine;

    void Awake()
    {
        player_stat = GameManager.Instance.myPlayer.transform.GetComponent<PlayerStat>();
        shooter_Transform = GetComponentInParent<Transform>();
    }

    private void OnEnable()
    {
        shootSpeed = basicSpeed * 5f;
        bulletSpeed = basicSpeed * 10f;
        troopCoroutine = StartCoroutine(FireCoroutine());
    }

    private void OnDisable()
    {
        if (troopCoroutine != null) 
        {
            StopCoroutine(troopCoroutine);
        }
    }

   // 발사 기능을 코루틴으로 구현
   IEnumerator FireCoroutine()
    {
        while (true)
        {
            Fire();
            yield return new WaitForSeconds(1f);
        }
    }

    void Fire()
    {
        Vector2 fire_direction = shooter_Transform.up;
        GameObject bullet = GameManager.Instance.Pool.GetProj(ProjType.Player_Bullet, transform.position, transform.rotation);
        //GameObject bullet = Instantiate(bulletobj, transform.position, transform.rotation);
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.velocity = fire_direction * bulletSpeed;
    }
}
