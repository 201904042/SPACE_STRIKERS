using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;


public class ExplosionRangeOfSpecialBomber : MonoBehaviour
{
    private PlayerSpecialSkill specialScript;
    public float damage; // specialBomber���� �޾ƿ�
    private float damageTik;//������ ������ ����
    private float activeTime;
    private bool isDamaging;
    public int level; //specialBomber���� �޾ƿ�

    private bool time_has_set;
    private void Awake()
    {
        specialScript = GameManager.gameInstance.myPlayer.GetComponent<PlayerSpecialSkill>();
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
            ObjectPool.poolInstance.ReleasePool(gameObject);
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
                if (enemy.gameObject.GetComponent<EnemyObject>() != null)
                {
                    enemy.gameObject.GetComponent<EnemyObject>().EnemyDamaged(damage, gameObject);
                }
            }
            yield return new WaitForSeconds(damageTik); // ������ ���ݸ�ŭ ���
        }
    }

 
    
}
