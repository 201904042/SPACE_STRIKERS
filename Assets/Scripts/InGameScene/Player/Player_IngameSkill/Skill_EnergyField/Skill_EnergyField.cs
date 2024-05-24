using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Skill_EnergyField : PlayerShoot
{

    public float enemyDamagerate;
    public float enemyDuration;
    public bool isEnemyShootable;
    public float enemyDamage;

    private bool isDamaging;
    private float damageTik;
    private float timer;
    private float curDamageRate;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        isDamaging = false;
        damageTik = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFirstSet || curDamageRate != enemyDamagerate)
        {
            enemyDamage = playerStat.damage * enemyDamagerate;
            curDamageRate = enemyDamagerate;
            timer = enemyDuration;
            isFirstSet = true;
        }

        
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            if (isEnemyShootable)
            {
                if(transform.parent != null)
                {
                    transform.parent = null;
                }
                transform.position += transform.up * 3 * Time.deltaTime;
            }
            else
            {
                Destroy(gameObject);
            }
            
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

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
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
                    enemy.gameObject.GetComponent<EnemyObject>().EnemyDamaged(enemyDamage, gameObject);
                }
            }
            yield return new WaitForSeconds(damageTik); // 데미지 간격만큼 대기
        }
    }

}
