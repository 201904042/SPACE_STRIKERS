using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAbility
{
    public float Damage;
    public float Defense;
    public float MoveSpeed;
    public float AttackSpeed;
    public float MaxHealth;
    public float HealthRegen;

    public float MobDamageRate;
    public float BossDamageRate;
    public float InGameExpRate;
    public float ItemDropRate;
    public float PowerGaugeSpeedRate;
    public float AccountExpRate;
    public float RewardRate;
    public int SpecialAttackCount;
    public int StartLevel;
    public int ReviveCount;
    public int StartWeaponLevel;
}

public class PlayerStat : MonoBehaviour
{
    public const int DefaultCount = 1;
    public const int DefaultRate = 100;
    public const int DefaultUSkillCount = 3;
    public const int BasicStat = 10; //레벨1의 가장 기본 스텟
    public const float MaxExpUpRate = 2; //레벨업마다 증가하는 목표exp의 비율
    public const float DefaultInvincibleTime = 3f; //무적시간
    private PlayerUI pUI => PlayerMain.pUI;
    private PlayerControl pControl => PlayerMain.pControl;

    [Header("기본 정보")]
    public int curPlayerID;

    //아웃게임에서 받아온 스텟
    public PlayerAbility OG_Stat;


    //인게임에 사용 스텟

    //필수
    public float IG_Dmg { get; private set; }
    public float IG_Dfs { get; private set; }
    public float IG_MSpd { get; private set; }
    public float IG_ASpd { get; private set; }
    public float IG_Regen { get; private set; }
    public float IG_MaxHp { get; private set; }
    

    //선택
    public float IG_MobDamageRate { get; private set; } //외부 => troopbase
    public float IG_BossDamageRate { get; private set; } //외부 => bossbase
    public float IG_InGameExpRate { get; private set; } //내부 => stat
    public float IG_ItemDropRate { get; private set; } //외부 => spawnManager
    public float IG_PowerGaugeSpeed { get; private set; } //내부 =>
    public float IG_AccountExpRate { get; private set; } //외부
    public int IG_Life { get; private set; } //내.외부

    

    //외부에 의해 바뀔수 있음
    public float IG_RewardRate;
    public int IG_curWeaponLv;
    public int IG_Level;
    public int levelInstance;
    public int IG_MaxExp;
    public int MaxExpInstance; //반환시 이전 maxExp를 설정하기 위함

    //인게임 스텟 옵저버

    [SerializeField] private float IG_CurHp;
    [SerializeField] private float IG_PowValue; //지금까지 모인 파워값
    [SerializeField] private float IG_CurExp;
    [SerializeField] private int IG_USkillCount;

    public float CurHp
    {
        get => IG_CurHp;
        set
        {
            IG_CurHp = Mathf.Min(value, IG_MaxHp);
            if (IG_CurHp <= 0) //현재 hp가 0이 될경우 라이프 감소 후 hp를 최대의 절반으로 변경
            {
                IG_Life--;
                if (IG_Life > 0)
                {
                    CurHp = IG_MaxHp / 2;
                }
            }
            pUI.HpBarChange();
        }
    }
    public float CurExp
    {
        get => IG_CurExp;
        set
        {
            IG_CurExp = value;
            if (IG_CurExp >= IG_MaxExp)
            {
                IG_Level++;
            }

            pUI.ExpBarChange();
        }
    }
    public int CurUSkillCount
    {
        get => IG_USkillCount;
        set
        {
            if (value < 0)
            {
                IG_USkillCount = 0;
                return;
            }
            IG_USkillCount = value;
            pUI.SetUniquSkillCount();
        }
    }
    public float CurPow
    {
        get => IG_PowValue;
        set
        {
            IG_PowValue = value;
            PowerLvSet(IG_PowValue);
            pUI.PowBarChange();
        }
    }
    public int IG_curPowerLevel; //파워값에 비례한 파워 레벨

    [Header("인게임 패시브 스킬 증가율")]
    // PassiveSkill
    public float PS_Dmg = 0;
    public float PS_Dfs = 0;
    public float PS_MSpd = 0;
    public float PS_ASpd = 0;
    public float PS_HpRegen = 0;

    public void Init()
    {
        curPlayerID = DataManager.account.GetChar();
        PlayerMain.Instance.SetCharSprite(curPlayerID);

        SetStat(curPlayerID);

        PlayerMain.pUI.HpBarChange();
        PlayerMain.pUI.ExpBarChange();
        PlayerMain.pUI.PowBarChange();
        PlayerMain.pUI.SetUniqueSkillImage(curPlayerID);
        PlayerMain.pUI.SetUniquSkillCount();
    }

