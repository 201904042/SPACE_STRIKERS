using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    private PlayerStat pStat;
    private PlayerControl pControl;

    //쉴드의 색
    SpriteRenderer shieldSpriteRenderer;
    private Color currentColor;
    private Color shieldColor_lv0 = new Color(1f, 1f, 1f, 0);
    private Color shieldColor_lv1 = new Color(1f, 1f, 1f, 60f/255f);
    private Color shieldColor_lv2 = new Color(1f, 153f/255f, 153f/255f, 60f/255f);
    private Color shieldColor_lv3 = new Color(1f, 56f/255f, 56f/255f, 60f/255f);

    

    private Coroutine shieldBehavior;

    private int maxShieldCount;
    private int curShieldCount;

    private void Awake()
    {
        shieldSpriteRenderer = GetComponent<SpriteRenderer>();

        
    }

    private void Start()
    {
        pStat = PlayerMain.pStat;
        pControl = PlayerMain.pControl;
    }

    protected void ResetShield()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (pStat.InvincibleState)
        {
            return;
        }
        if (collision.CompareTag("Enemy") || collision.CompareTag("Enemy_Projectile"))
        {
            
            pStat.PlayerKnockBack(collision); //쉴드가 손상될경우 플레이어에게 넉백효과
            GameManager.Instance.Pool.ReleasePool(gameObject);

        }
    }

    //public void ShieldColorChange()
    //{
    //    if(shieldCurNum == 1)
    //    {

    //        currentColor = shieldColor_lv1;
    //    }
    //    else if(shieldCurNum == 2)
    //    {

    //        currentColor = shieldColor_lv2;
    //    }
    //    else if (shieldCurNum == 3)
    //    {

    //        currentColor = shieldColor_lv3;
    //    }
    //    else
    //    {
    //        currentColor = shieldColor_lv0;
    //    }
    //    shieldSpriteRenderer.color = currentColor;
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (shieldIsActive)
    //    {
    //        if (collision.CompareTag("Enemy") || collision.CompareTag("Enemy_Projectile"))
    //        {
    //            if(collision.GetComponent<EnemyObject>() != null)
    //            {
    //                collision.GetComponent<EnemyObject>().EnemyDamaged(shieldDamage,gameObject);
    //            }
    //            playerControlScript.PlayerKnockBack(collision); //쉴드가 손상될경우 플레이어에게 넉백효과

    //            shieldTimer = 0; // 부딫혔다면 쉴드 타이머 초기화
    //            shieldCurNum -= 1;
    //            ShieldColorChange();

    //            if (shieldCurNum <= 0)
    //            {
    //                shieldIsActive = false;
    //            }
    //        }
    //    }
    //}
}
