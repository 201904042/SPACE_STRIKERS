using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour
{
    public const int basicStat = 10; //레벨1의 가장 기본 스텟
    public GameObject selectSkillUI;

    private PlayerUI pUI => PlayerMain.pUI;
    private PlayerControl pControl => PlayerMain.pControl;

    [Header("기본 정보")]
    public int curPlayerID;
    public bool isFirstSetDone;

    [Header("최종 스텟. 다른 스크립트에 참조되는 스텟")]
    //InGame
    public float IG_Dmg; //추가스텟 적용 이후 공격 혹은 이동에 직접 쓰일 변수(패시브 스킬이 추가된 스텟)
    public float IG_Dfs;
    public float IG_MSpd;
    public float IG_ASpd;
    public float IG_Hp;
    private float IG_CurHp;
    public float CurHp
    {
        get => IG_CurHp;
        set
        {
            IG_CurHp = Mathf.Min(value, IG_Hp);
            pUI.HpBarChange();
        }
    }
    public float IG_HpRegen;
    public int IG_WeaponLv; //최종 무기 레벨
    public int IG_RewardRate; //아웃게임에서의 보상 증가율(파츠 능력 등) => 미네랄 , 재료 등의 개수 증가

    //레벨 관련
    public int IG_Level; //캐릭터의 레벨이 아닌 인게임에서의 레벨
    public int IG_MaxExp; //목표 exp. IG_CurExp >= IG_MaxExp 라면 레벨업 판정
    public int MaxExpInstance; //레벨업을 할때 기존 목표exp값을 복사. 스킬을 고르지 않고 반환할경우 해당 값을 목표exp로 삼음
    private int IG_CurExp; //현재 가지고 있는 exp
    public float MaxExpInceaseRate = 1.5f;
    public int CurExp
    {
        get => IG_CurExp;
        set
        {
            IG_CurExp = value;
            if (IG_CurExp >= IG_MaxExp)
            {
                LevelUp();
            }

            pUI.HpBarChange();
        }
    }

    private int IG_USkillCount;
    public int USkillCount
    {
        get => IG_USkillCount;
        set
        {
            if(value < 0)
            {
                IG_USkillCount = 0;
                return;
            }
            IG_USkillCount = value;
            pUI.SetUniquSkillCount();
        }
    }
    public int IG_curPowerLevel; //스페셜 증가치
    public float IG_PowIncreaseRate; //초당 파워 증가율
    private float curPower; //현재까지 모은 power의 양
    
    public float CurPow
    {
        get => curPower;
        set
        {
            curPower = value;
            pUI.ExpBarChange();
        }
    }

    //무적시간
    public float invincibleTime = 3f;

    [Header("초기스텟(아웃게임에서 받아온 스텟)")]
    //OutGame
    private float OG_Dmg; //추가스텟이 적용되지 않은 기본의 스텟 변수(기체 레벨 추가스텟 + 파츠 레벨이 적용됨)
    private float OG_Dfs;
    private float OG_MSpd;
    private float OG_ASpd;
    private float OG_Hp;
    private float OG_HpRegen;
    public int OS_UDmgUp; //아웃게임에서 받아온 특수스킬 뎀증율
    


    [Header("인게임 증가율(스킬 등)")]
    // PassiveSkill
    public float PS_Dmg;
    public float PS_Dfs;
    public float PS_MSpd;
    public float PS_ASpd;
    public float PS_HpRegen;

    // ExtraSkill
    public int ES_RewardUp; //인게임에서의 보상 증가율 => other스킬의 보상 증가


    [Header("각종 상태")]
    public bool CanMove;
    public bool CanAttack;
    public bool InvincibleState;
    public bool isKnockbackRun;


    public void Init()
    {
        
        PS_Dmg = 1;
        PS_Dfs = 1;
        PS_MSpd = 1;
        PS_ASpd = 1;
        PS_HpRegen = 0;

        isFirstSetDone = false;
        int savedPlayerId = DataManager.account.GetChar();
        curPlayerID = savedPlayerId;
        SetStat(curPlayerID);

        
        IG_WeaponLv = 1;

        IG_Level = 1;
        IG_MaxExp = 5;
        IG_CurExp = 0;

        IG_USkillCount = 3;
        IG_curPowerLevel = 0;
        curPower = 0;
        IG_PowIncreaseRate = 1f;
        
        OS_UDmgUp = 100; //증가율 100 = 기본 스텟 * 1

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
        ApplyStat();
        CurHp = IG_Hp;
    }

    /// <summary>
    /// 플레이어 스텟 데이터에서 해당 id의 플레이어의  초기 스텟을 설정
    /// </summary>
    public void PlayerSet(int id)
    {
        CharData curPlayerChar = DataManager.character.GetData(id);

        ////아웃게임에서 받아온 캐릭터의 스텟 todo -> 캐릭터 스텟 + 장착파트의 스텟
        //IG_Level = curPlayerChar.IG_Level;
        //OG_Dmg = curPlayerChar.IG_Dmg;
        //OG_Dfs = curPlayerChar.defense;
        //OG_MSpd = curPlayerChar.IG_MSpd;
        //OG_ASpd = curPlayerChar.IG_ASpd;
        //OG_Hp = curPlayerChar.hp;

        IG_Level = 1;
        OG_Dmg = 10;
        OG_Dfs = 10;
        OG_MSpd = 10;
        OG_ASpd = 10;
        OG_Hp = 100;
        OG_HpRegen = 0;
    }

    /// <summary>
    /// 패시브를 통해 강화된 스텟
    /// </summary>
    public void ApplyStat()
    {
        IG_Dmg = OG_Dmg * PS_Dmg;
        IG_Dfs = OG_Dfs * PS_Dfs;
        IG_MSpd = OG_MSpd * PS_MSpd;
        IG_ASpd = OG_ASpd * PS_ASpd;
        IG_Hp = OG_Hp;
        OG_HpRegen = OG_HpRegen * PS_HpRegen;
    }

    [ContextMenu("레벨업")]
    public void LevelUp()
    {
        IG_Level++;
        MaxExpInstance = IG_MaxExp; //기존 목표exp 복사
        IG_MaxExp = (int)((float)IG_MaxExp * MaxExpInceaseRate); //목표 exp 증가
        CurExp = 0;

        GameManager.Game.UI.IGetSkill.OpenInterface();
        Time.timeScale = 0f;
    }

    /// <summary>
    /// 플레이어가 데미지를 받을 경우 데미지 적용 및 데미지액션 실행
    /// </summary>
    public void PlayerDamaged(float damage, GameObject attackObj)
    {
        if (!InvincibleState)
        {
            float applyDamage = damage * (1 - (0.01f * IG_Dfs)); //todo => 방어력 로직 다시 체크

            CurHp = CurHp - applyDamage;
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

    //무적 상태 부여 코루틴
    private IEnumerator ActiveInvincible(float invincible_time)
    {
        SpriteRenderer playerSprite = PlayerMain.Instance.GetComponent<SpriteRenderer>();
        if (playerSprite == null)
        {
            Debug.LogError("플레이어의 스프라이트 렌더러를 찾을 수 없음");
        }
        InvincibleState = true;
        playerSprite.color = new Color(1, 1, 1, 0.5f);
        yield return new WaitForSeconds(invincible_time);
        InvincibleState = false;
        playerSprite.color = new Color(1, 1, 1, 1f);
    }

    



}