using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Shield : MonoBehaviour
{
    private PlayerControl playerControl;
    private PlayerStat playerStat;
    private GameObject shieldGenerator;
    private float shieldDamage;
    public float curDamagerate;

    private void Awake()
    {
        playerStat = GameManager.game.myPlayer.GetComponent<PlayerStat>();
        playerControl = GameManager.game.myPlayer.GetComponent<PlayerControl>();
        shieldGenerator = GameObject.Find("skill_shieldGenerator");
        transform.SetParent(shieldGenerator.transform);
    }

    private void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        shieldDamage = playerStat.damage * shieldGenerator.GetComponent<Skill_ShieldGenerator>().damageRate;
        curDamagerate = shieldGenerator.GetComponent<Skill_ShieldGenerator>().damageRate;
    }


    private void Update()
    {
        if(transform.parent != shieldGenerator)
        {
            transform.SetParent(shieldGenerator.transform);
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

            playerControl.PlayerKnockBack(collision); //쉴드가 손상될경우 플레이어에게 넉백효과
            shieldGenerator.GetComponent<Skill_ShieldGenerator>().ShieldOn = false;
            Managers.Instance.Pool.ReleasePool(gameObject);
        }   
    }
}
