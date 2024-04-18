using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skill_Missile : PlayerShoot
{
    private bool hashit = false;
    public GameObject splash_Collider_Object;
    private float missile_damage;
    private float player_statDamage;
    public float missile_damageRate;
    public float explosion_range;
    private float missile_speed;

    protected override void Awake()
    {
        base.Awake();
        missile_damageRate = 1.5f;
        explosion_range = 1.0f;
        player_statDamage = player_stat.damage;
        missile_speed = 10f;
    }

    private void Update()
    {
        if (!is_firstSet)
        {
            missile_damage = player_statDamage * missile_damageRate;
            is_firstSet = true;
        }

        transform.position += transform.up * missile_speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hashit)
        {
            return;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            //임시로 적을 맞추면 데미지를 합산
            //적을 만들면 적의 hp를 감소하게끔 바꾸기
            hashit = true;
            if (collision.GetComponent<Enemy>() != null)
            {
                collision.GetComponent<Enemy>().Enemydamaged(missile_damage, gameObject);
            }

            Explosion_SplashDamage splashDamage = Instantiate(splash_Collider_Object, transform.position, transform.rotation).GetComponent<Explosion_SplashDamage>();
            splashDamage.explosion_range = explosion_range;
            splashDamage.missile_Damage = missile_damage;
            Destroy(gameObject);
        }
    }
}