    public void ChangePlayer(int id)
    {
        curPlayerID = id;
        PlayerMain.Instance.SetCharSprite(curPlayerID);
        PlayerMain.pUI.SetUniqueSkillImage(curPlayerID);
        SetStat(curPlayerID);
    }

    private void InitInGameStat()
    {
        IG_Dmg = 0;
        IG_Dfs = 0;
        IG_MSpd = 0;
        IG_ASpd = 0;
        IG_Regen = 0;
        IG_MaxHp = 0;


        //선택
        IG_MobDamageRate = DefaultRate;
        IG_BossDamageRate = DefaultRate;
        IG_InGameExpRate = DefaultRate;
        IG_ItemDropRate = 30;
        IG_PowerGaugeSpeed = DefaultRate;
        IG_USkillCount = DefaultUSkillCount;
        IG_AccountExpRate = DefaultRate;
        IG_Life = DefaultCount;
        IG_curWeaponLv = DefaultCount;
        IG_RewardRate = DefaultRate;
        IG_Level = DefaultCount;
        levelInstance = IG_Level;
        IG_MaxExp = 5;
    }

    public void SetStat(int playerId)
    {
        InitInGameStat(); //스텟들 초기화
        OG_Stat = new PlayerAbility();
        SetOutGameStat(playerId); //OG_Stat 설정

        //OG스텟를 기반으로 IG스텟 설정
        SetPassiveStat(); //혹시모르니 패시브 스킬 반영

        IG_MaxHp = OG_Stat.MaxHealth;
        IG_MobDamageRate += OG_Stat.MobDamageRate;
        IG_BossDamageRate += OG_Stat.BossDamageRate;
        IG_InGameExpRate += OG_Stat.InGameExpRate;
        IG_ItemDropRate += OG_Stat.ItemDropRate;
        IG_PowerGaugeSpeed += OG_Stat.PowerGaugeSpeedRate;
        IG_USkillCount += OG_Stat.SpecialAttackCount;
        IG_AccountExpRate += OG_Stat.AccountExpRate;
        IG_Life += OG_Stat.ReviveCount;
        IG_curWeaponLv += OG_Stat.StartWeaponLevel;
        IG_RewardRate += OG_Stat.RewardRate;
        IG_Level += OG_Stat.StartLevel;

        CurHp = IG_MaxHp;
        CurExp = 0;
        CurPow = 0;
    }

    /// <summary>
    /// 플레이어 스텟 데이터에서 해당 id의 플레이어의  초기 스텟을 설정
    /// </summary>
    public void SetOutGameStat(int id)
    {
        List<Ability> abilities = new List<Ability>();
        CharData curPlayerChar = DataManager.character.GetData(id);
        int[] partsCodes = DataManager.account.GetPartsArray();

        foreach (Ability cStat in curPlayerChar.abilityDatas)
        {
            Ability.AddOrUpdateAbility(abilities, cStat);
        }

        foreach (int partsId in partsCodes) //최대 4번 반복
        {
            if(partsId == 0)
            {
                continue;
            }
            PartsData pData = DataManager.parts.GetData(partsId);
            Ability.AddOrUpdateAbility(abilities, pData.mainAbility);
            foreach (Ability pSubStat in pData.subAbilities)
            {
                Ability.AddOrUpdateAbility(abilities, pSubStat);
            }
        }

        UpdatePlayerStats(OG_Stat, abilities);
    }

    /// <summary>
    /// 패시브를 통해 강화된 스텟
    /// </summary>
    public void SetPassiveStat()
    {
        //아웃게임 스텟 + 패시브 스텟
        IG_Dmg = OG_Stat.Damage + (OG_Stat.Damage *( PS_Dmg/100));
        IG_Dfs = OG_Stat.Defense + (OG_Stat.Defense * (PS_Dfs / 100));
        IG_MSpd = OG_Stat.MoveSpeed + (OG_Stat.MoveSpeed * (PS_MSpd / 100));
        IG_ASpd = OG_Stat.AttackSpeed + (OG_Stat.AttackSpeed * (PS_ASpd / 100));
        IG_Regen = OG_Stat.HealthRegen + (OG_Stat.HealthRegen * (PS_HpRegen / 100));
    }

    
    [ContextMenu("레벨업")]
    public void LevelUp()
    {
        levelInstance++;
        MaxExpInstance = IG_MaxExp; //기존 목표exp 복사
        IG_MaxExp = (int)((float)IG_MaxExp * MaxExpUpRate); //목표 exp 증가
        CurExp = 0;

        GameManager.Game.UI.IGetSkill.OpenInterface();
        Time.timeScale = 0f;
    }

