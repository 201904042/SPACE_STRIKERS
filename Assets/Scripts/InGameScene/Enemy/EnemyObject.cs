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
    public GameObject enemyHpBar;

    protected EnemyJsonReader enemyData; //적들의 정보가 담긴 데이터리스트
    protected GameObject canvas;
    protected GameObject hpBarInstance;
    protected RectTransform hpBar;
    protected Slider hpSlider;

    public bool isAttackReady; //공격할 준비 완료
    private bool isEnemySlow; //현재 감속 상태

    public bool isEliminatable;

    [SerializeField]
    protected bool isAttack; //공격중
    [SerializeField]
    protected bool isMove; //움직이는 중

    private bool isEnemyDropItem; //해당 적이 아이템을 드롭할지
    public bool MakeEnemyDropItem
    {
        get => isEnemyDropItem;
        set
        {
            isEnemyDropItem = value;
            if (isEnemyDropItem)
            {
                gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }
    public bool MakeEnemyShocked
    {
        get => isEnemySlow;
        set 
        {
            isEnemySlow = value;
            isAttackReady = !value;
            if (isEnemySlow)
            {
                gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }


    protected virtual void Awake()
    {
        canvas = GameObject.Find("Canvas");
        enemyData = DataManager.dataInstance.gameObject.GetComponent<EnemyJsonReader>();
    }

    private IEnumerator SetEliminate()
    {
        yield return new WaitForSeconds(1f);
        isEliminatable = true;
    }

    protected virtual void Start()
    {
        HpBarSet();
        SetStat();
    }

    protected virtual void OnEnable()
    {
        SetStat();
        StartCoroutine(SetEliminate());
    }

    protected virtual void Update()
    {
        //중간에 id를 바꿨을때 내용(디버그용)
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
        isEliminatable = false;

        SetEnemySprite();
    }

    private Vector3 commonScale = new Vector3(0.5f, 0.5f, 0.5f);
    private Vector3 EliteScale = new Vector3(1.4f, 0.7f, 0.5f);

    private void SetEnemySprite()
    {
        //스프라이트 지정

        

    }

    /// <summary>
    /// hp바 없으면 생성하고 세팅후 비활성화
    /// </summary>
    private void HpBarSet()
    {
        if (hpBar != null) return;
        hpBarInstance = Instantiate(enemyHpBar, canvas.transform); //오브젝트 풀 필요없다.
        hpBarInstance.transform.SetParent(canvas.transform.GetChild(0));
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

    /// <summary>
    /// 적 사망시 exp 생성
    /// </summary>
    private void DropExp()
    {
        for (int i = 0; i < enemyStat.enemyExpAmount; i++)
        {
            PoolManager.poolInstance.GetProj(ProjType.Item_Exp, transform.position, transform.rotation);
        }
    }

    /// <summary>
    /// 시스템에 의한 적을 제거(보상이 없는 제거)
    /// </summary>
    public void EnemyEliminate()
    {
        hpBar.gameObject.SetActive(false);
        PoolManager.poolInstance.ReleasePool(gameObject);
    }

    /// <summary>
    /// 플레이어에 의한 적 제거 (보상을 생성하는 제거)
    /// </summary>
    public void EnemyDeath() //적 사망시. 적 처치 보상 있음
    {
        if(enemyStat.enemyGrade == "Boss")
        {
            SpawnManager.spawnInstance.isBossDown = true;
            SpawnManager.spawnInstance.isBossSpawned = false;
        }
        DropExp();
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
        if (GameManager.gameInstance.myPlayer.transform.GetChild(0).GetComponent<playerShooterUpgrade>().shooterLevel < 3)
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
    /// 적 사망시 스코어 증가
    /// </summary>
    private void AddEnemyScoreToStageScore()
    {
        GameManager.gameInstance.score += enemyStat.enemyScoreAmount;
    }

    /// <summary>
    /// 적 데미지를 받은 경우
    /// </summary>
    public void EnemyDamaged(float damage, GameObject attackObj)
    {
        curHp -= damage;
        curHp = Mathf.Max(curHp, 0);
        Debug.Log(gameObject.name+"이 "+attackObj + " 에 의해 " + damage + " 의 데미지를 입음");
    }

    /// <summary>
    /// 보더와 충돌시 적 제거
    /// </summary>
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BulletBorder")  )
        {
            if(isEliminatable == true)
            {
                if (enemyStat.enemyGrade == "common" || enemyStat.enemyGrade == "elite")
                {
                    EnemyEliminate();
                }
            }
        }
    }
}
