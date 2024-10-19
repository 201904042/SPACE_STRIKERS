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
        playerStat = GameManager.Instance.myPlayer.GetComponent<PlayerStat>();
        playerControl = GameManager.Instance.myPlayer.GetComponent<PlayerControl>();
        shieldGenerator = GameObject.Find("skill_shieldGenerator");
        transform.SetParent(shieldGenerator.transform);
    }

    private void OnEnable()
    {
        Init();
    }

    private void Init()
    {

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
            GameManager.Instance.Pool.ReleasePool(gameObject);
        }   
    }
}
