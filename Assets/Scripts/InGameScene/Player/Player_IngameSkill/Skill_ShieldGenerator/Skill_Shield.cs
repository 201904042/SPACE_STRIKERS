using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Shield : MonoBehaviour
{
    GameObject player;
    private PlayerControl p_control;
    private PlayerStat p_stat;
    private Skill_ShieldGenerator s_gen;
    private float shield_damage;
    public float cur_damageRate;
    private bool is_firstSet;

    private void Awake()
    {
        player = GameObject.Find("Player");
        p_stat = player.GetComponent<PlayerStat>();
        p_control = player.GetComponent<PlayerControl>();
        s_gen = gameObject.GetComponentInParent<Skill_ShieldGenerator>();
    }

    private void Update()
    {
        if (!is_firstSet || cur_damageRate != s_gen.DamageRate)
        {
            shield_damage = p_stat.damage * s_gen.DamageRate;
            cur_damageRate = s_gen.DamageRate;
            is_firstSet = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Enemy_Projectile"))
        {
            if (collision.GetComponent<Enemy>() != null)
            {
                collision.GetComponent<Enemy>().Enemydamaged(shield_damage, gameObject);
            }
            p_control.player_push(collision); //쉴드가 손상될경우 플레이어에게 넉백효과
            s_gen.is_shieldOn = false;
            Destroy(gameObject);
        }
    }
}
