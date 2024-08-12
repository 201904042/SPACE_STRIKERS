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
    public GameObject enemyHpBar;

    protected EnemyJsonReader enemyData; //������ ������ ��� �����͸���Ʈ
    protected GameObject canvas;
    protected GameObject hpBarInstance;
    protected RectTransform hpBar;
    protected Slider hpSlider;

    public bool isAttackReady; //������ �غ� �Ϸ�
    public bool isEnemySlow; //���� ���� ����
    public bool isEnemyDropItem; //�ش� ���� �������� �������

    protected bool isAttack; //������
    protected bool isMove; //�����̴� ��

    protected virtual void Awake()
    {
        canvas = GameObject.Find("Canvas");
        enemyData = DataManager.dataInstance.gameObject.GetComponent<EnemyJsonReader>();
    }
    protected virtual void Start()
    {
        HpBarSet();
        SetStat();
    }

    protected virtual void OnEnable()
    {
        SetStat();
    }

    protected virtual void Update()
    {
        //�߰��� id�� �ٲ����� ����(����׿�)
        if (enemyStat.enemyId != curEnemyId)
        {
            SetStat();
        }

        if (curHp <= 0)
        {
            EnemyDeath();
            return;
        }
        
        if (curHp < enemyStat.enemyMaxHp)
        {
            if (!hpBarInstance.activeSelf)
            {
                hpBarInstance.SetActive(true);
            }

            HpBarUpdate();
        }
        
    }

    private void SetStat()
    {
        foreach (Enemy enemy in enemyData.EnemyList.enemy)
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
            }
        }

        isAttackReady = true;
        isEnemySlow = false;
        SetEnemySprite();
    }

    private Vector3 commonScale = new Vector3(0.5f, 0.5f, 0.5f);
    private Vector3 EliteScale = new Vector3(1.4f, 0.7f, 0.5f);

    private void SetEnemySprite()
    {
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
    private void HpBarSet()
    {
        if (hpBar != null) return;
        hpBarInstance = Instantiate(enemyHpBar, canvas.transform); //������Ʈ Ǯ �ʿ����.
        hpBarInstance.transform.SetParent(canvas.transform.GetChild(0));
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

    /// <summary>
    /// �� ����� exp ����
    /// </summary>
    private void EnemyExpInstatiate()
    {
        for (int i = 0; i < enemyStat.enemyExpAmount; i++)
        {
            PoolManager.poolInstance.GetProj(ProjType.Item_Exp, transform.position, transform.rotation);
        }
    }

    /// <summary>
    /// �ý��ۿ� ���� ���� ����(������ ���� ����)
    /// </summary>
    public void EnemyEliminate()
    {
        hpBar.gameObject.SetActive(false);
        PoolManager.poolInstance.ReleasePool(gameObject);
    }

    /// <summary>
    /// �÷��̾ ���� �� ���� (������ �����ϴ� ����)
    /// </summary>
    public void EnemyDeath() //�� �����. �� óġ ���� ����
    {
        EnemyExpInstatiate();
        AddEnemyScoreToStageScore();
        if(isEnemyDropItem)
        {
            DropItem();
        }
        hpBar.gameObject.SetActive(false);
        PoolManager.poolInstance.ReleasePool(gameObject);
    }

    private void DropItem()
    {
        if (GameManager.gameInstance.myPlayer.transform.GetChild(0).GetComponent<playerShooterUpgrade>().shooterLevel < 6)
        {
            PoolManager.poolInstance.GetProj(ProjType.Item_ShooterUP,transform.position, transform.rotation);
        }
        else
        {
            ProjType[] randomItemList = new ProjType[] { ProjType.Item_LevelUp, ProjType.Item_PowUp, ProjType.Item_SpecialUp };

            int randomIndex = Random.Range(0, randomItemList.Length);
            PoolManager.poolInstance.GetProj(randomItemList[randomIndex],transform.position,transform.rotation);
        }
    }
    
    /// <summary>
    /// �� ����� ���ھ� ����
    /// </summary>
    private void AddEnemyScoreToStageScore()
    {
        GameManager.gameInstance.score += enemyStat.enemyScoreAmount;
    }

    /// <summary>
    /// �� �������� ���� ���
    /// </summary>
    public void EnemyDamaged(float damage, GameObject attackObj)
    {
        curHp -= damage;
        curHp = Mathf.Max(curHp, 0);
        Debug.Log(gameObject.name+"�� "+attackObj + " �� ���� " + damage + " �� �������� ����");
    }

    /// <summary>
    /// ������ �浹�� �� ����
    /// </summary>
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BulletBorder"))
        {
            EnemyEliminate();
        }
    }
}
