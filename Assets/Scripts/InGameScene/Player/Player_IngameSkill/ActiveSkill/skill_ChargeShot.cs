using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_ChargeShot : PlayerProjectile
{
    public float damageRate;
    public bool isPenetrate;

    private float chargeShotDamage;
    private float chargeShotSpeed;

    private List<GameObject> hittedList;

    protected override void Awake()
    {
        base.Awake();
        chargeShotSpeed = 2f;
        hasHit = false;
    }

    protected override void OnEnable()
    {
        Init();
    }

    protected override void Init()
    {
        base.Init();
        hittedList = new List<GameObject>();
        chargeShotDamage = playerStat.damage * damageRate;
    }


    private void Update()
    {
        transform.position += transform.up * chargeShotSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (collision.GetComponent<EnemyObject>() != null)
            {
                if (hittedList.Contains(collision.gameObject) == false)
                {
                    collision.GetComponent<EnemyObject>().EnemyDamaged(chargeShotDamage, gameObject);
                    hittedList.Add(collision.gameObject);
                }
            }
            if (!isPenetrate)
            {
                GameManager.Instance.Pool.ReleasePool(gameObject);
            }
        }
    }
}