using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_ChargeShot : PlayerShoot
{
    public float damageRate;
    public bool isPenetrate;

    private float chargeShotDamage;
    private float chargeShotSpeed;
    private bool hasHit;

    protected override void Awake()
    {
        base.Awake();
        chargeShotSpeed = 2f;
        hasHit = false;
    }

    private void Update()
    {
        if (!isFirstSet)
        {
            chargeShotDamage = playerStat.damage * damageRate;
            isFirstSet = true;
        }
        transform.position += transform.up * chargeShotSpeed * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (hasHit && !isPenetrate)
            {
                return;
            }
            if (collision.GetComponent<EnemyObject>() != null)
            {
                collision.GetComponent<EnemyObject>().EnemyDamaged(chargeShotDamage, gameObject);
                hasHit = true;
            }
            if (!isPenetrate)
            {
                Destroy(gameObject);
            }
        }
    }
}
