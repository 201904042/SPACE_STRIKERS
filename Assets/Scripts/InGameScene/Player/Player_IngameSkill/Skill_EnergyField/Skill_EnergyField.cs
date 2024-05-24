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
        if (collision.CompareTag("Enemy")) // �� ��ü�� �浹�� ���
        {
            isDamaging = true; // �������� �� �غ� �Ǿ����� ǥ��
            StartCoroutine(DealDamage(collision)); // ������ �ֱ� ����
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        if (collision.CompareTag("Enemy")) // �� ��ü�� �浹�� ���� ���
        {
            isDamaging = false; // ������ �ߴ�
        }
    }

    private IEnumerator DealDamage(Collider2D enemy)
    {
        while (enemy != null && enemy.gameObject.activeSelf && isDamaging) // �������� �ִ� ����
        {
            if (enemy.gameObject.tag == "Enemy")
            {
                if (enemy.gameObject.GetComponent<EnemyObject>() != null)
                {
                    enemy.gameObject.GetComponent<EnemyObject>().EnemyDamaged(enemyDamage, gameObject);
                }
            }
            yield return new WaitForSeconds(damageTik); // ������ ���ݸ�ŭ ���
        }
    }

}
