using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EnemyObject : MonoBehaviour
{
    [Header("���� ����")]
    public Enemy enemyStat; //�� ������ id�� ó���� ���� �ʱ�ȭ
    public int curEnemyId; //�ӽ�. ���� �ʱ�ȭ �� id�� �ٲ۰�츦 üũ�ϱ� ����
    public float curHp; //������ maxHp. �̰��� 0 �̵Ǹ� �ı�
    public float curHpSave; //ü�¹� ������Ʈ�� ���� ����.
    public GameObject enemyHpBar;

    public bool isAttackReady;
    public bool isEnemySlow;
    public bool isEnemyDropItem;


    protected EnemyJsonReader enemyList; //������ ������ ��� �����͸���Ʈ
    protected GameObject canvas;
    protected GameObject hpBarInstance;
    protected RectTransform hpBar;
    protected Slider hpSlider;

    protected virtual void Awake()
    {
        enemyList = DataManager.dataInstance.GetComponent<EnemyJsonReader>();
        canvas = GameObject.Find("Canvas");
        hpBarSet();
        SetStat();
    }

    protected virtual void OnEnable()
    {
        //Ȱ��ȭ �ɶ� id�� ���� ��������
        SetStat();
    }

    protected virtual void Update()
    {
        //�߰��� id�� �ٲ����� ����
        if (enemyStat.enemyId != curEnemyId)
        {
            SetStat();
        }

        if (curHp <= 0)
        {
            EnemyDeath();
            return;
        }

        if (curHp != curHpSave)
        {
            if (!hpBarInstance.activeSelf)
            {
                hpBarInstance.SetActive(true);
            }
            curHpSave = curHp;
        }
        if (curHp < enemyStat.enemyMaxHp)
        {
            HpBarUpdate();
        }
        
    }

    private void SetStat()
    {
        foreach (Enemy enemy in enemyList.EnemyList.enemy)
        {
            if (enemyStat.enemyId == enemy.enemyId)
            {
                enemyStat.enemyGrade = enemy.enemyGrade;
                enemyStat.enemyName = enemy.enemyName;
                enemyStat.enemyMaxHp = enemy.enemyMaxHp;
                enemyStat.enemyDamage = enemy.enemyDamage;
                enemyStat.enemyMoveSpeed = enemy.enemyMoveSpeed;
                enemyStat.enemyAttackSpeed = enemy.enemyAttackSpeed;
                enemyStat.enemyExpAmount = enemy.enemyExpAmount;
                enemyStat.enemyScoreAmount = enemy.enemyScoreAmount;
                enemyStat.enemyMoveAttack = enemy.enemyMoveAttack;
                enemyStat.isEnemyAiming = enemy.isEnemyAiming;
                curEnemyId = enemy.enemyId;
                curHp = enemyStat.enemyMaxHp;
                curHpSave = curHp;
            }
        }
        isAttackReady = true;
        isEnemySlow = false;
        setEnemySprite();
    }

    private Vector3 commonScale = new Vector3(0.5f, 0.5f, 0.5f);
    private Vector3 EliteScale = new Vector3(1.4f, 0.7f, 0.5f);

    private void setEnemySprite()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        //�ӽ� ũ������
        if(enemyStat.enemyGrade == "common")
        {
            //Commonũ��
            gameObject.transform.localScale = commonScale;
        }
        else if(enemyStat.enemyGrade == "elite")
        {
            gameObject.transform.localScale = EliteScale;
        }
        

        //��������Ʈ ����

        //������ ��� ���ο� ���� �� ����
        if (isEnemyDropItem)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }

    }

    /// <summary>
    /// hp�� ������ �����ϰ� ������ ��Ȱ��ȭ
    /// </summary>
    private void hpBarSet()
    {
        if (hpBar != null) return;
        hpBarInstance = Instantiate(enemyHpBar, canvas.transform); //������Ʈ Ǯ �ʿ����.
        hpBar = hpBarInstance.GetComponent<RectTransform>();
        hpSlider = hpBarInstance.GetComponent<Slider>();
        hpBar.sizeDelta = new Vector2(transform.localScale.x * 200, hpBar.sizeDelta.y);
        hpBarInstance.SetActive(false);
    }

    private void HpBarUpdate() //hp���� ��ġ�� ������Ʈ
    {
        Vector3 hpBar_pos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y - (transform.localScale.y), 0));
        hpBar.position = hpBar_pos;
        hpSlider.value = curHp / enemyStat.enemyMaxHp;
    }

    private void EnemyExpInstatiate()
    {
        for (int i = 0; i < enemyStat.enemyExpAmount; i++)
        {
            ObjectPool.poolInstance.GetProj(ProjType.Item_Exp, transform.position, transform.rotation);
        }
    }
    public void EnemyEliminate() //�ý��� �� ����. ��óġ ���� ����
    {
        hpBar.gameObject.SetActive(false);
        ObjectPool.poolInstance.ReleasePool(gameObject);
    }

    public void EnemyDeath() //�� �����. �� óġ ���� ����
    {
        EnemyExpInstatiate();
        AddEnemyScoreToStageScore();
        if(isEnemyDropItem)
        {
            DropItem();
        }
        hpBar.gameObject.SetActive(false);
        ObjectPool.poolInstance.ReleasePool(gameObject);
    }

    private void DropItem()
    {
        if (GameObject.Find("Player").transform.GetChild(0).GetComponent<playerShooterUpgrade>().shooterLevel < 6)
        {
            ObjectPool.poolInstance.GetProj(ProjType.Item_ShooterUP,transform.position, transform.rotation);
        }
        else
        {
            ProjType[] randomItemList = new ProjType[] { ProjType.Item_LevelUp, ProjType.Item_PowUp, ProjType.Item_SpecialUp };

            int randomIndex = Random.Range(0, randomItemList.Length);
            ObjectPool.poolInstance.GetProj(randomItemList[randomIndex],transform.position,transform.rotation);
        }
    }
        
    private void AddEnemyScoreToStageScore()
    {
        GameManager.gameInstance.score += enemyStat.enemyScoreAmount;
    }

    public void EnemyDamaged(float _damage, GameObject attackObj)
    {
        curHp -= _damage;
        curHp = Mathf.Max(curHp, 0);
        Debug.Log(gameObject.name+"�� "+attackObj + " �� ���� " + _damage + " �� �������� ����");
    }


    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BulletBorder"))
        {
            EnemyEliminate();
        }
    }
}
