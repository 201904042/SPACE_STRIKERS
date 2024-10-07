using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyObject : MonoBehaviour
{
    [Header("���� ����")]
    public Enemy enemyStat; // �� ������ id�� ó���� ���� �ʱ�ȭ
    public int curEnemyId; // �ӽ�. ���� �ʱ�ȭ �� id�� �ٲ۰�츦 üũ�ϱ� ����
    public float curHp; // ������ maxHp. �̰��� 0 �̵Ǹ� �ı�
    public GameObject enemyHpBar;

    private EnemyJsonReader enemyData;
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

    public bool MakeEnemyShocked
    {
        get => isEnemySlow;
        set
        {
            isEnemySlow = value;
            isAttackReady = !value;
            UpdateSpriteColor();
        }
    }

    private readonly Color enemyBasicColor = Color.white;
    private readonly Color itemEnemyColor = Color.red;
    private readonly Color shockedColor = Color.blue;

    protected virtual void Awake()
    {
        canvas = GameObject.Find("Canvas");
        //enemyData = DataManager.dataInstance.GetComponent<EnemyJsonReader>();
    }

    protected virtual void Start()
    {
        HpBarSet();
        initStat();
    }

    protected virtual void OnEnable()
    {
        initStat();
        StartCoroutine(SetEliminate());

        isEnemyDropItem = false;
    }

    protected virtual void OnDisable()
    {

    }


    private IEnumerator SetEliminate()
    {
        yield return new WaitForSeconds(1f);
        isEliminatable = true;
    }

    protected virtual void Update()
    {
        if (enemyStat.enemyId != curEnemyId)
        {
            initStat();
        }

        if (curHp <= 0)
        {
            EnemyDeath();
            return;
        }

        if (curHp < enemyStat.enemyMaxHp && !hpBarInstance.activeSelf)
        {
            hpBarInstance.SetActive(true);
        }

        HpBarUpdate();
    }

    private void initStat()
    {
        foreach (var enemy in enemyData.EnemyList.enemy)
        {
            if (enemyStat.enemyId == enemy.enemyId)
            {
                UpdateEnemyStat(enemy);
                curHp = enemyStat.enemyMaxHp;
                break;
            }
        }
        isAttackReady = true;
        isEnemySlow = false;
        isEliminatable = false;
        isEnemyDropItem=false;
        UpdateSpriteColor();
    }

    private void UpdateEnemyStat(Enemy enemy)
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

    private void HpBarSet()
    {
        if (hpBar != null) return;

        hpBarInstance = Instantiate(enemyHpBar, canvas.transform);
        hpBarInstance.transform.SetParent(canvas.transform.GetChild(0));
        hpBar = hpBarInstance.GetComponent<RectTransform>();
        hpSlider = hpBarInstance.GetComponent<Slider>();
        hpBar.sizeDelta = new Vector2(transform.localScale.x * 200, hpBar.sizeDelta.y);
        hpBarInstance.SetActive(false);
    }

    private void HpBarUpdate()
    {
        var hpBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y - transform.localScale.y, 0));
        hpBar.position = hpBarPos;
        hpSlider.value = curHp / enemyStat.enemyMaxHp;
    }

    private void DropExp()
    {
        for (int i = 0; i < enemyStat.enemyExpAmount; i++)
        {
            Managers.Instance.Pool.GetProj(ProjType.Item_Exp, transform.position, transform.rotation);
        }
    }

    public void EnemyEliminate()
    {
        hpBar.gameObject.SetActive(false);
        Managers.Instance.Pool.ReleasePool(gameObject);
    }

    public void EnemyDeath()
    {
        if (enemyStat.enemyGrade == "Boss")
        {
            Managers.Instance.Spawn.isBossDown = true;
            Managers.Instance.Spawn.isBossSpawned = false;
        }

        DropExp();
        AddEnemyScoreToStageScore();

        if (isEnemyDropItem)
        {
            DropItem();
        }

        hpBar.gameObject.SetActive(false);
        Managers.Instance.Pool.ReleasePool(gameObject);
    }

    private void DropItem()
    {
        var projType = GameManager.Instance.myPlayer.transform.GetChild(0).GetComponent<playerShooterUpgrade>().shooterLevel < 3
            ? ProjType.Item_ShooterUP
            : GetRandomItemType();

        Managers.Instance.Pool.GetProj(projType, transform.position, transform.rotation);
    }

    private ProjType GetRandomItemType()
    {
        var randomItemList = new[]
        {
            ProjType.Item_LevelUp,
            ProjType.Item_PowUp,
            ProjType.Item_SpecialUp
        };

        return randomItemList[Random.Range(0, randomItemList.Length)];
    }

    private void AddEnemyScoreToStageScore()
    {
        GameManager.Instance.score += enemyStat.enemyScoreAmount;
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
            if (enemyStat.enemyGrade == "common" || enemyStat.enemyGrade == "elite")
            {
                EnemyEliminate();
            }
        }
    }
}
