using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    private PlayerStat pStat => PlayerMain.pStat;
    private PlayerControl pControl => PlayerMain.pControl;

    //쉴드의 색
    SpriteRenderer shieldSpriteRenderer;
    private Color currentColor;
    private Color shieldColor_lv0 = new Color(1f, 1f, 1f, 0);
    private Color shieldColor_lv1 = new Color(1f, 1f, 1f, 60f/255f);
    private Color shieldColor_lv2 = new Color(1f, 153f/255f, 153f/255f, 60f/255f);
    private Color shieldColor_lv3 = new Color(1f, 56f/255f, 56f/255f, 60f/255f);

    

    private Coroutine shieldRestoreCoroutine;

    private int maxShieldCount;
    private int curShieldCount;

    private float restoreDelay;
    private float crashDamage;


    private void Awake()
    {
        shieldSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        ResetShield();
    }

    private void ResetShield()
    {
        maxShieldCount = pStat.IG_WeaponLv;
        curShieldCount = maxShieldCount;
        ShieldColorChange();

        restoreDelay = PlayerMain.ShieldBaseInterval;
        crashDamage= (PlayerMain.ShieldDamageRate/100) * pStat.IG_Dmg;
    }

    private IEnumerator RestoreShield(float delay)
    {
        while (curShieldCount <= maxShieldCount)
        {
            yield return new WaitForSeconds(delay);
            curShieldCount++;
            ShieldColorChange();

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (curShieldCount == 0)
        {
            //쉴드 카운트가 0이면 더이상 작동 안함
            return;
        }

        if (pStat.InvincibleState)
        {
            //무적 상태라면 넉백만 주고 쉴드 차감 안함
            pStat.PlayerKnockBack(collision); //쉴드가 손상될경우 플레이어에게 넉백효과
            return;
        }


        if (collision.CompareTag("Enemy") || collision.CompareTag("Enemy_Projectile"))
        {
            if (collision.CompareTag("Enemy"))
            {
                collision.GetComponent<EnemyObject>().EnemyDamaged(gameObject, (int)crashDamage);
            }

            if (collision.CompareTag("Enemy_Projectile"))
            {
                GameManager.Game.Pool.ReleasePool(gameObject);
            }

            curShieldCount -= 1;
            ShieldColorChange();

            pStat.PlayerKnockBack(collision); //쉴드가 손상될경우 플레이어에게 넉백효과

            if(shieldRestoreCoroutine == null) //쉴드 손상시 쉴드 복구 코루틴 시작 => 일정시간뒤 현재 쉴드수를 증가. 최대수까지 반복
            {
                shieldRestoreCoroutine = StartCoroutine(RestoreShield(restoreDelay));
            }
        }
    }



    public void ShieldColorChange()
    {
        if (curShieldCount == 1)
        {

            currentColor = shieldColor_lv1;
        }
        else if (curShieldCount == 2)
        {

            currentColor = shieldColor_lv2;
        }
        else if (curShieldCount == 3)
        {

            currentColor = shieldColor_lv3;
        }
        else
        {
            currentColor = shieldColor_lv0;
        }
        shieldSpriteRenderer.color = currentColor;
    }

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
