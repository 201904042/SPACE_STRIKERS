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
    private bool isFirstSet;

    private void Awake()
    {
        playerStat = GameManager.gameInstance.myPlayer.GetComponent<PlayerStat>();
        playerControl = GameManager.gameInstance.myPlayer.GetComponent<PlayerControl>();
        shieldGenerator = GameObject.Find("skill_shieldGenerator");
        transform.SetParent(shieldGenerator.transform);
    }

    private void OnEnable()
    {
        Init();

    }

    private void Init()
    {
        isFirstSet = false;
    }


    private void Update()
    {
        
        if (!isFirstSet || curDamagerate != shieldGenerator.GetComponent<Skill_ShieldGenerator>().damageRate)
        {
            shieldDamage = playerStat.damage * shieldGenerator.GetComponent<Skill_ShieldGenerator>().damageRate;
            curDamagerate = shieldGenerator.GetComponent<Skill_ShieldGenerator>().damageRate;
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

            playerControl.PlayerKnockBack(collision); //쉴드가 손상될경우 플레이어에게 넉백효과
            shieldGenerator.GetComponent<Skill_ShieldGenerator>().isShieldOn = false;
            PoolManager.poolInstance.ReleasePool(gameObject);
        }   
    }
}
