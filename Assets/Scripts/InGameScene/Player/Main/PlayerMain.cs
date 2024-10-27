using System;
using System.Collections;
using System.Collections.Generic;
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

    

    public void playerInit()
    {
        pStat = GetComponent<PlayerStat>();
        pControl = GetComponent<PlayerControl>();
        pUI = GetComponent<PlayerUI>();
        pSpecial = GetComponent<PlayerSpecialSkill>();
        pSkill = GetComponent<PlayerSkillManager>();
        pShooter = GetComponent<playerShooterUpgrade>();

        pStat.Init();
        pControl.Init();
        pUI.Init();
        pSpecial.Init();
        pSkill.Init();
        pShooter.Init();

        SetTestButtons();
        isPlayerSetDone = true;
    }


    private void Update()
    {
        PlayerMove();
        IncreasePow();

       
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
        if (pStat.curPowerValue <= powMax)
        {
            pStat.curPowerValue += Time.deltaTime * pStat.powerIncreaseRate;
        }

        PowerLvSet();
    }

    private void PowerLvSet()
    {
        float powerIncrease = pStat.curPowerValue;
        if (powerIncrease > powlv1Max && pStat.powerLevel == 0)
        {
            pStat.powerLevel = 1;
        }
        else if (powerIncrease > powlv2Max && pStat.powerLevel == 1)
        {
            pStat.powerLevel = 2;
        }
        else if (powerIncrease >= powMax && pStat.powerLevel == 2)
        {
            pStat.powerLevel = 3;
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

        for(int i =0; i< TestButtons.childCount; i++)
        {
            TestButtons.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
        }

        TestButtons.Find("ShooterUp").GetComponent<Button>().onClick.AddListener(ShooterUPBtn);
        TestButtons.Find("ShooterDown").GetComponent<Button>().onClick.AddListener(ShooterDownBtn);
        TestButtons.Find("NextChar").GetComponent<Button>().onClick.AddListener(NextBtn);
        TestButtons.Find("PrevChar").GetComponent<Button>().onClick.AddListener(PrevBtn);
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
    #endregion
}

