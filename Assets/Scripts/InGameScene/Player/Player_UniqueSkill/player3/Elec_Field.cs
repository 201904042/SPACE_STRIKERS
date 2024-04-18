using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Elec_Field : MonoBehaviour
{
    private GameObject player;
    private Player_specialSkill specialScript;

    private float damage;
    private bool isDamaging;
    private float damageTik;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        specialScript = player.GetComponent<Player_specialSkill>();
        damage = specialScript.special_Damage;
        isDamaging = false;
        damageTik = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) // �� ��ü�� �浹�� ���
        {
            isDamaging = true; // �������� �� �غ� �Ǿ����� ǥ��
            StartCoroutine(DealDamage(collision)); // ������ �ֱ� ����
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) // �� ��ü�� �浹�� ���� ���
        {
            isDamaging = false; // ������ �ߴ�
        }
    }

    private IEnumerator DealDamage(Collider2D enemy)
    {
        while (isDamaging) // �������� �ִ� ����
        {
            if(enemy != null)
            {
                if (enemy.gameObject.tag == "Enemy")
                {
                    if (enemy.gameObject.GetComponent<Enemy>() != null)
                    {
                        enemy.gameObject.GetComponent<Enemy>().Enemydamaged(damage, gameObject);
                    }

                }
                yield return new WaitForSeconds(damageTik); // ������ ���ݸ�ŭ ���
            }
            else
            {
                isDamaging = false;
                yield return null;
            }
        }
    }
}