    /// <summary>
    /// 플레이어가 데미지를 받을 경우 데미지 적용 및 데미지액션 실행
    /// </summary>
    public void PlayerDamaged(float damage, GameObject attackObj)
    {
        if (!PlayerMain.Instance.isInvincibleState)
        {
            float applyDamage = damage - (int)(damage * IG_Dfs/100);

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
        if (PlayerMain.Instance.isInvincibleState) return;

        Collider2D attackingCollider = attack_obj.GetComponent<Collider2D>();
        if (attackingCollider != null)
        {
            StartCoroutine(ActiveInvincible(DefaultInvincibleTime));
            PlayerKnockBack(attackingCollider);
        }
    }


    //플레이어 넉백
    public void PlayerKnockBack(Collider2D collision)
    {
        Vector2 curPosition = transform.position;
        Vector2 targetPosition = CalculateTargetPos(curPosition, collision);

        // 코루틴이 실행 중이 아니면 실행
        if (!PlayerMain.Instance.isKnockbackRun)
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
        PlayerMain.Instance.isKnockbackRun = true; // 코루틴 실행 상태 설정
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

        PlayerMain.Instance.isKnockbackRun = false; // 코루틴 종료 상태 설정
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
        PlayerMain.Instance.isInvincibleState = true;
        playerSprite.color = new Color(1, 1, 1, 0.5f);
        yield return new WaitForSeconds(invincible_time);
        PlayerMain.Instance.isInvincibleState = false;
        playerSprite.color = new Color(1, 1, 1, 1f);
    }



    private void UpdatePlayerStats(PlayerAbility playerStats, List<Ability> abilities)
    {
        foreach (var ability in abilities)
        {
            switch (ability.key)
            {
                case 1: // 데미지 증가
                    playerStats.Damage += ability.value;
                    break;
                case 2: // 방어도 증가
                    playerStats.Defense += ability.value;
                    break;
                case 3: // 이동속도 증가
                    playerStats.MoveSpeed += ability.value;
                    break;
                case 4: // 공격속도 증가
                    playerStats.AttackSpeed += ability.value;
                    break;
                case 5: // 최대 체력 증가
                    playerStats.MaxHealth += ability.value;
                    break;
                case 101: // 초당 체력 재생
                    playerStats.HealthRegen += ability.value;
                    break;
                case 102: // 잡몹 데미지 증가
                    playerStats.MobDamageRate += ability.value;
                    break;
                case 103: // 보스 데미지 증가
                    playerStats.BossDamageRate += ability.value;
                    break;
                case 104: // 인게임 경험치 획득률 증가
                    playerStats.InGameExpRate += ability.value;
                    break;
                case 105: // 인게임 아이템 확률 증가
                    playerStats.ItemDropRate += ability.value;
                    break;
                case 201: // 파워게이지 속도 증가
                    playerStats.PowerGaugeSpeedRate += ability.value;
                    break;
                case 202: // 특수공격 횟수 증가
                    playerStats.SpecialAttackCount += ability.value;
                    break;
                case 203: // 계정 경험치 획득률 증가
                    playerStats.AccountExpRate += ability.value;
                    break;
                case 204: // 미네랄 획득률 증가
                    playerStats.RewardRate += ability.value;
                    break;
                case 301: // 시작 레벨
                    playerStats.StartLevel += ability.value;
                    break;
                case 401: // 부활
                    playerStats.ReviveCount += 1;
                    break;
                case 402: // 시작 무기레벨
                    playerStats.StartWeaponLevel += ability.value;
                    break;
                default:
                    Debug.LogWarning($"Unknown Ability Id: {ability.key}");
                    break;
            }
        }
    }

    //파워 게이지 상수
    public const float powlv1Max = 5;
    public const float powlv2Max = 15;
    public const float powMax = 30;

    private int PowerLvSet(float curPow)
    {
        if (curPow > powlv1Max && IG_curPowerLevel == 0)
        {
            IG_curPowerLevel =  1;
        }
        else if (curPow > powlv2Max && IG_curPowerLevel == 1)
        {
            IG_curPowerLevel = 2;
        }
        else if (curPow >= powMax && IG_curPowerLevel == 2)
        {
            IG_curPowerLevel = 3;
        }

        return 0;
    }

}
