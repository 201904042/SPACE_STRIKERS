using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyObject : MonoBehaviour
{
    private const string HpBarPath = "UI/Ingame/Enemy/E_HpBar";
    public Slider hpBar; //�ش� ������ �Ҵ�� HPBar UI��ü. �ݵ�� �����ϰ� �־����

    protected const int C_DefaultAtkDelay = 10;
    protected const int C_DefaultProjNum = 1;
    protected const int E_DefaultAtkDelay = 6;
    protected const int E_DefaultProjNum = 3;

    protected int increaseRate => SetIncreaseRate(GameManager.Game.phase);
    protected int SetIncreaseRate(int phase) //������ �������� ���Ѹ�忡�� ����
    {
        switch(phase)
        {
            case 1: return 100;
            case 2: return 150; 
            case 3: return 200; 
            case 4: return 250; 
            case 5: return 300;
            case 6: return 500;
        }

        return 100;
    }

    public int id;
    public EnemyType type;

    //�� ���ʹ� �������� ���� * ������������ ������ �ݿ��� ����
    [SerializeField] protected int maxHp;
    [SerializeField] protected int curHp;
    [SerializeField] protected int damage;
    [SerializeField] protected int moveSpeed;
    [SerializeField] protected int attackSpeed;
    [SerializeField] protected int expAmount;
    [SerializeField] protected int scoreAmount;

    //������ǵǴ� ���� ���ݿ� ���� �߻�ü ���� ���� ������ ������
    [SerializeField] protected int curProjNum; //�ش� ���� �ѹ� �����Ҷ� �߻��ϴ� �߻�ü �� -> ��� ����
    [SerializeField] protected float curAtkDelay; //���� ���� ���� ������(��) -> ��� ����

    //Ȱ��ȭ�� ���ÿ� ����Ǵ� ���� �ൿ(������Ʈ�� ����)
    [SerializeField] protected Coroutine enemyBehavior; //���� �ൿ �ڷ�ƾ�� ����

    [Header("���� ����")]
    [SerializeField] protected bool isEliminatable; //�ý����� ������ ����
    [SerializeField] protected bool isDamageable; //�ý����� ������ ����
    [SerializeField] protected bool isDropItem; //������ �������� ���
    [SerializeField] protected bool isStop; //�������� ������ ����
    [SerializeField] protected bool isAttack; //�������� ����
    [SerializeField] protected bool isShocked; //�÷��̾��� ��ų�� ���� ����, �ӵ� �� ���� ����

    protected virtual void OnDisable() => ResetObject();

  
    private void Start()
    {
        SetEnemy();
    }

    #region �ʱ�ȭ �� ����
    /// <summary>
    /// ���� ���� �Լ�. �� �Լ��� �������� ���� ���� ���� �� �ൿ ����
    /// </summary>
    /// <param name="enemyId"></param>
    /// <param name="increaseRate"></param>
    public void SetEnemy() //5�� ���� 100% 10���� 150%, 15���� 200% 20���� 250% 25���� 300% 30���� 500%
    {
        InitializeEnemyData();
        StartCoroutine(EnemyBehavior());
    }

    //���� ����, ��������Ʈ ����, �ʱ�HP�� ����
    private void InitializeEnemyData()
    {
        SetStat(); //������ ����
        SetHpBar();
    }

    
    protected virtual void SetStat() 
    {
        EnemyData data = DataManager.enemy.GetData(id);
        type = data.type;
        maxHp = data.hp * (increaseRate / 100);
        damage = data.damage * (increaseRate / 100);
        moveSpeed = data.moveSpeed * (increaseRate / 100);
        attackSpeed = data.attackSpeed* (increaseRate / 100);
        expAmount = data.expAmount * (increaseRate / 100);
        scoreAmount = data.scoreAmount * (increaseRate / 100); //�⺻ ���ݿ� ����ġ�� ����
        curHp = maxHp;

        gameObject.name = DataManager.master.GetData(id).name;

        //���� �ʱ�ȭ
        isEliminatable = false;
        isDamageable = false;
        isDropItem = false;
        isStop = false;
        isAttack = false;
        isShocked = false; 
    }

    public void SetItemDrop()
    {
        isDropItem = true;
    }


    private void SetHpBar()
    {
        if (hpBar == null )
        {
            Transform hpBarParent = GameManager.Canvas.Find("E_HpBars");
            hpBar = GameManager.InstantiateObject(GameManager.LoadFromResources<GameObject>(HpBarPath), hpBarParent).GetComponent<Slider>();
            hpBar.name = $"{gameObject.name}'s maxHp";
            RectTransform hpRect = hpBar.GetComponent<RectTransform>();
            hpRect.sizeDelta = new Vector2(transform.localScale.x * 200, hpRect.sizeDelta.y);
        }

        hpBar.value = 1;
        hpBar.gameObject.SetActive(false);
    }

    public virtual void ResetObject()
    {
        type = EnemyType.None;
        maxHp = 0;
        curHp = 0;
        damage = 0;
        moveSpeed = 0;
        attackSpeed = 0;
        expAmount = 0;
        scoreAmount = 0;
        //isStopByLine = false;
        //isAimAttack = false;
        isEliminatable = false;
        isDropItem = false;

        GetComponent<SpriteRenderer>().color = Color.white;
        if (hpBar.gameObject.activeSelf)
        {
            hpBar.gameObject.SetActive(false);
        }
        if (enemyBehavior != null)
        {
            StopCoroutine(enemyBehavior);
            enemyBehavior = null;
        }
    }

    #endregion

    #region ���� �� ����
    /// <summary>
    /// ������ �������� �ٶ� �� �Լ� ���
    /// </summary>
    /// <param name="hitObject"></param>
    /// <param name="damage"></param>
    public virtual void EnemyDamaged(GameObject hitObject, int damage)
    {
        //if (!isDamageable)
        //{
        //    return;
        //}
        //ActiveHitEffect();
        //curHp = Mathf.Max(curHp - damage, 0);
        //UpdateHpBarValue();
        //if(curHp == 0)
        //{
        //    EnemyDeath();
        //}
        //Debug.Log($"{gameObject.name}�� {hitObject}�� ���� {damage}�� �������� ����");
    }

    protected void UpdateHpBarValue()
    {
        if (!hpBar.gameObject.activeSelf)
        {
            hpBar.gameObject.SetActive(true);
        }
        var hpBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y - transform.localScale.y, 0));
        hpBar.GetComponent<RectTransform>().position = hpBarPos;
        hpBar.value = (float)curHp / (float)maxHp;
    }

    //�����Ӹ��� üũ
    protected void UpdateHpBarPos()
    {
        //hp�ٰ� ��Ȱ��ȭ �Ǿ��ִٸ� üũ ���� => UpdateHpBarValue�� ���� ���� Ȱ��ȭ�Ǿ����
        if (!hpBar.gameObject.activeSelf)
        {
            return;
        }
        hpBar.transform.position = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y - transform.localScale.y, 0));
    }
    private Coroutine hit;
    protected void ActiveHitEffect() {
        if(hit == null)
        {
            hit = StartCoroutine(HitEffect());
        }
       
    }

    private Color hitColor = new Color(1, 184 / 255f, 184 / 255f);
    private IEnumerator HitEffect()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;

        hit = null;
    }

    //�ý��ۿ� ���� ������ �־����� �ʴ� ����
    public void EliminateEnemy()
    {
        if (!isEliminatable)
        {
            return;
        }

        //������ �΋H���µ� �ý��ۿ� ���� ����
        GameManager.Game.Pool.ReleasePool(gameObject);
    }

    //exp, ����, ������ �� ������ �־����� ����
    public void EnemyDeath()
    {
        DropExp();
        AddEnemyScoreToStageScore();
        hpBar.gameObject.SetActive(false);
        if (isDropItem) DropItem();

        GameManager.Game.Pool.ReleasePool(gameObject);
    }

    private void DropExp()
    {
        for (int i = 0; i < expAmount; i++)
        {
            var exp = GameManager.Game.Pool.GetOtherProj(OtherProjType.Item_Exp, transform.position, transform.rotation).GetComponent<Exp_object>();
            exp.OnExp();
        }
    }

    //���� �߰�
    private void AddEnemyScoreToStageScore() => GameManager.Game.Score += scoreAmount;



    private void DropItem()
    {
        OtherProjType projType = DetermineProjType();
        GameManager.Game.Pool.GetOtherProj(projType, transform.position, transform.rotation);
    }

    private OtherProjType DetermineProjType()
    {
        return PlayerMain.pStat.IG_curWeaponLv < 3
            ? OtherProjType.Item_ShooterUP
            : GetRandomItemType();
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
    #endregion

    #region ���� �� �ൿ

    //���� �ൿ �ڷ�ƾ -> �� ��ü ���� �������̵�
    protected virtual IEnumerator EnemyBehavior() 
    {
        UpdateHpBarPos();
        
        if ((!isEliminatable || !isDamageable) && IsVisibleFrom() ) //ȭ�� �ȿ� ������ ������ ������
        {
            isEliminatable = true;
            isDamageable = true;
            Debug.Log($"{gameObject.name} �����غ�");
        }

        yield return null;
    }

    private bool IsVisibleFrom()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null)
            return false;

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }

    /// <summary>
    /// �⺻������ �׳� ���� �̵�
    /// </summary>
    protected void Move()
    {
        transform.position += transform.up * moveSpeed * Time.deltaTime;
    }

    protected IEnumerator MoveToPosition(Vector3 targetPosition, float speed)
    {
        // ��� ��ġ�� ������ ������ �̵�
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
    }

    public EnemyProjectile FireSingle(OtherProjType _proj, float angle = 0, bool _isAim = false)
    {
        EnemyProjectile proj = GameManager.Game.Pool.GetOtherProj(_proj, transform.position, transform.rotation).GetComponent<EnemyProjectile>();

        if (_isAim)
        {
            // �÷��̾ ���� ������ ����ϰ� angle��ŭ ȸ����Ŵ
            Vector3 dir = (PlayerMain.Instance.transform.position - transform.position).normalized;
            proj.transform.up = Quaternion.Euler(0, 0, angle) * dir;
        }
        else
        {
            // transform.up ���⿡�� angle��ŭ ȸ����Ŵ
            proj.transform.up = Quaternion.Euler(0, 0, angle) * transform.up;
        }

        return proj;
    }


    public EnemyProjectile[] FireMulti(OtherProjType _proj, int _projectileCount = 1, float _spreadAngle = 0, bool _isAim = false)
    {
        EnemyProjectile[] proj = new EnemyProjectile[_projectileCount];
        float startAngle = -_spreadAngle / 2;
        for (int i = 0; i < _projectileCount; i++)
        {
            // �� �߻�ü�� �߻�Ǵ� ���� ���
            float angle = startAngle + (i * (_spreadAngle / (_projectileCount - 1)));
            if (_projectileCount == 1)
            {
                angle = 0;
            }

            proj[i] = FireSingle(_proj, angle, _isAim);
        }

        return proj;
    }

    ////���� �ð����� �ݺ� �Ͽ� �߻���
    //public void FireRepeat(OtherProjType _proj, int repeatTime, int _speed, int _dmgRate, int _liveTime = 0, int _size = 0, int _projectileCount = 1, float _spreadAngle = 0, bool _isAim = false)
    //{
    //    for(int i =0 ; i < repeatTime; i++)
    //    {
    //        if(_projectileCount == 1)
    //        {
    //            FireSingle(_proj, _dmgRate, _liveTime, _size, _isAim);
    //        }
    //        else
    //        {
    //            FireMulti(_proj, _dmgRate, _liveTime, _size, _projectileCount, _spreadAngle, _isAim);
    //        }
    //    }
    //}

    #endregion


    public int GetCollisionDamage()
    {
        return damage;
    }

    private bool CheckEliminatable()
    {
        if (!isEliminatable)
        {
            return false;
        }
        if (type == EnemyType.Boss || type == EnemyType.MidBoss)
        {
            return false;
        }

        return true;
    }

    //�⺻ :  �������� Ʈ���ŵǸ� ����
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyEliminate"))
        {
            if (CheckEliminatable())
            {
                EliminateEnemy();
            }
        }
    }

}
