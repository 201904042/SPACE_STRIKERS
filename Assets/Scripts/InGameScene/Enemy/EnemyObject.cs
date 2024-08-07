using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EnemyObject : MonoBehaviour
{
    [Header("공통 스텟")]
    public Enemy enemyStat; //이 스텟의 id로 처음에 스텟 초기화
    public int curEnemyId; //임시. 스텟 초기화 후 id를 바꾼경우를 체크하기 위함
    public float curHp; //현재의 maxHp. 이것이 0 이되면 파괴
    public float curHpSave; //체력바 업데이트를 위한 기준.
    public GameObject enemyHpBar;

    public bool isAttackReady;
    public bool isEnemySlow;
    public bool isEnemyDropItem;


    protected EnemyJsonReader enemyList; //적들의 정보가 담긴 데이터리스트
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
        //활성화 될때 id에 따라 스텟지정
        SetStat();
    }

    protected virtual void Update()
    {
        //중간에 id를 바꿨을때 내용
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
        //임시 크기지정
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
        if (isEnemyDropItem)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }

    }

    /// <summary>
    /// hp바 없으면 생성하고 세팅후 비활성화
    /// </summary>
    private void hpBarSet()
    {
        if (hpBar != null) return;
        hpBarInstance = Instantiate(enemyHpBar, canvas.transform); //오브젝트 풀 필요없다.
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
            ObjectPool.poolInstance.GetProj(ProjType.Item_Exp, transform.position, transform.rotation);
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
        Debug.Log(gameObject.name+"이 "+attackObj + " 에 의해 " + _damage + " 의 데미지를 입음");
    }


    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BulletBorder"))
        {
            EnemyEliminate();
        }
    }
}
