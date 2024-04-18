using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class Shield : MonoBehaviour
{
    SpriteRenderer shield_spriteRenderer;
    Color color;
    private Color shieldColor_lv0 = new Color(1f, 1f, 1f, 0);
    private Color shieldColor_lv1 = new Color(1f, 1f, 1f, 60f/255f);
    private Color shieldColor_lv2 = new Color(1f, 153f/255f, 153f/255f, 60f/255f);
    private Color shieldColor_lv3 = new Color(1f, 56f/255f, 56f/255f, 60f/255f);

    GameObject player;
    PlayerControl player_control_script;
    public int shield_maxNum;
    
    public int shield_curNum;

    public bool shield_is_active;
    private float shield_restore_time;
    private float shield_timer;

    private float player_stat_damage;
    private float shield_damage;
    private float shield_damageRate = 100; //스텟데미지의 4배(혹은 왠만한 일반몹 한방컷하도록 스텟 조정할것

    public bool shield_hasDamageable;
    private void Awake()
    {
        shield_spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        shield_maxNum = 3; //나중에 슈터의 레벨(2,4,6)에 따라 개수 증가 
        shield_curNum = shield_maxNum;
        shield_restore_time = 10f; //플레이어의 공격속도 비례로 고칠것
        shield_timer = 0;
        shield_is_active = true;
        player = GameObject.Find("Player");
        if (player == null)
        {
            Debug.Log("can't find player");
        }

        player_control_script = player.GetComponent<PlayerControl>();
        shield_hasDamageable = true;
        player_stat_damage = player.GetComponent<PlayerStat>().damage ;
        shield_damage = player_stat_damage * shield_damageRate;

    }

    private void Update()
    {
        
        if (shield_curNum < shield_maxNum)
        {
            shield_timer += Time.deltaTime;
            if(shield_restore_time <= shield_timer)
            {
                shield_is_active = true;
                shield_curNum += 1;
                shield_color_change();
                shield_timer = 0;
            }

        }
    }

    public void shield_color_change()
    {
        if(shield_curNum == 1)
        {

            color = shieldColor_lv1;
        }
        else if(shield_curNum == 2)
        {

            color = shieldColor_lv2;
        }
        else if (shield_curNum == 3)
        {

            color = shieldColor_lv3;
        }
        else
        {
            color = shieldColor_lv0;
        }
        shield_spriteRenderer.color = color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (shield_is_active)
        {
            if (collision.CompareTag("Enemy") || collision.CompareTag("Enemy_Projectile"))
            {
                if(collision.GetComponent<Enemy>() != null)
                {
                    collision.GetComponent<Enemy>().Enemydamaged(shield_damage,gameObject);
                }
                player_control_script.player_push(collision); //쉴드가 손상될경우 플레이어에게 넉백효과

                shield_timer = 0; // 부딫혔다면 쉴드 타이머 초기화
                shield_curNum -= 1;
                shield_color_change();

                if (shield_curNum <= 0)
                {
                    shield_is_active = false;
                }
            }
        }
    }
}
