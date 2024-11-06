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
    public Slider hpBar; //해당 적에게 할당된 HPBar UI객체. 반드시 소유하고 있어야함

    protected const int C_DefaultAtkDelay = 10;
    protected const int C_DefaultProjNum = 1;
    protected const int E_DefaultAtkDelay = 6;
    protected const int E_DefaultProjNum = 3;

    protected int increaseRate => SetIncreaseRate(GameManager.Game.phase);
    protected int SetIncreaseRate(int phase) //페이즈 증가량은 무한모드에만 적용
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

    [Header("최종 스텟")]
    public int id;
    public EnemyType type;

    [SerializeField] protected int maxHp;
    [SerializeField] protected int curHp;

    [SerializeField] protected int damage;
    [SerializeField] protected int moveSpeed;
    [SerializeField] protected int attackSpeed;
    [SerializeField] protected int expAmount;
    [SerializeField] protected int scoreAmount;

    [SerializeField] protected int curProjNum; //해당 적이 한번 공격할때 발사하는 발사체 양

    [SerializeField] protected float curAtkDelay; //최종 계산된 어택 딜레이(초)

    [SerializeField] protected Coroutine enemyBehavior; //적의 행동 코루틴을 저장

    [Header("상태 스텟")]
    [SerializeField] protected bool isEliminatable; //시스템적 삭제가 가능
    [SerializeField] protected bool isDropItem; //죽으면 아이템을 드롭
    [SerializeField] protected bool isStop; //움직임을 정지한 상태
    [SerializeField] protected bool isAttack; //공격중인 상태
    [SerializeField] protected bool isShocked; //플레이어의 스킬에 의한 상태, 속도 및 공속 감소

    protected virtual void OnDisable() => ResetObject();

  
    private void Start()
    {
        SetEnemy();
    }

    #region 초기화 및 설정
    /// <summary>
    /// 적의 시작 함수. 이 함수를 기점으로 적의 스텟 설정 및 행동 시작
    /// </summary>
    /// <param name="enemyId"></param>
    /// <param name="increaseRate"></param>
    public void SetEnemy() //5분 이전 100% 10분전 150%, 15분전 200% 20분전 250% 25분전 300% 30분전 500%
    {
        InitializeEnemyData();
        StartCoroutine(EnemyBehavior());
    }

    //스텟 설정, 스프라이트 설정, 초기HP바 설정
    private void InitializeEnemyData()
    {
        SetStat(); //스텟을 설정
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
        scoreAmount = data.scoreAmount * (increaseRate / 100); //기본 스텟에 증가치를 곱함
        //isStopByLine = data.isStop;
        //isAimAttack = data.isAim;
        curHp = maxHp;

        
        gameObject.name = DataManager.master.GetData(id).name;

        //상태 초기화
        isEliminatable = false;
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
        if (hpBar == null)
        {
            Transform hpBarParent = GameManager.canvas.Find("E_HpBars");
            hpBar = GameManager.InstantObject(GameManager.LoadFromResources<GameObject>(HpBarPath), hpBarParent).GetComponent<Slider>();

            RectTransform hpRect = hpBar.GetComponent<RectTransform>();
            hpRect.position = 
            hpRect.sizeDelta = new Vector2(transform.localScale.x * 200, hpRect.sizeDelta.y);
            hpBar.value = 1;
            hpBar.name = $"{gameObject.name}'s maxHp";
            hpBar.gameObject.SetActive(false);
        }
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
        hpBar?.gameObject.SetActive(false);
        if (enemyBehavior != null)
        {
            StopCoroutine(enemyBehavior);
            enemyBehavior = null;
        }
    }

    #endregion

    #region 피해 및 제거
    /// <summary>
    /// 적에게 데미지를 줄때 이 함수 사용
    /// </summary>
    /// <param name="hitObject"></param>
    /// <param name="damage"></param>
    public void EnemyDamaged(GameObject hitObject, int damage)
    {
        ActiveHitEffect();
        curHp = Mathf.Max(curHp - damage, 0);
        UpdateHpBarValue();
        if(curHp == 0)
        {
            EnemyDeath();
        }
        Debug.Log($"{gameObject.name}이 {hitObject}에 의해 {damage}의 데미지를 입음");
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

    //프레임마다 체크
    protected void UpdateHpBarPos()
    {
        //hp바가 비활성화 되어있다면 체크 안함 => UpdateHpBarValue를 통해 먼저 활성화되어야함
        if (!hpBar.gameObject.activeSelf)
        {
            return;
        }
        hpBar.transform.position = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y - transform.localScale.y, 0));
    }

    protected void ActiveHitEffect() => StartCoroutine(HitEffect());

    private Color hitColor = new Color(1, 184 / 255f, 184 / 255f);
    private IEnumerator HitEffect()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    //시스템에 의한 보상이 주어지지 않는 제거
    public void EliminateEnemy()
    {
        if (!isEliminatable)
        {
            return;
        }

        //보더에 부딫히는등 시스템에 의한 제거
        GameManager.Game.Pool.ReleasePool(gameObject);
    }

    //exp, 점수, 아이템 등 보상이 주어지는 제거
    public void EnemyDeath()
    {
        DropExp();
        AddEnemyScoreToStageScore();

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

    //점수 추가
    private void AddEnemyScoreToStageScore() => GameManager.Game.Score += scoreAmount;



    private void DropItem()
    {
        OtherProjType projType = DetermineProjType();
        GameManager.Game.Pool.GetOtherProj(projType, transform.position, transform.rotation);
    }

    private OtherProjType DetermineProjType()
    {
        return PlayerMain.pStat.IG_WeaponLv < 3
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

    #region 공격 및 행동

    //적의 행동 코루틴 -> 각 개체 별로 오버라이딩
    protected virtual IEnumerator EnemyBehavior() 
    {
        UpdateHpBarPos();
        if (IsVisibleFrom()) //화면 안에 들어오면 삭제가 가능함
        {
            isEliminatable = true;
            Debug.Log($"{gameObject.name} 삭제준비");
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
    /// 기본적으로 그냥 위로 이동
    /// </summary>
    protected void Move()
    {
        transform.position += transform.up * moveSpeed * Time.deltaTime;
    }

    public EnemyProjectile FireSingle(OtherProjType _proj, int _dmgRate, int _liveTime = 0, int _size = 0, bool _isAim = false)
    {
        EnemyProjectile proj = GameManager.Game.Pool.GetOtherProj(_proj, transform.position, transform.rotation).GetComponent<EnemyProjectile>();

        if (_isAim)
        {
            Vector3 dir = (PlayerMain.Instance.transform.position - transform.position).normalized;
            proj.transform.up = dir;
        }
        else
        {
            proj.transform.up = transform.up;
        }

        proj.SetProjParameter(_dmgRate, _liveTime, _size);
        return proj;
    }

    public void FireMulti(OtherProjType _proj, int _dmgRate, int _liveTime = 0, int _size = 0, int _projectileCount = 1, float _spreadAngle = 0, bool _isAim = false)
    {
        // _isAim이 false이면 전방을 기준, true이면 플레이어의 방향을 기준으로 함
        Vector3 pDir = Vector3.zero;
        if (_isAim)
        {
            // 플레이어의 위치를 기준으로 방향을 계산
            pDir = (PlayerMain.Instance.transform.position - transform.position).normalized;
        }

        // 발사되는 각도 범위 설정
        float startAngle = -_spreadAngle / 2;
        for (int i = 0; i < _projectileCount; i++)
        {
            // 각 발사체가 발사되는 각도 계산(단 발사체가 한개라면 angle을 0으로 초기화)
            float angle = startAngle + (i * (_spreadAngle / (_projectileCount - 1)));
            if (_projectileCount == 1)
            {
                angle = 0;
            }

            Vector3 dir = _isAim ? Quaternion.Euler(0, 0, angle) * pDir : Quaternion.Euler(0, 0, angle) * transform.up;
            dir.Normalize();
            EnemyProjectile proj = FireSingle(_proj, _dmgRate, _liveTime, _size, _isAim);
            proj.transform.up = dir;
        }
    }

    ////일정 시간마다 반복 하여 발사함
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

    //기본 :  적보더에 트리거되면 삭제
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
