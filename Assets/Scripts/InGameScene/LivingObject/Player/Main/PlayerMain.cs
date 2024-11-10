using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMain : MonoBehaviour //�÷��̾��� ���� ��ũ��Ʈ
{
    

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


    public static PlayerMain Instance;

    public static PlayerStat pStat;
    public static PlayerControl pControl;
    public static PlayerUI pUI;

    public static PlayerShooter pShooter;
    public static PlayerUSkill pUSkill;
    public static PlayerSkill pSkill;


    
    [Header("���� ����")]
    public bool isControllable; //�÷��̾� ���۰���?
    public bool isPlayerSetDone; //
    public bool isOnMove => GameManager.Game.BattleSwitch; //�÷��̾��� �̵� ����
    public bool isOnAttack => GameManager.Game.BattleSwitch; //�÷��̾��� ���� ����
    public bool isInvincibleState; // 
    public bool isKnockbackRun;

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
        pUSkill = GetComponent<PlayerUSkill>();
        pSkill = GetComponent<PlayerSkill>();
        pShooter = GetComponent<PlayerShooter>();

        pStat.Init(); //�ֿ켱 ��ũ��Ʈ
        pUI.Init();

        pControl.Init();
        pControl.PlayerInputOn();

        pUSkill.Init();
        pSkill.Init();
        pShooter.Init();
        

        SetTestButtons();

        isControllable = true;
        isPlayerSetDone = true;
        //PlayerSkill ps = PlayerMain.pSkill;
        //ps.AddSkill((ActiveSkill)ps.FindSkillByCode(605));
        //ps.AddSkill((ActiveSkill)ps.FindSkillByCode(605));
        //ps.AddSkill((ActiveSkill)ps.FindSkillByCode(605));
        //ps.AddSkill((ActiveSkill)ps.FindSkillByCode(605));
        //ps.AddSkill((ActiveSkill)ps.FindSkillByCode(605));
        //ps.AddSkill((ActiveSkill)ps.FindSkillByCode(605));
        //ps.AddSkill((ActiveSkill)ps.FindSkillByCode(605));
    } 

    private void Update()
    {
        PlayerMove();
        CheckPlayerLevel();
        IncreasePow();
        HpRegeneration();
    }

    private void CheckPlayerLevel()
    {
        if(pStat.IG_Level > pStat.levelInstance)
        {
            pStat.LevelUp();
        }
    }

    public void SetCharSprite(int id)
    {
        SpriteRenderer playerSprite = GetComponent<SpriteRenderer>();
        Sprite charSprite = Resources.Load<Sprite>(DataManager.master.GetData(id).spritePath);
        if (charSprite == null)
        {
            Debug.Log("ĳ���� ��������Ʈ : �߸��� ��� �Ҵ�");
        }

        playerSprite.sprite = charSprite;
    }

    private void HpRegeneration()
    {
        if(pStat.PS_HpRegen <= 0)
        {
            return;
        }
        HpRestore(pStat.IG_HealthRegen * Time.deltaTime);
    }

    public void HpRestore(float healAmount)
    {
        pStat.CurHp = pStat.CurHp + healAmount;
    }

    private void PlayerMove()
    {
        if (isPlayerSetDone || isOnMove)
        {
            pControl.PlayerMove();
        }
        pControl.KeepPlayerInViewport();
    }

    public IEnumerator PlayerDeadAnim()
    {
        //���� �ִϸ��̼� �۵�
        yield return null;
    }

    public IEnumerator PlayerClearAnim()
    {
        //������ ����
        yield return null;
    }

    private void IncreasePow()
    {
        if (pStat.CurPow < PlayerStat.powMax)
        {
            float addValue = Time.deltaTime * pStat.IG_PowerGaugeSpeed;
            pStat.CurPow = Mathf.Min(pStat.CurPow + addValue, PlayerStat.powMax); // �ʴ� ������ �ӵ��� ������ ����
        }

        pUI.PowBarChange();
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
        if (isInvincibleState)
        {
            return;
        }

        if (collision.CompareTag("Enemy"))
        {
            pStat.PlayerDamaged(collision.GetComponent<EnemyObject>().GetCollisionDamage() / 2, collision.gameObject);
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
    



    #region ����� ��ư
    private Transform TestButtons;

    public void SetTestButtons()
    {
        TestButtons = GameObject.Find("TestButtons").transform;
        if(TestButtons == null) 
        {
            Debug.LogError("�׽�Ʈ��ư�׷��� ã�� ����");
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
        pUSkill.SpecialFire(pStat.curPlayerID, i);
    }

    #endregion
}

