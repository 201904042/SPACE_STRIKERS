using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyObject : MonoBehaviour
{
    [Header("공통 스텟")]
    public EnemyData enemyStat; // 이 스텟의 id로 처음에 스텟 초기화
    public int curEnemyId; // 임시. 스텟 초기화 후 id를 바꾼경우를 체크하기 위함
    public float curHp; // 현재의 IG_Hp. 이것이 0 이되면 파괴
    public GameObject enemyHpBar;

    private GameObject canvas;
    private GameObject hpBarInstance;
    private RectTransform hpBar;
    private Slider hpSlider;

    public bool isAttackReady;
    private bool isEnemySlow;

    public bool isEliminatable;

    public bool isAttack;
    public bool isMove;
    public int stopCount; //스탑라인을 통과한 수

    public bool isEnemyDropItem;
    public bool MakeEnemyDropItem
    {
        get => isEnemyDropItem;
        set
        {
            isEnemyDropItem = value;
            UpdateSpriteColor();
        }
    }


    private readonly Color enemyBasicColor = Color.white;
    private readonly Color itemEnemyColor = Color.red;
    private readonly Color shockedColor = Color.blue;

    protected virtual void Awake()
    {
        canvas = GameObject.Find("Canvas");
    }

    protected virtual void Start()
    {
        HpBarSet();
    }

    protected virtual void OnEnable()
    {
        StartCoroutine(SetEliminatable());
    }

    protected virtual void OnDisable()
    {

    }

    protected virtual void Update()
    {
        if (isEliminatable)
        {
            if (curHp <= 0)
            {
                EnemyDeath();
                return;
            }

            if (curHp < enemyStat.hp && !hpBarInstance.activeSelf)
            {
                hpBarInstance.SetActive(true);
            }

            HpBarUpdate();
        }
    }

    public void SetId(int id)
    {
        curEnemyId = id;
        initStat();
        
    }

    private void initStat()
    {
        enemyStat = DataManager.enemy.GetData(curEnemyId);

        UpdateEnemyStat(enemyStat);
        curHp = enemyStat.hp;

        isAttackReady = true;
        isEnemySlow = false;
        isEliminatable = false;
        isEnemyDropItem = false;
        UpdateSpriteColor();
    }

    private IEnumerator SetEliminatable()
    {
        yield return new WaitForSeconds(1f);
        isEliminatable = true;
    }

    private void UpdateEnemyStat(EnemyData enemy)
    {
        enemyStat.type = enemy.type;
        enemyStat.hp = enemy.hp;
        enemyStat.damage = enemy.damage;
        enemyStat.moveSpeed = enemy.moveSpeed;
        enemyStat.attackSpeed = enemy.attackSpeed;
        enemyStat.expAmount = enemy.expAmount;
        enemyStat.scoreAmount = enemy.scoreAmount;
        enemyStat.isStop = enemy.isStop;
        enemyStat.isAim = enemy.isAim;
        curEnemyId = enemy.id;
    }

    private void UpdateSpriteColor()
    {
        var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (isEnemySlow)
        {
            spriteRenderer.color = shockedColor;
        }
        else
        {
            spriteRenderer.color = isEnemyDropItem ? itemEnemyColor : enemyBasicColor;
        }
    }

    //해당 에너미 오브젝트의 hp를 만듬
    private void HpBarSet()
    {
        if (hpBar != null) return;

        hpBarInstance = Instantiate(enemyHpBar, canvas.transform);
        hpBar = hpBarInstance.GetComponent<RectTransform>();
        hpSlider = hpBarInstance.GetComponent<Slider>();

        hpBarInstance.name = $"{gameObject.name}'s maxHp";
        hpBarInstance.transform.SetParent(canvas.transform.GetChild(0));
        
        hpBar.sizeDelta = new Vector2(transform.localScale.x * 200, hpBar.sizeDelta.y);
        hpBarInstance.SetActive(false);
    }

    private void HpBarUpdate()
    {
        var hpBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y - transform.localScale.y, 0));
        hpBar.position = hpBarPos;
        hpSlider.value = curHp / enemyStat.hp;
    }

    private void DropExp()
    {
        for (int i = 0; i < enemyStat.expAmount; i++)
        {
            Exp_object exp = GameManager.Game.Pool.GetOtherProj(OtherProjType.Item_Exp, transform.position, transform.rotation).GetComponent<Exp_object>();
            exp.OnExp();
        }
    }

    public void EnemyEliminate()
    {
        hpBar.gameObject.SetActive(false);
        GameManager.Game.Pool.ReleasePool(gameObject);
    }

    public void EnemyDeath()
    {
        //if (enemyStat.type == EnemyType.Boss)
        //{
        //    GameManager.Game.Spawn.isBossDown = true;
        //    GameManager.Game.Spawn.isBossSpawned = false;
        //} 이건 보스의 스크립트로 옮기기

        DropExp();
        AddEnemyScoreToStageScore();

        if (isEnemyDropItem)
        {
            DropItem();
        }

        hpBar.gameObject.SetActive(false);
        GameManager.Game.Pool.ReleasePool(gameObject);
    }

    //public Coroutine EnemySlow(int slowRate, float SlowTime)
    //{
    //    float enemySpeedIndex = 
    //    isEnemySlow = true;
    //    isAttackReady = false;

    //    UpdateSpriteColor();

    //    yield return new WaitForSeconds(SlowTime);
    //}

    private void DropItem()
    {
        var projType = PlayerMain.pStat.IG_WeaponLv < 3
            ? OtherProjType.Item_ShooterUP
            : GetRandomItemType();

        GameManager.Game.Pool.GetOtherProj(projType, transform.position, transform.rotation);
    }

    private OtherProjType GetRandomItemType()
    {
        var randomItemList = new[]
        {
            OtherProjType.Item_LevelUp,
            OtherProjType.Item_PowUp,
            OtherProjType.Item_SpecialUp
        };

        return randomItemList[Random.Range(0, randomItemList.Length)];
    }

    private void AddEnemyScoreToStageScore()
    {
        GameManager.Game.Score += enemyStat.scoreAmount;
    }

    public void EnemyDamaged(float damage, GameObject attackObj)
    {
        curHp = Mathf.Max(curHp - damage, 0);
        Debug.Log($"{gameObject.name}이 {attackObj}에 의해 {damage}의 데미지를 입음");
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyEliminate") && isEliminatable)
        {
            if (enemyStat.type == EnemyType.CommonType1 || enemyStat.type == EnemyType.CommonType2 || enemyStat.type == EnemyType.EliteType1|| enemyStat.type == EnemyType.EliteType2)
            {
                EnemyEliminate();
            }
        }
    }
}
