using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class EnemyObject : MonoBehaviour
{
    public Enemy enemyStat;
    private EnemyJsonReader enemyList;
    private GameManager gameManager;
    public GameObject[] itemList;

    private int curEnemyId;
    //[HideInInspector]
    public float curHp;
    //[HideInInspector]
    public float curHpSave;

    [Header("�� ü�¹�")]
    private GameObject canvas;
    private RectTransform hpBar;
    private GameObject hpBarInstance;
    private Slider hpSlider;
    public GameObject enemyHpBar;

    public GameObject exp;

    public bool isEnemyCanAttack;
    public bool isEnemySlow;
    public bool isEnemyDropItem;
    private void Awake()
    {
        enemyList = GameObject.Find("DataManager").GetComponent<EnemyJsonReader>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        canvas = GameObject.Find("Canvas");
        ItemSet();
        if (enemyStat.enemyId != curEnemyId)
        {
            SetStat();
        }
        hpBarSet();
    }

    private void Update()
    {
        if (enemyStat.enemyId != curEnemyId)
        {
            SetStat();
        }
        if (curHp > 0)
        {
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
        else
        {
            EnemyDeath();
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
                isEnemyCanAttack = false;
                isEnemySlow = false;
            }
        }
        setEnemySprite();
    }
    private Vector3 commonScale = new Vector3(0.5f, 0.5f, 0.5f);
    private Vector3 EliteScale = new Vector3(1.4f, 0.7f, 0.5f);

    private void setEnemySprite()
    {
        //ũ������
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
        if(isEnemyDropItem)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }

    }

    private void hpBarSet()
    {
        hpBarInstance = Instantiate(enemyHpBar, canvas.transform);
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
            Instantiate(exp, transform.position, transform.rotation);
        }
    }
    public void EnemyEliminate() //�ý��� �� ����. ��óġ ���� ����
    {

        Destroy(hpBar.gameObject);
        Destroy(gameObject);
    }

    public void EnemyDeath() //�� �����. �� óġ ���� ����
    {
        EnemyExpInstatiate();
        AddEnemyScoreToStageScore();
        if(isEnemyDropItem)
        {
            DropItem();
        }
        Destroy(hpBar.gameObject);
        Destroy(gameObject);
    }
    private void ItemSet()
    {
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] {"Assets/Prefabs/Item"});
        itemList = new GameObject[guids.Length];
        for (int i = 0; i < itemList.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            itemList[i] = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
        }
    }

    private void DropItem()
    {
        if (GameObject.Find("Player").transform.GetChild(0).GetComponent<playerShooterUpgrade>().shooterLevel < 6)
        {
            Instantiate(itemList[0]);
        }
        else
        {
            int itemMaxIndex = itemList.Length - 1;
            int randomIndex = Random.Range(0, itemMaxIndex);

            Instantiate(itemList[randomIndex + 1]);
        }
    }
        
    private void AddEnemyScoreToStageScore()
    {
        gameManager.stageScore += enemyStat.enemyScoreAmount;
    }

    public void EnemyDamaged(float _damage, GameObject attackObj)
    {
        curHp -= _damage;
        Debug.Log(gameObject.name+"�� "+attackObj + " �� ���� " + _damage + " �� �������� ����");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BulletBorder"))
        {
            EnemyEliminate();
        }
    }
}
