using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;


public class specialBomber_explosionRange : MonoBehaviour
{
    private GameObject player;
    private Player_specialSkill specialScript;
    public float damage; // specialBomber���� �޾ƿ�
    private float damageTik;//������ ������ ����
    private float active_time;
    private bool isDamaging;
    public int level; //specialBomber���� �޾ƿ�

    private bool time_has_set;
    private void Awake()
    {
        player = GameObject.Find("Player");
        specialScript = player.GetComponent<Player_specialSkill>();
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
                active_time = 3;
                specialScript.special_FireTime = active_time;
            }

        }
        else if (level == 2)
        {
            transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
            if (!time_has_set)
            {
                active_time = 4;
                specialScript.special_FireTime = active_time;
            }
        }
        else if (level == 3)
        {
            transform.localScale = new Vector3(5, 5f, 5f);
            if (!time_has_set)
            {
                active_time = 5;
                specialScript.special_FireTime = active_time;
            }
        }

        if (!time_has_set) {
            time_has_set = true;
        }

        active_time -= Time.deltaTime;
        if (active_time <= 0)
        {
            specialScript.special_Active = false;
            Destroy(gameObject);
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

    private void OnTriggerExit2D(Collider2D collision)
    {
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
                    enemy.gameObject.GetComponent<Enemy>().Enemydamaged(damage, gameObject);
                }
            }
            yield return new WaitForSeconds(damageTik); // ������ ���ݸ�ŭ ���
        }
    }

 
    
}
