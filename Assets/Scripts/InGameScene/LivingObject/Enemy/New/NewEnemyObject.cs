using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NewEnemyObject : MonoBehaviour
{
    private const string HpBarPath = "UI/Ingame/Enemy/E_HpBar";

    public int id;
    public EnemyType type;

    [Header("최종 스텟")]
    public int maxHp;
    public int damage;
    public int moveSpeed;
    public int attackSpeed;
    public int expAmount;
    public int scoreAmount;
    public bool isStopByLine;
    public bool isAimAttack;

    private int curHp;
    private Slider hpBar;

    private Coroutine enemyBehavior;

    private bool isEliminatable;
    private bool isDropItem;

    private void OnDisable() => ResetObject();
    private void OnBecameVisible() => isEliminatable = true; //카메라에 보인 시점부터 파괴 가능


    /// <summary>
    /// 적의 시작 함수. 이 함수를 기점으로 적의 스텟 설정 및 행동 시작
    /// </summary>
    /// <param name="enemyId"></param>
    /// <param name="increaseRate"></param>
    public void SetEnemy(int enemyId, int increaseRate = 100)
    {
        InitializeEnemyData(enemyId, increaseRate);
        StartCoroutine(EnemyBehavior());
    }

    //스텟 설정, 스프라이트 설정, 초기HP바 설정
    private void InitializeEnemyData(int enemyId, int increaseRate, bool _isDropItem = false)
    {
        SetStat(enemyId, increaseRate); //스텟을 설정
        SetSprite(enemyId); //아이디에 의거한 적 스프라이트 설정
        SetHpBar(); //적 개체에 대한 HP바를 할당 

        isDropItem = _isDropItem;
    }

    //적의 행동 코루틴 -> 각 개체 별로 오버라이딩
    protected virtual IEnumerator EnemyBehavior() { yield return null; }

    
    private void SetStat(int enemyId, int increaseRate) 
    {
        EnemyData data = DataManager.enemy.GetData(enemyId);
        id = enemyId;
        type = data.type;
        maxHp = data.hp * increaseRate;
        damage = data.damage * increaseRate;
        moveSpeed = data.moveSpeed;
        attackSpeed = data.attackSpeed;
        expAmount = data.expAmount * increaseRate;
        scoreAmount = data.scoreAmount * increaseRate;
        isStopByLine = data.isStop;
        isAimAttack = data.isAim;
        curHp = maxHp;
        gameObject.name = DataManager.master.GetData(enemyId).name;

        isEliminatable = false;
    }

    private void SetSprite(int id)
    {
        var enemySprite = GameManager.LoadFromResources<Sprite>(DataManager.master.GetData(id).spritePath);
        if (enemySprite != null)
        {
            GetComponent<SpriteRenderer>().sprite = enemySprite;
        }
    }

    private void SetHpBar()
    {
        if (hpBar == null)
        {
            Transform hpBarParent = GameManager.canvas.Find("E_HpBars");
            hpBar = GameManager.InstantObject(GameManager.LoadFromResources<GameObject>(HpBarPath), hpBarParent).GetComponent<Slider>();

            RectTransform hpRect = hpBar.GetComponent<RectTransform>();
            hpRect.sizeDelta = new Vector2(transform.localScale.x * 200, hpRect.sizeDelta.y);
            hpBar.value = 1;
            hpBar.name = $"{gameObject.name}'s maxHp";
            hpBar.gameObject.SetActive(false);
        }
    }

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
        Debug.Log($"{gameObject.name}이 {hitObject}에 의해 {damage}의 데미지를 입음");
    }

    protected void UpdateHpBarValue()
    {
        if (!hpBar.gameObject.activeSelf)
        {
            hpBar.gameObject.SetActive(true);
        }
        hpBar.value = (float)curHp / maxHp;
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


    public void ActiveHitEffect() => StartCoroutine(HitEffect());

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


    public void ResetObject()
    {
        id = 0;
        type = EnemyType.None;
        maxHp = 0;
        curHp = 0;
        damage = 0;
        moveSpeed = 0;
        attackSpeed = 0;
        expAmount = 0;
        scoreAmount = 0;
        isStopByLine = false;
        isAimAttack = false;
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

    

    //Attack루틴 등에 쓰일 공격방식
    public EnemyProjectile FireSingle(OtherProjType _proj, int _speed, int _dmgRate, int _liveTime = 0, int _size = 0,  bool _isAim = false)
    {
        EnemyProjectile proj = GameManager.Game.Pool.GetOtherProj(_proj, transform.position, transform.rotation).GetComponent<EnemyProjectile>();
        proj.SetProjParameter(_speed, _dmgRate, _liveTime, _size);
        //파라미터 세팅 : 속도, 데미지증폭, 생성시간, 크기 등. 이는 오버라이드를 통해 오브젝트 별로 실행
        return proj;
    }

    public void FireMulti(OtherProjType _proj, int _speed, int _dmgRate, int _liveTime =0 , int _size = 0, int _projectileCount = 1, float _spreadAngle = 0, bool _isAim = false)
    {
        float startAngle = -_spreadAngle / 2;
        float angleStep = _spreadAngle / (_projectileCount - 1);

        for (int i = 0; i < _projectileCount; i++)
        {
            float angle = startAngle + (angleStep * i);
            Vector3 dir = Quaternion.Euler(0, 0, angle) * transform.up;
            EnemyProjectile proj = FireSingle(_proj, _speed, _dmgRate , _liveTime , _size , _isAim);
            proj.transform.up = dir;
        }
    }


    private bool CheckEliminatable()
    {
        if (!isEliminatable)
        {
            return false;
        }
        if(type == EnemyType.Boss || type == EnemyType.MidBoss)
        {
            return false;
        }

        return true;
    }
}
