using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Elec_Field : MonoBehaviour
{
    private PlayerSpecialSkill specialScript;

    private float damage;
    private bool isDamaging;
    private float damageTik;
    // Start is called before the first frame update
    private void Awake()
    {
        specialScript = GameManager.gameInstance.myPlayer.GetComponent<PlayerSpecialSkill>();
        damage = specialScript.specialDamage;
        isDamaging = false;
        damageTik = 0.1f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) // 적 객체와 충돌한 경우
        {
            isDamaging = true; // 데미지를 줄 준비가 되었음을 표시
            StartCoroutine(DealDamage(collision)); // 데미지 주기 시작
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) // 적 객체와 충돌이 끝난 경우
        {
            isDamaging = false; // 데미지 중단
        }
    }

    private IEnumerator DealDamage(Collider2D enemy)
    {
        while (isDamaging) // 데미지를 주는 동안
        {
            if(enemy != null)
            {
                if (enemy.gameObject.tag == "Enemy")
                {
                    if (enemy.gameObject.GetComponent<EnemyObject>() != null)
                    {
                        enemy.gameObject.GetComponent<EnemyObject>().EnemyDamaged(damage, gameObject);
                    }

                }
                yield return new WaitForSeconds(damageTik); // 데미지 간격만큼 대기
            }
            else
            {
                isDamaging = false;
                yield return null;
            }
        }
    }
}

