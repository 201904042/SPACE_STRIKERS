using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyObject : MonoBehaviour
{
    [Header("���� ����")]
    public EnemyData enemyStat; // �� ������ id�� ó���� ���� �ʱ�ȭ
    public int curEnemyId; // �ӽ�. ���� �ʱ�ȭ �� id�� �ٲ۰�츦 üũ�ϱ� ����
    public float curHp; // ������ IG_Hp. �̰��� 0 �̵Ǹ� �ı�
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
    public int stopCount; //��ž������ ����� ��

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
        enemyStat.socreAmount = enemy.socreAmount;
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

    //�ش� ���ʹ� ������Ʈ�� hp�� ����
    private void HpBarSet()
    {
        if (hpBar != null) return;

        hpBarInstance = Instantiate(enemyHpBar, canvas.transform);
        hpBar = hpBarInstance.GetComponent<RectTransform>();
        hpSlider = hpBarInstance.GetComponent<Slider>();

        hpBarInstance.name = $"{gameObject.name}'s hp";
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
            GameManager.Game.Pool.GetOtherProj(OtherProjType.Item_Exp, transform.position, transform.rotation);
        }
    }

    public void EnemyEliminate()
    {
        hpBar.gameObject.SetActive(false);
        GameManager.Game.Pool.ReleasePool(gameObject);
    }

    public void EnemyDeath()
    {
        if (enemyStat.type == EnemyType.Boss)
        {
            GameManager.Game.Spawn.isBossDown = true;
            GameManager.Game.Spawn.isBossSpawned = false;
        }

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
        GameManager.Game.score += enemyStat.socreAmount;
    }

    public void EnemyDamaged(float damage, GameObject attackObj)
    {
        curHp = Mathf.Max(curHp - damage, 0);
        Debug.Log($"{gameObject.name}�� {attackObj}�� ���� {damage}�� �������� ����");
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BulletBorder") && isEliminatable)
        {
            if (enemyStat.type == EnemyType.CommonType1 || enemyStat.type == EnemyType.CommonType2 || enemyStat.type == EnemyType.EliteType1|| enemyStat.type == EnemyType.EliteType2)
            {
                EnemyEliminate();
            }
        }
    }
}
