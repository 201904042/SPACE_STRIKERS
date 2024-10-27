using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMain : MonoBehaviour //�÷��̾��� ���� ��ũ��Ʈ
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


    //���� �������� �����Ұ�
    public const float bulletBaseInterval = 1f;  // �� �⺻ �߻� �ֱ�
    public const float MissileBaseInterval = 2f; // �Ѿ��� 2�ʿ� �ѹ�. �̻����� 3�ʿ� �ѹ�. ȣ���� 2�ʿ� �ѹ�
    public const float HomingBaseInterval = 0.75f;
    public const float TroopBaseInterval = 1f;
    public const float ShieldBaseInterval = 5; //������ ����� �ð�

    public const int bulletBaseDamageRate = 100;  // �� �⺻ ������ ������
    public const int MissileBaseDamageRate = 150; // ���� ������  = �÷��̾� ���ݷ� + (�ð� * ������)
    public const int ExplosionBaseDamageRate = 80;
    public const int HomingBaseDamageRate = 30;
    public const int TroopBaseDamageRate = 80; // 80 => 100 => 120
    public const float ShieldDamageRate = 300; //������ ����� �ð�

    public const int bulletBaseSpeed = 10;
    public const int MissileBaseSpeed = 5;
    public const int HomingBaseSpeed = 15;
    public const int TroopBaseSpeed = 10;
    

    public const float ExplosionBaseLiveTime = 1; // 1�� �����ϵ�
    public const float ExplosionBaseRange = 1; //-> ũ�� 1 -> 1.5 -> 2



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



    #region ����� ��ư
    private Transform TestButtons;

    public void SetTestButtons()
    {
        TestButtons = GameObject.Find("TestButtons").transform;
        if(TestButtons == null) 
        {
            Debug.LogError("�׽�Ʈ��ư�׷��� ã�� ����");
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

