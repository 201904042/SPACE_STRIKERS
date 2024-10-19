using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class Shield : MonoBehaviour
{
    SpriteRenderer shieldSpriteRenderer;
    Color color;
    private Color shieldColor_lv0 = new Color(1f, 1f, 1f, 0);
    private Color shieldColor_lv1 = new Color(1f, 1f, 1f, 60f/255f);
    private Color shieldColor_lv2 = new Color(1f, 153f/255f, 153f/255f, 60f/255f);
    private Color shieldColor_lv3 = new Color(1f, 56f/255f, 56f/255f, 60f/255f);

    PlayerControl playerControlScript;
    public int shieldMaxNum;
    
    public int shieldCurNum;

    public bool shieldIsActive;
    private float shieldRestoreTime;
    private float shieldTimer;

    private float playerStatDamage;
    private float shieldDamage;
    private float shieldDamageRate = 100; //스텟데미지의 4배(혹은 왠만한 일반몹 한방컷하도록 스텟 조정할것

    public bool shieldHasDamageable;
    private void Awake()
    {
        shieldSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        shieldMaxNum = 3; //나중에 슈터의 레벨(2,4,6)에 따라 개수 증가 
        shieldCurNum = shieldMaxNum;
        shieldRestoreTime = 10f; //플레이어의 공격속도 비례로 고칠것
        shieldTimer = 0;
        shieldIsActive = true;
        

        playerControlScript = GameManager.game.myPlayer.GetComponent<PlayerControl>();
        shieldHasDamageable = true;
        playerStatDamage = GameManager.game.myPlayer.GetComponent<PlayerStat>().damage ;
        shieldDamage = playerStatDamage * shieldDamageRate;

    }

    private void Update()
    {
        
        if (shieldCurNum < shieldMaxNum)
        {
            shieldTimer += Time.deltaTime;
            if(shieldRestoreTime <= shieldTimer)
            {
                shieldIsActive = true;
                shieldCurNum += 1;
                ShieldColorChange();
                shieldTimer = 0;
            }

        }
    }

    public void ShieldColorChange()
    {
        if(shieldCurNum == 1)
        {

            color = shieldColor_lv1;
        }
        else if(shieldCurNum == 2)
        {

            color = shieldColor_lv2;
        }
        else if (shieldCurNum == 3)
        {

            color = shieldColor_lv3;
        }
        else
        {
            color = shieldColor_lv0;
        }
        shieldSpriteRenderer.color = color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (shieldIsActive)
        {
            if (collision.CompareTag("Enemy") || collision.CompareTag("Enemy_Projectile"))
            {
                if(collision.GetComponent<EnemyObject>() != null)
                {
                    collision.GetComponent<EnemyObject>().EnemyDamaged(shieldDamage,gameObject);
                }
                playerControlScript.PlayerKnockBack(collision); //쉴드가 손상될경우 플레이어에게 넉백효과

                shieldTimer = 0; // 부딫혔다면 쉴드 타이머 초기화
                shieldCurNum -= 1;
                ShieldColorChange();

                if (shieldCurNum <= 0)
                {
                    shieldIsActive = false;
                }
            }
        }
    }
}
