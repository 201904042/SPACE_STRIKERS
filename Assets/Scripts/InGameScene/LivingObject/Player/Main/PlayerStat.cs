using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAbility
{
    public float Damage { get; set; }
    public float Defense { get; set; }
    public float MoveSpeed { get; set; }
    public float AttackSpeed { get; set; }
    public float MaxHealth { get; set; }
    public float HealthRegen { get; set; }

    public float MobDamageRate { get; set; }
    public float BossDamageRate { get; set; }
    public float InGameExpRate { get; set; }
    public float ItemDropRate { get; set; }
    public float PowerGaugeSpeedRate { get; set; }
    public float AccountExpRate { get; set; }
    public float RewardRate { get; set; }
    public int SpecialAttackCount { get; set; }
    public int StartLevel { get; set; }
    public int ReviveCount { get; set; }
    public int StartWeaponLevel { get; set; }
}

public class PlayerStat : MonoBehaviour
{
    public const int DefaultRate = 100;
    public const int basicStat = 10; //레벨1의 가장 기본 스텟
    public const float MaxExpUpRate = 1.5f; //레벨업마다 증가하는 목표exp의 비율
    public const float invincibleTime = 3f; //무적시간
    private PlayerUI pUI => PlayerMain.pUI;
    private PlayerControl pControl => PlayerMain.pControl;

    [Header("기본 정보")]
    public int curPlayerID;

    //아웃게임에서 받아온 스텟
    private PlayerAbility OG_Stat = new PlayerAbility();


    //인게임에서 받은 스텟

    //필수
    public float IG_Dmg { get; private set; }
    public float IG_Dfs { get; private set; }
    public float IG_MoveSpeed { get; private set; }
    public float IG_AttackSpeed { get; private set; }
    public float IG_HealthRegen { get; private set; }
    public float IG_MaxHealth { get; private set; }
    

    //선택
    public float IG_MobDamageRate { get; private set; }
    public float IG_BossDamageRate { get; private set; }
    public float IG_InGameExpRate { get; private set; }
    public float IG_ItemDropRate { get; private set; }
    public float IG_PowerGaugeSpeed { get; private set; }
    public int IG_SpecialAttackCount { get; private set; }
    public float IG_AccountExpRate { get; private set; }
    public int IG_ReviveCount { get; private set; }

    public float IG_RewardRate;
    private float IG_CurHp;
    private float IG_PowValue; //지금까지 모인 파워값

    public int IG_curWeaponLv;
    public int IG_Level;
    public int levelInstance;
    private int IG_CurExp;
    public int IG_MaxExp;
    private int IG_USkillCount;
    public int IG_curPowerLevel => PowerLvSet(IG_PowValue); //파워값에 비례한 파워 레벨
    public int MaxExpInstance; //반환시 이전 maxExp를 설정하기 위함

    //인게임 스텟 옵저버
    public float CurHp
    {
        get => IG_CurHp;
        set
        {
            IG_CurHp = Mathf.Min(value, IG_MaxHealth);
            pUI.HpBarChange();
        }
    }
    public int CurExp
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
    public int USkillCount
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
            pUI.PowBarChange();
        }
    }

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
        InitInGameStat();
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
        InitInGameStat();
        SetStat(curPlayerID);
    }

    private void InitInGameStat()
    {
        IG_Dmg = 0;
        IG_Dfs = 0;
        IG_MoveSpeed = 0;
        IG_AttackSpeed = 0;
        IG_HealthRegen = 0;
        IG_MaxHealth = 0;


        //선택
        IG_MobDamageRate = DefaultRate;
        IG_BossDamageRate = DefaultRate;
        IG_InGameExpRate = DefaultRate;
        IG_ItemDropRate = 30;
        IG_PowerGaugeSpeed = 0;
        IG_SpecialAttackCount = 0;
        IG_AccountExpRate = DefaultRate;
        IG_ReviveCount = 0;
        IG_curWeaponLv = 1;
        IG_RewardRate = DefaultRate;
        IG_Level = 1;
        levelInstance = 1;
        IG_CurHp = 0;
        IG_CurExp = 0;
        IG_MaxExp = 5;
        IG_USkillCount = 3;
        IG_PowValue = 0;
    }

    public void SetStat(int playerId)
    {
        SetOutGameStat(playerId);
        SetPassiveStat();

        IG_MaxHealth = OG_Stat.MaxHealth;
        CurHp = IG_MaxHealth;

        IG_MobDamageRate += OG_Stat.MobDamageRate;
        IG_BossDamageRate += OG_Stat.BossDamageRate;
        IG_InGameExpRate += OG_Stat.InGameExpRate;
        IG_ItemDropRate += OG_Stat.ItemDropRate;
        IG_PowerGaugeSpeed += OG_Stat.PowerGaugeSpeedRate;
        IG_SpecialAttackCount += OG_Stat.SpecialAttackCount;
        IG_AccountExpRate += OG_Stat.AccountExpRate;
        IG_ReviveCount += OG_Stat.ReviveCount;
        IG_curWeaponLv += OG_Stat.StartWeaponLevel;
        IG_RewardRate += OG_Stat.RewardRate;
        IG_USkillCount += OG_Stat.SpecialAttackCount;
        IG_Level += OG_Stat.StartLevel;
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
            PartsAbilityData pData = DataManager.parts.GetData(partsId);
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
        IG_MoveSpeed = OG_Stat.MoveSpeed + (OG_Stat.MoveSpeed * (PS_MSpd / 100));
        IG_AttackSpeed = OG_Stat.AttackSpeed + (OG_Stat.AttackSpeed * (PS_ASpd / 100));
        IG_HealthRegen = OG_Stat.HealthRegen + (OG_Stat.HealthRegen * (PS_HpRegen / 100));
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
        if (PlayerMain.Instance.isInvincibleState) return;

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
                    playerStats.SpecialAttackCount += (int)ability.value;
                    break;
                case 203: // 계정 경험치 획득률 증가
                    playerStats.AccountExpRate += ability.value;
                    break;
                case 204: // 미네랄 획득률 증가
                    playerStats.RewardRate += ability.value;
                    break;
                case 301: // 시작 레벨
                    playerStats.StartLevel += (int)ability.value;
                    break;
                case 401: // 부활
                    playerStats.ReviveCount += 1;
                    break;
                case 402: // 시작 무기레벨
                    playerStats.StartWeaponLevel += (int)ability.value;
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
            return 1;
        }
        else if (curPow > powlv2Max && IG_curPowerLevel == 1)
        {
            return 2;
        }
        else if (curPow >= powMax && IG_curPowerLevel == 2)
        {
            return 3;
        }

        return 0;
    }

}
