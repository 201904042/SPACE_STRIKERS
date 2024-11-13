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
    public UnityEngine.UI.Slider hpBar; //해당 적에게 할당된 HPBar UI객체. 반드시 소유하고 있어야함

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

    public int id;
    public EnemyType type;

    //각 에너미 데이터의 스텟 * 게임페이즈의 증폭률 반영된 스텟
    [SerializeField] protected float maxHp;
    [SerializeField] protected float curHp;
    [SerializeField] protected int damage;
    [SerializeField] public int moveSpeed;
    [SerializeField] protected int attackSpeed;
    [SerializeField] protected int expAmount;
    [SerializeField] protected int scoreAmount;

    //상속정의되는 현재 공격에 사용될 발사체 수와 공격 사이의 딜레이
    [SerializeField] protected int curProjNum; //해당 적이 한번 공격할때 발사하는 발사체 양 -> 상속 정의
    [SerializeField] protected float curAtkDelay; //최종 계산된 어택 딜레이(초) -> 상속 정의

    //활성화와 동시에 실행되는 적의 행동(업데이트의 역할)
    [SerializeField] protected Coroutine enemyBehavior; //적의 행동 코루틴을 저장

    [Header("상태 스텟")]
    [SerializeField] protected bool isEliminatable; //시스템적 삭제가 가능
    [SerializeField] public bool isAttackable; //시스템적 삭제가 가능
    [SerializeField] protected bool isDamageable; //시스템적 삭제가 가능
    [SerializeField] protected bool isDropItem; //죽으면 아이템을 드롭
    [SerializeField] protected bool isStop; //움직임을 정지한 상태
    [SerializeField] protected bool isAttack; //공격중인 상태
    [SerializeField] public bool isShocked; //플레이어의 스킬에 의한 상태, 속도 및 공속 감소

    protected virtual void OnDisable() => ResetObject();

    
    private void OnEnable()
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
        curHp = maxHp;

        gameObject.name = DataManager.master.GetData(id).name;

        //상태 초기화
        isEliminatable = false;
        isAttackable = false;
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
            hpBar = Instantiate(Resources.Load<GameObject>(HpBarPath), hpBarParent).GetComponent<Slider>();
            hpBar.name = $"{gameObject.name}'s maxHp";
            //RectTransform hpRect = hpBar.GetComponent<RectTransform>();
            //hpRect.sizeDelta = new Vector2(200, hpRect.sizeDelta.y);
        }
        
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
        isEliminatable = false;
        isAttackable = false;
        isDropItem = false;

        GetComponent<SpriteRenderer>().color = Color.white;
        if (hpBar.gameObject.activeSelf)
        {
            hpBar.gameObject.SetActive(false);
            hpBar.value = hpBar.minValue;
            hpBar.value = 1;
        }
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
    public virtual void EnemyDamaged(GameObject hitObject, int damage)
    {
        //Debug.Log($"{gameObject.name}이 {hitObject}에 의해 {damage}의 데미지를 입음");
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

    //점수 추가
    private void AddEnemyScoreToStageScore() => GameManager.Game.Score += (int)scoreAmount;



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

    #region 공격 및 행동

    //적의 행동 코루틴 -> 각 개체 별로 오버라이딩
    protected virtual IEnumerator EnemyBehavior() 
    {
        UpdateHpBarPos();
        
        if ((!isEliminatable || !isDamageable) && IsVisibleFrom() ) //화면 안에 들어오면 삭제가 가능함
        {
            isEliminatable = true;
            isAttackable = true;
            isDamageable = true;
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

    protected IEnumerator MoveToPosition(Vector3 targetPosition, float speed)
    {
        // 대상 위치에 도달할 때까지 이동
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
            // 플레이어를 향한 방향을 계산하고 angle만큼 회전시킴
            Vector3 dir = (PlayerMain.Instance.transform.position - transform.position).normalized;
            proj.transform.up = Quaternion.Euler(0, 0, angle) * dir;
        }
        else
        {
            // transform.up 방향에서 angle만큼 회전시킴
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
            // 각 발사체가 발사되는 각도 계산
            float angle = startAngle + (i * (_spreadAngle / (_projectileCount - 1)));
            if (_projectileCount == 1)
            {
                angle = 0;
            }

            proj[i] = FireSingle(_proj, angle, _isAim);
        }

        return proj;
    }

    #endregion


    public int GetCollisionDamage()
    {
        return (int)damage;
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
