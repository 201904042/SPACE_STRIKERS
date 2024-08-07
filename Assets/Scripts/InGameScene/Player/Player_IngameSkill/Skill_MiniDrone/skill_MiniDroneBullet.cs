using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class skill_MiniDroneBullet : PlayerShoot
{
    private bool hashit = false;

    public float damage;
    private float speed;

    protected override void Awake()
    {
        base.Awake();
        speed = 10f;
    }

    protected override void OnEnable()
    {
        Init();
    }

    protected override void Init()
    {
        base.Init();
        hashit = false;
    }


    private void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hashit)
        {
            return;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            if (collision.GetComponent<EnemyObject>() != null)
            {
                collision.GetComponent<EnemyObject>().EnemyDamaged(damage, gameObject);
            }

            hashit = true;
            ObjectPool.poolInstance.ReleasePool(gameObject);
        }
    }
}
