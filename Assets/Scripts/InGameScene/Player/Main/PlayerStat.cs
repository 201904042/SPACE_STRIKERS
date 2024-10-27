using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour
{
    public const int basicStat = 10; //����1�� ���� �⺻ ����
    public GameObject selectSkillUI;

    private PlayerUI pUI => PlayerMain.pUI;
    private PlayerControl pControl => PlayerMain.pControl;

    [Header("�⺻ ����")]
    public int curPlayerID;
    public bool isFirstSetDone;

    [Header("���� ����. �ٸ� ��ũ��Ʈ�� �����Ǵ� ����")]
    //InGame
    public float IG_Dmg; //�߰����� ���� ���� ���� Ȥ�� �̵��� ���� ���� ����(�нú� ��ų�� �߰��� ����)
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
    public int IG_WeaponLv; //���� ���� ����
    public int IG_RewardRate; //�ƿ����ӿ����� ���� ������(���� �ɷ� ��) => �̳׶� , ��� ���� ���� ����

    //���� ����
    public int IG_Level; //ĳ������ ������ �ƴ� �ΰ��ӿ����� ����
    public int IG_NextExp; //��ǥ exp. IG_CurExp >= IG_NextExp ��� ������ ����
    private int IG_CurExp; //���� ������ �ִ� exp
    public int CurExp
    {
        get => IG_CurExp;
        set
        {
            IG_CurExp += value;
            if (IG_CurExp >= IG_NextExp)
            {
                LevelUP();
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
        }
    }
    public int IG_curPowerLevel; //����� ����ġ
    public float IG_PowIncreaseRate; //�ʴ� �Ŀ� ������
    private float curPower; //������� ���� power�� ��
    public float AddPower
    {
        get => curPower;
        set
        {
            curPower = value;
        }
    }

    //�����ð�
    public float invincibleTime = 3f;

    [Header("�ʱ⽺��(�ƿ����ӿ��� �޾ƿ� ����)")]
    //OutGame
    private float OG_Dmg; //�߰������� ������� ���� �⺻�� ���� ����(��ü ���� �߰����� + ���� ������ �����)
    private float OG_Dfs;
    private float OG_MSpd;
    private float OG_ASpd;
    private float OG_Hp;
    private float OG_HpRegen;
    public int OS_UDmgUp; //�ƿ����ӿ��� �޾ƿ� Ư����ų ������
    


    [Header("�ΰ��� ������(��ų ��)")]
    // PassiveSkill
    public float PS_Dmg;
    public float PS_Dfs;
    public float PS_MSpd;
    public float PS_ASpd;
    public float PS_HpRegen;

    // ExtraSkill
    public int ES_RewardUp; //�ΰ��ӿ����� ���� ������ => other��ų�� ���� ����


    [Header("���� ����")]
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

        PlayerSkillManager ps = PlayerMain.pSkill;
        //ps.AddPassiveSkill((InGamePassiveSkill)ps.FindSkillByCode(641));

        IG_WeaponLv = 1;

        IG_Level = 1;
        IG_NextExp = 5;
        IG_CurExp = 0;

        IG_USkillCount = 3;
        IG_curPowerLevel = 0;
        curPower = 0;
        IG_PowIncreaseRate = 1f;
        
        OS_UDmgUp = 100; //������ 100 = �⺻ ���� * 1

    }

    public void ChangePlayer(int id)
    {
        curPlayerID = id;
        SetStat(curPlayerID);
    }

    public void SetStat(int playerId)
    {
        Debug.Log("�÷��̾� ���� ����");
        PlayerSet(playerId);
        ApplyStat();
        CurHp = IG_Hp;
    }

    /// <summary>
    /// �÷��̾� ���� �����Ϳ��� �ش� id�� �÷��̾���  �ʱ� ������ ����
    /// </summary>
    public void PlayerSet(int id)
    {
        CharData curPlayerChar = DataManager.character.GetData(id);

        ////�ƿ����ӿ��� �޾ƿ� ĳ������ ���� todo -> �̺κ��� Ư�� �����ͺ��̽��� ���� �޾ƿÿ���
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
    /// �нú긦 ���� ��ȭ�� ����
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

    /// <summary>
    /// �÷��̾ �������� ���� ��� ������ ���� �� �������׼� ����
    /// </summary>
    public void PlayerDamaged(float damage, GameObject attackObj)
    {
        if (!InvincibleState)
        {
            float applyDamage = damage * (1 - (0.01f * IG_Dfs)); //todo => ���� ���� �ٽ� üũ

            CurHp -= applyDamage;
            Debug.Log(attackObj.name + " �� ���� " + applyDamage + " �� �������� ����");
            PlayerDamagedAction(attackObj); //�˹� �� ���� �ο�
        }
    }

    /// <summary>
    /// �������� ���� �� �ڷ� �и�
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

    
    //�÷��̾� �˹�
    public void PlayerKnockBack(Collider2D collision)
    {
        Vector2 curPosition = transform.position;
        Vector2 targetPosition = CalculateTargetPos(curPosition, collision);

        // �ڷ�ƾ�� ���� ���� �ƴϸ� ����
        if (!isKnockbackRun)
        {
            StartCoroutine(PushbackLerp(curPosition, targetPosition));
        }
    }
    #region �˹� ���
    private Vector2 CalculateTargetPos(Vector3 curpos, Collider2D col)
    {
        Vector2 pushDirection = (curpos - col.transform.position).normalized;
        return curpos - new Vector3(-pushDirection.x, -pushDirection.y, 0) * 2;
    }

    private IEnumerator PushbackLerp(Vector3 startPos, Vector3 endPos)
    {
        isKnockbackRun = true; // �ڷ�ƾ ���� ���� ����
        float startTime = Time.time;
        float elapseTime = 0f;

        while (elapseTime < 0.1f)
        {
            // �浹 ���¸� Ȯ��
            if (CheckCollide())
            {
                break;
            }

            float t = Mathf.Clamp01(elapseTime / 0.1f);
            transform.position = Vector3.Lerp(startPos, endPos, t);
            elapseTime = Time.time - startTime;

            yield return null;
        }

        isKnockbackRun = false; // �ڷ�ƾ ���� ���� ����
    }

    private bool CheckCollide()
    {
        PlayerControl pControl = PlayerMain.pControl;

        return (pControl.isTopCollide || pControl.isBottomCollide || pControl.isLeftCollide || pControl.isRightCollide);
    }
    #endregion

    //���� ���� �ο� �ڷ�ƾ
    private IEnumerator ActiveInvincible(float invincible_time)
    {
        SpriteRenderer playerSprite = PlayerMain.Instance.GetComponent<SpriteRenderer>();
        if (playerSprite == null)
        {
            Debug.LogError("�÷��̾��� ��������Ʈ �������� ã�� �� ����");
        }
        InvincibleState = true;
        playerSprite.color = new Color(1, 1, 1, 0.5f);
        yield return new WaitForSeconds(invincible_time);
        InvincibleState = false;
        playerSprite.color = new Color(1, 1, 1, 1f);
    }

    public void LevelUP()
    {
        IG_Level++;
        IG_NextExp += 5; //�ӽ� ó��. ���� ��Ȯ�� ���� ����

        selectSkillUI.SetActive(true);

        Time.timeScale = 0f;
    }

    
}