using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Shield : MonoBehaviour
{
    GameObject player;
    private PlayerControl playerControl;
    private PlayerStat playerStat;
    private Skill_ShieldGenerator skillGenerator;
    private float shieldDamage;
    public float curDamagerate;
    private bool isFirstSet;

    private void Awake()
    {
        player = GameObject.Find("Player");
        playerStat = player.GetComponent<PlayerStat>();
        playerControl = player.GetComponent<PlayerControl>();
        skillGenerator = gameObject.GetComponentInParent<Skill_ShieldGenerator>();
    }

    private void Update()
    {
        if (!isFirstSet || curDamagerate != skillGenerator.DamageRate)
        {
            shieldDamage = playerStat.damage * skillGenerator.DamageRate;
            curDamagerate = skillGenerator.DamageRate;
            isFirstSet = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Enemy_Projectile"))
        {
            if (collision.GetComponent<EnemyObject>() != null)
            {
                collision.GetComponent<EnemyObject>().EnemyDamaged(shieldDamage, gameObject);
            }
            playerControl.player_push(collision); //쉴드가 손상될경우 플레이어에게 넉백효과
            skillGenerator.isShieldOn = false;
            Destroy(gameObject);
        }
    }
}
