using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissile : PlayerShoot
{
    private bool hashit = false;
    public GameObject splash_Collider_Object;

    [Header("�⺻�̻��� ����")]
    [SerializeField]
    private float missile_damage;
    [SerializeField]
    private float player_statDamage;
    [SerializeField]
    private float missile_damageRate;
    [SerializeField]
    private float explosion_range;

    protected override void Awake()
    {
        base.Awake();
        missile_damageRate = 1.5f;
        explosion_range = 1.0f;
        player_statDamage = player_stat.damage;

        missile_damage = player_statDamage * missile_damageRate;
    }
  

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(hashit)
        {
            return;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            //�ӽ÷� ���� ���߸� �������� �ջ�
            //���� ����� ���� hp�� �����ϰԲ� �ٲٱ�
            hashit = true;
            if (collision.GetComponent<Enemy>() != null)
            {
                collision.GetComponent<Enemy>().Enemydamaged(missile_damage, gameObject);
            }

            Explosion_SplashDamage splashDamage = Instantiate(splash_Collider_Object,transform.position,transform.rotation).GetComponent< Explosion_SplashDamage>();
            splashDamage.explosion_range = explosion_range;
            splashDamage.missile_Damage = missile_damage;
            Destroy(gameObject);
        }
    }
}
