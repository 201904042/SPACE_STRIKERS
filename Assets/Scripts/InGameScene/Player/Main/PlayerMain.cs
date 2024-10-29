using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMain : MonoBehaviour //플레이어의 메인 스크립트
{
    public const float powlv1Max = 5;
    public const float powlv2Max = 15;
    public const float powMax = 30;

    public static PlayerMain Instance;

    public static PlayerStat pStat;
    public static PlayerControl pControl;
    public static PlayerUI pUI;
    public static playerShooterUpgrade pShooter;
    public static PlayerSpecialSkill pSpecial;
    public static PlayerSkillManager pSkill;


    //무기 레벨별로 정리할것
    public const float bulletBaseInterval = 1f;  // 각 기본 발사 주기
    public const float MissileBaseInterval = 2f; // 총알은 2초에 한번. 미사일은 3초에 한번. 호밍은 2초에 한번
    public const float HomingBaseInterval = 0.75f;
    public const float TroopBaseInterval = 1f;
    public const float ShieldBaseInterval = 5; //쉴드의 재생성 시간

    public const int bulletBaseDamageRate = 100;  // 각 기본 데미지 증가율
    public const int MissileBaseDamageRate = 150; // 최종 데미지  = 플레이어 공격력 + (플공 * 증가율)
    public const int ExplosionBaseDamageRate = 80;
    public const int HomingBaseDamageRate = 30;
    public const int TroopBaseDamageRate = 80; // 80 => 100 => 120
    public const float ShieldDamageRate = 300; //쉴드의 재생성 시간

    public const int bulletBaseSpeed = 10;
    public const int MissileBaseSpeed = 5;
    public const int HomingBaseSpeed = 15;
    public const int TroopBaseSpeed = 10;
    

    public const float ExplosionBaseLiveTime = 1; // 1초 고정일듯
    public const float ExplosionBaseRange = 1; //-> 크기 1 -> 1.5 -> 2



    public bool isPlayerSetDone;

    private void Awake()
    {
        isPlayerSetDone = false;
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }

        playerInit();
    }

    private void OnDisable()
    {
        
    }

    public void playerInit()
    {
        pStat = GetComponent<PlayerStat>();
        pControl = GetComponent<PlayerControl>();
        pUI = GetComponent<PlayerUI>();
        pSpecial = GetComponent<PlayerSpecialSkill>();
        pSkill = GetComponent<PlayerSkillManager>();
        pShooter = GetComponent<playerShooterUpgrade>();

        pUI.ComponentSet();
        
        pStat.Init(); //최우선 스크립트
        pUI.Init();

        pControl.Init();
        pControl.PlayerInputOn();

        pSpecial.Init();
        pSkill.Init();
        pShooter.Init();
        

        SetTestButtons();
        isPlayerSetDone = true;
        PlayerSkillManager ps = PlayerMain.pSkill;
        ps.AddActiveSkill((ActiveSkill)ps.FindSkillByCode(605));
        ps.AddActiveSkill((ActiveSkill)ps.FindSkillByCode(605));
        ps.AddActiveSkill((ActiveSkill)ps.FindSkillByCode(605));
        ps.AddActiveSkill((ActiveSkill)ps.FindSkillByCode(605));
        ps.AddActiveSkill((ActiveSkill)ps.FindSkillByCode(605));
        ps.AddActiveSkill((ActiveSkill)ps.FindSkillByCode(605));
        ps.AddActiveSkill((ActiveSkill)ps.FindSkillByCode(605));
    }


    private void Update()
    {
        PlayerMove();
        IncreasePow();
        RestoreHp();

    }

    private void RestoreHp()
    {
        if(pStat.PS_HpRegen <= 0)
        {
            return;
        }

        pStat.CurHp += (pStat.IG_HpRegen * Time.deltaTime); //초당 pStat.IG_HpRegen%만큼 회복 3이면 초당 3회복
    }

    private void PlayerMove()
    {
        if (isPlayerSetDone || pStat.CanMove)
        {
            pControl.PlayerMove();
        }
        pControl.KeepPlayerInViewport();
    }

    private void IncreasePow()
    {
        if (pStat.AddPower < powMax)
        {
            float addValue = Time.deltaTime * pStat.IG_PowIncreaseRate;
            pStat.AddPower = Mathf.Min(pStat.AddPower + addValue, powMax); // 초당 지정된 속도로 게이지 누적
        }
        PowerLvSet();
        pUI.PowBarChange();
    }

    private void PowerLvSet()
    {
        float curPow = pStat.AddPower;
        if (curPow > powlv1Max && pStat.IG_curPowerLevel == 0)
        {
            pStat.IG_curPowerLevel = 1;
        }
        else if (curPow > powlv2Max && pStat.IG_curPowerLevel == 1)
        {
            pStat.IG_curPowerLevel = 2;
        }
        else if (curPow >= powMax && pStat.IG_curPowerLevel == 2)
        {
            pStat.IG_curPowerLevel = 3;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Border"))
        {
            switch (collision.name)
            {
                case "Top":
                    pControl.isTopCollide = true; break;
                case "Bottom":
                    pControl.isBottomCollide = true; break;
                case "Right":
                    pControl.isRightCollide = true; break;
                case "Left":
                    pControl.isLeftCollide = false; break;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (pStat.InvincibleState)
        {
            return;
        }

        if (collision.CompareTag("Enemy"))
        {
            pStat.PlayerDamaged(collision.GetComponent<EnemyObject>().enemyStat.damage / 2, collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Border"))
        {
            switch (collision.name)
            {
                case "Top":
                    pControl.isTopCollide = false; break;
                case "Bottom":
                    pControl.isBottomCollide = false; break;
                case "Right":
                    pControl.isRightCollide = false; break;
                case "Left":
                    pControl.isLeftCollide = false; break;
            }
        }
    }
    



    #region 디버깅 버튼
    private Transform TestButtons;

    public void SetTestButtons()
    {
        TestButtons = GameObject.Find("TestButtons").transform;
        if(TestButtons == null) 
        {
            Debug.LogError("테스트버튼그룹을 찾지 못함");
        }

        Button[] btns = TestButtons.GetComponentsInChildren<Button>();

        for(int i =0; i< btns.Length; i++)
        {
            btns[i].onClick.RemoveAllListeners();
        }

        TestButtons.Find("ShooterUp").GetComponent<Button>().onClick.AddListener(ShooterUPBtn);
        TestButtons.Find("ShooterDown").GetComponent<Button>().onClick.AddListener(ShooterDownBtn);
        TestButtons.Find("NextChar").GetComponent<Button>().onClick.AddListener(NextBtn);
        TestButtons.Find("PrevChar").GetComponent<Button>().onClick.AddListener(PrevBtn);
        Transform UniqueBtn = TestButtons.Find("UniqueBtn");
        for(int i = 0; i < UniqueBtn.childCount; i++)
        {
            int index = i+1; //1~3
            UniqueBtn.GetChild(i).GetComponent<Button>().onClick.AddListener(() => USkillBtn(index));
        }

    }

    public void ShooterUPBtn()
    {
        if (pShooter.shooterLevel < 3)
        {
            pShooter.shooterLevel += 1;
        }
    }

    public void ShooterDownBtn()
    {
        if (pShooter.shooterLevel > 1)
        {
            pShooter.shooterLevel -= 1;
        }

    }

    public void NextBtn()
    {
        if (pShooter.charId < 104)
        {
            pShooter.charId += 1;
        }
        else
        {
            pShooter.charId = 101;
        }
    }
    public void PrevBtn()
    {
        if (pShooter.charId > 101)
        {
            pShooter.charId -= 1;
        }
        else
        {
            pShooter.charId = 104;
        }
    }


    public void USkillBtn(int i)
    {
        pSpecial.SpecialFire(pStat.curPlayerID, i);
    }

    #endregion
}

