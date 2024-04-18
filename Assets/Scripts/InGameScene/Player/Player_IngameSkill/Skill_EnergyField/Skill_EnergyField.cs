using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyJsonReader;

public class Skill_EnergyField : PlayerShoot
{

    public float e_damageRate;
    public float e_duration;
    public bool e_is_shootable;


    public float e_damage;
    private bool isDamaging;
    private float damageTik;
    private float timer;
    private float cur_damageRate;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        player_stat = player.GetComponent<PlayerStat>();
        isDamaging = false;
        damageTik = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!is_firstSet || cur_damageRate != e_damageRate)
        {
            e_damage = player_stat.damage * e_damageRate;
            cur_damageRate = e_damageRate;
            timer = e_duration;
            is_firstSet = true;
        }

        
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            if (e_is_shootable)
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
                if (enemy.gameObject.GetComponent<Enemy>() != null)
                {
                    enemy.gameObject.GetComponent<Enemy>().Enemydamaged(e_damage, gameObject);
                }
            }
            yield return new WaitForSeconds(damageTik); // ������ ���ݸ�ŭ ���
        }
    }

}
