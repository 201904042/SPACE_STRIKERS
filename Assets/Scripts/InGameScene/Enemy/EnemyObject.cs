using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;



public class EnemyObject : MonoBehaviour
{
    public Enemy enemyStat; //이 스텟의 id로 처음에 스텟 초기화
    private EnemyJsonReader enemyList; //적들의 정보가 담기 데이터리스트
    public GameObject[] itemList; //적이 스폰할 아이템

    public int curEnemyId; //임시. 스텟 초기화 후 id를 바꾼경우를 체크하기 위함
    public float curHp; //현재의 hp. 이것이 0 이되면 파괴
    public float curHpSave; //체력바 업데이트를 위한 기준.

    [Header("적 체력바")]
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
        enemyList =DataManager.dataInstance.GetComponent<EnemyJsonReader>();
        canvas = GameObject.Find("Canvas");
        hpBarSet();
    }

    private void OnEnable()
    {
        //활성화 될때 id에 따라 스텟지정
        ItemSet();
        SetStat();
    }

    private void Update()
    {
        //중간에 id를 바꿨을때 내용
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
        //크기지정
        if(enemyStat.enemyGrade == "common")
        {
            //Common크기
            gameObject.transform.localScale = commonScale;
        }
        else if(enemyStat.enemyGrade == "elite")
        {
            gameObject.transform.localScale = EliteScale;
        }

        //스프라이트 지정

        //아이템 드롭 여부에 따른 색 지정
        if(isEnemyDropItem)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }

    }

    private void hpBarSet()
    {
        if (hpBar != null) return;
        hpBarInstance = Instantiate(enemyHpBar, canvas.transform);
        hpBar = hpBarInstance.GetComponent<RectTransform>();
        hpSlider = hpBarInstance.GetComponent<Slider>();
        hpBar.sizeDelta = new Vector2(transform.localScale.x * 200, hpBar.sizeDelta.y);
        hpBarInstance.SetActive(false);
    }

    private void HpBarUpdate() //hp바의 위치를 업데이트
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
    public void EnemyEliminate() //시스템 적 제거. 적처치 보상 없음
    {
        hpBar.gameObject.SetActive(false);
        ObjectPool.poolInstance.ReleasePool(gameObject);
    }

    public void EnemyDeath() //적 사망시. 적 처치 보상 있음
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
        GameManager.gameInstance.score += enemyStat.enemyScoreAmount;
    }

    public void EnemyDamaged(float _damage, GameObject attackObj)
    {
        curHp -= _damage;
        curHp = Mathf.Max(curHp, 0);
        Debug.Log(gameObject.name+"이 "+attackObj + " 에 의해 " + _damage + " 의 데미지를 입음");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BulletBorder"))
        {
            EnemyEliminate();
        }
    }
}
