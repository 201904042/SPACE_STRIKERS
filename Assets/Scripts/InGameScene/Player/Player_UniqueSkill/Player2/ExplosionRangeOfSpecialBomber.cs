using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;


public class ExplosionRangeOfSpecialBomber : MonoBehaviour
{
    private GameObject player;
    private PlayerSpecialSkill specialScript;
    public float damage; // specialBomber에서 받아옴
    private float damageTik;//데미지 사이의 간격
    private float activeTime;
    private bool isDamaging;
    public int level; //specialBomber에서 받아옴

    private bool time_has_set;
    private void Awake()
    {
        player = GameObject.Find("Player");
        specialScript = player.GetComponent<PlayerSpecialSkill>();
        damageTik = 0.1f;
        isDamaging = false;
        time_has_set = false;


    }
    void Update()
    {
        if (level == 1)
        {
            transform.localScale = new Vector3(2f, 2f, 2f);
            if (!time_has_set)
            {
                activeTime = 3;
                specialScript.specialFireTime = activeTime;
            }

        }
        else if (level == 2)
        {
            transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
            if (!time_has_set)
            {
                activeTime = 4;
                specialScript.specialFireTime = activeTime;
            }
        }
        else if (level == 3)
        {
            transform.localScale = new Vector3(5, 5f, 5f);
            if (!time_has_set)
            {
                activeTime = 5;
                specialScript.specialFireTime = activeTime;
            }
        }

        if (!time_has_set) {
            time_has_set = true;
        }

        activeTime -= Time.deltaTime;
        if (activeTime <= 0)
        {
            specialScript.specialActive = false;
            Destroy(gameObject);
        }
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
        while (enemy != null && enemy.gameObject.activeSelf && isDamaging) // 데미지를 주는 동안
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
    }

 
    
}
