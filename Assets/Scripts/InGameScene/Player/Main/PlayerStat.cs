using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public const int basicStat = 10; //레벨1의 가장 기본 스텟
    public GameObject selectSkillUI;// todo => 나중엔 인터페이스 적용

    [Header("기본 정보")]
    public int curPlayerID;
    public bool isFirstSetDone;

    [Header("초기스텟 + 패시브 스텟 적용 \n 각 요소에 직접 참조됨")]
    public float damage; //추가스텟 적용 이후 공격 혹은 이동에 직접 쓰일 변수(패시브 스킬이 추가된 스텟)
    public float defence;
    public float moveSpeed;
    public float attackSpeed;
    public float maxHp;
    public float curHp;

    [Header("초기스텟(기본+파츠스텟+기체레벨스텟)")]
    private float initDamage; //추가스텟이 적용되지 않은 기본의 스텟 변수(기체 레벨 추가스텟 + 파츠 레벨이 적용됨)
    private float initDefence;
    private float initMoveSpeed;
    private float initAttackSpeed;
    private float initHp;

    [Header("패시브 증가율")]
    public float damageIncreaseRate;
    public float defenceIncreaseRate;
    public float moveSpeedIncreaseRate;
    public float attackSpeedIncreaseRate;
    public float hpRegenRate;

    private PlayerControl playerController;

    public int weaponLevel;
    public int rewardRate; //아웃게임에서의 보상 증가율(파츠 능력 등) => 미네랄 , 재료 등의 개수 증가
    public int rewardRateIncrease; //인게임에서의 보상 증가율 => other스킬의 보상 증가

    //레벨 관련
    public int level; //캐릭터의 레벨이 아닌 인게임에서의 레벨
    public int nextExp; //목표 exp. curExp >= nextExp 라면 레벨업 판정
    public int curExp; //현재 가지고 있는 exp
    public int GetExp
    {
        get => curExp;
        set
        {
            curExp += value;
            if(curExp >= nextExp)
            {
                LevelUP();
            }
        }
    }

    public bool CanMove;
    public bool CanAttack;
    public bool InvincibleState;
    public bool isKnockbackRun;
    public float invincibleTime = 3f;

    public int specialCount;
    public int powerLevel; //스페셜 증가치
    public float powerIncreaseRate; //초당 파워 증가율
    public float curPowerValue;
    

    public float specialDamageRate; //스페셜 스킬의 데미지 증가율. 파츠나 어빌리티에 의해 증가


    private void Awake()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerControl>();
        
    }

    private void Start()
    {
        
    }

    public void Init()
    {
        
        damageIncreaseRate = 1;
        defenceIncreaseRate = 1;
        moveSpeedIncreaseRate = 1;
        attackSpeedIncreaseRate = 1;
        hpRegenRate = 0;

        isFirstSetDone = false; 
        int savedPlayerId = DataManager.account.GetChar();
        curPlayerID = savedPlayerId;
        SetStat(curPlayerID);

        PlayerSkillManager ps = PlayerMain.pSkill;
        //ps.AddPassiveSkill((InGamePassiveSkill)ps.FindSkillByCode(641));

        level = 1;
        nextExp = 5;
        curExp = 0;

        specialCount = 3;
        powerLevel = 0;
        powerIncreaseRate = 1f;
        curPowerValue = 0;
        specialDamageRate = 1f; //증가율

    }

    public void ChangePlayer(int id)
    {
        curPlayerID = id;
        SetStat(curPlayerID);
    }

    public void SetStat(int playerId)
    {
        Debug.Log("플레이어 스텟 설정");
        PlayerSet(playerId);
        maxHp = initHp;
        curHp = maxHp;

        ApplyStat();
    }

    /// <summary>
    /// 플레이어 스텟 데이터에서 해당 id의 플레이어의  초기 스텟을 설정
    /// </summary>
    public void PlayerSet(int id)
    {
        CharData curPlayerChar = DataManager.character.GetData(id);

        ////아웃게임에서 받아온 캐릭터의 스텟 todo -> 이부분은 특정 데이터베이스를 통해 받아올예정
        //level = curPlayerChar.level;
        //initDamage = curPlayerChar.damage;
        //initDefence = curPlayerChar.defense;
        //initMoveSpeed = curPlayerChar.moveSpeed;
        //initAttackSpeed = curPlayerChar.attackSpeed;
        //initHp = curPlayerChar.hp;

        level = 1;
        initDamage = 10;
        initDefence = 10;
        initMoveSpeed = 10;
        initAttackSpeed = 10;
        initHp = 100;
    }

    /// <summary>
    /// 패시브를 통해 강화된 스텟
    /// </summary>
    public void ApplyStat()
    {
        damage = initDamage * damageIncreaseRate;
        defence = initDefence * defenceIncreaseRate;
        moveSpeed = initMoveSpeed * moveSpeedIncreaseRate;
        attackSpeed = initAttackSpeed * attackSpeedIncreaseRate;
    }

    /// <summary>
    /// 플레이어가 데미지를 받을 경우 데미지 적용 및 데미지액션 실행
    /// </summary>
    public void PlayerDamaged(float damage, GameObject attackObj)
    {
        if (!InvincibleState)
        {
            float applyDamage = damage * (1 - (0.01f * defence)); //todo => 방어력 로직 다시 체크

            curHp -= applyDamage;
            Debug.Log(attackObj.name + " 에 의해 " + applyDamage + " 의 데미지를 입음");
            PlayerDamagedAction(attackObj); //넉백 및 무적 부여
        }
    }

    /// <summary>
    /// 무적상태 돌입 및 뒤로 밀림
    /// </summary>
    /// <param name="attack_obj"></param>
    public void PlayerDamagedAction(GameObject attack_obj)
    {
        if (InvincibleState) return;

        Collider2D attackingCollider = attack_obj.GetComponent<Collider2D>();
        if (attackingCollider != null)
        {
            StartCoroutine(ActiveInvincible(invincibleTime));
            PlayerKnockBack(attackingCollider);
        }
    }

    //플레이어 넉백
    public void PlayerKnockBack(Collider2D collision)
    {
        Vector2 curPosition = transform.position;
        Vector2 targetPosition = CalculateTargetPos(curPosition, collision);

        // 코루틴이 실행 중이 아니면 실행
        if (!isKnockbackRun)
        {
            StartCoroutine(PushbackLerp(curPosition, targetPosition));
        }
    }
    #region 넉백 계산
    private Vector2 CalculateTargetPos(Vector3 curpos, Collider2D col)
    {
        Vector2 pushDirection = (curpos - col.transform.position).normalized;
        return curpos - new Vector3(-pushDirection.x, -pushDirection.y, 0) * 2;
    }

    private IEnumerator PushbackLerp(Vector3 startPos, Vector3 endPos)
    {
        isKnockbackRun = true; // 코루틴 실행 상태 설정
        float startTime = Time.time;
        float elapseTime = 0f;

        while (elapseTime < 0.1f)
        {
            // 충돌 상태를 확인
            if (CheckCollide())
            {
                break;
            }

            float t = Mathf.Clamp01(elapseTime / 0.1f);
            transform.position = Vector3.Lerp(startPos, endPos, t);
            elapseTime = Time.time - startTime;

            yield return null;
        }

        isKnockbackRun = false; // 코루틴 종료 상태 설정
    }

    private bool CheckCollide()
    {
        PlayerControl pControl = PlayerMain.pControl;

        return (pControl.isTopCollide || pControl.isBottomCollide || pControl.isLeftCollide || pControl.isRightCollide);
    }
    #endregion

    //무적 상태 부여
    private IEnumerator ActiveInvincible(float invincible_time)
    {
        InvincibleState = true;
        yield return new WaitForSeconds(invincible_time);
        InvincibleState = false;
    }


    

    public void LevelUP()
    {
        level++;
        nextExp += 5; //임시 처리. 이후 정확한 공식 대입

        selectSkillUI.SetActive(true);

        Time.timeScale = 0f;
    }

    //private void MakePlayerTranslucent()
    //{
    //    playerSprite.color = isInvincibleState
    //        ? new Color(1, 1, 1, 0.5f)
    //        : new Color(1, 1, 1, 1f);
    //}

    public const float powlv1Max = 5;
    public const float powlv2Max = 15;
    public const float powMax = 30;
    private IEnumerator PowerValueIncrease()
    {
        while (true)
        {
            if (!GameManager.Instance.BattleSwitch|| curPowerValue >= powMax)
            {
                yield return null;
                continue;
            }



            //if ( && !isSkillActivating)
            //{
            //    //파워가 맥스치보다 적거나 스페셜이 비활성화일 경우에만 파워가 올라간다.
                
            //}

            curPowerValue += Time.deltaTime * powerIncreaseRate;
            PowerLvSet();
            yield return null;
        }
    }


    private void PowerLvSet()
    {
        float powerIncrease = curPowerValue;
        if (powerIncrease > powlv1Max && powerLevel == 0)
        {
            powerLevel = 1;
        }
        else if (powerIncrease > powlv2Max && powerLevel == 1)
        {
            powerLevel = 2;
        }
        else if (powerIncrease >= powMax && powerLevel == 2)
        {
            powerLevel = 3;
        }
    }



    private void OnTriggerStay2D(Collider2D collision)
    {
        if (InvincibleState)
        {
            return;
        }

        if (collision.CompareTag("Enemy"))
        {
            PlayerDamaged(collision.GetComponent<EnemyObject>().enemyStat.damage / 2, collision.gameObject);
        }
    }
}