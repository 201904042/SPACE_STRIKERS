using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skill_ChargeShot : PlayerShoot
{
    public float damageRate;
    public bool is_penetrate;

    private float chargeShot_damage;
    private float speed;
    private bool hashit;

    protected override void Awake()
    {
        base.Awake();
        speed = 2f;
        hashit = false;
    }

    private void Update()
    {
        if (!is_firstSet)
        {
            chargeShot_damage = player_stat.damage * damageRate;
            is_firstSet = true;
        }
        transform.position += transform.up * speed * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (hashit && !is_penetrate)
            {
                return;
            }
            if (collision.GetComponent<Enemy>() != null)
            {
                collision.GetComponent<Enemy>().Enemydamaged(chargeShot_damage, gameObject);
                hashit = true;
            }
            if (!is_penetrate)
            {
                Destroy(gameObject);
            }
            
        }
    }
}
