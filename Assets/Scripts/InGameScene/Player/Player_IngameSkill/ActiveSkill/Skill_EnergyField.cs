using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;


public class Skill_EnergyField : PlayerProjectile
{
    public float damageRate;
    public float activeTime;
    public bool isShootable;
    public float enemyDamage;
    public float skillRange;

    private float shootSpeed;
    [SerializeField]
    private List<GameObject> hittedEnemy;
    private bool isActive;
    private bool isDealing;
    private bool isShoot;

    protected override void Awake()
    {
        base.Awake();
        shootSpeed = 3;
    }

    protected override void OnEnable()
    {
        Init();

        StartCoroutine(ActiveTimer(activeTime));
    }

    protected override void OnDisable()
    {
        StopCoroutine(ActiveTimer(activeTime));
        StopCoroutine(DealDamage());
        base.OnDisable();
    }

    protected override void Init()
    {
        base.Init();
        hittedEnemy = new List<GameObject>();

        enemyDamage = playerStat.damage * damageRate;
        isActive = false;
        isDealing = false;
        isShoot = false;
        transform.localScale = new Vector3(skillRange, skillRange, 0);
    }

    private void Update()
    {

        if(isActive&& !isDealing&&  hittedEnemy.Count > 0)
        {
            StartCoroutine(DealDamage());
        }

        if (isShoot)
        {
            transform.position += transform.up * shootSpeed * Time.deltaTime;
        }
    }

    private IEnumerator ActiveTimer(float activeTime)
    {
        isActive = true;
        yield return new WaitForSeconds(activeTime);

        if (isShootable)
        {
            if (transform.parent != null)
            {
                transform.parent = null;
            }

            isShoot = true;
        }
        else
        {
            isActive = false;
            GameManager.Instance.Pool.ReleasePool(gameObject);
        }
    }

    private IEnumerator DealDamage()
    {
        isDealing = true;
        if (hittedEnemy.Count > 0){
            foreach (GameObject enemy in hittedEnemy)
            {
                if (enemy.gameObject.GetComponent<EnemyObject>() != null)
                {
                    enemy.gameObject.GetComponent<EnemyObject>().EnemyDamaged(enemyDamage, gameObject);
                }
            }
        }
        yield return new WaitForSeconds(0.1f); // ������ ���ݸ�ŭ ���
        isDealing = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) // �� ��ü�� �浹�� ���
        {
            if (hittedEnemy.Contains(collision.gameObject) == false)
            {
                hittedEnemy.Add(collision.gameObject);
            }
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        if (collision.CompareTag("Enemy")) // �� ��ü�� �浹�� ���
        {
            hittedEnemy.Remove(collision.gameObject);
        }
    }

    

}
