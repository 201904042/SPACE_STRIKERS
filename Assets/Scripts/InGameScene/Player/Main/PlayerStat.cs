using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour
{
    public const int basicStat = 10; //����1�� ���� �⺻ ����
    public GameObject selectSkillUI;// todo => ���߿� �������̽� ����

    [Header("�⺻ ����")]
    public int curPlayerID;
    public bool isFirstSetDone;

    [Header("�ʱ⽺�� + �нú� ���� ���� \n �� ��ҿ� ���� ������")]
    public float damage; //�߰����� ���� ���� ���� Ȥ�� �̵��� ���� ���� ����(�нú� ��ų�� �߰��� ����)
    public float defence;
    public float moveSpeed;
    public float attackSpeed;
    public float maxHp;
    public float curHp;

    [Header("�ʱ⽺��(�⺻+��������+��ü��������)")]
    private float initDamage; //�߰������� ������� ���� �⺻�� ���� ����(��ü ���� �߰����� + ���� ������ �����)
    private float initDefence;
    private float initMoveSpeed;
    private float initAttackSpeed;
    private float initHp;

    [Header("�нú� ������")]
    public float damageIncreaseRate;
    public float defenceIncreaseRate;
    public float moveSpeedIncreaseRate;
    public float attackSpeedIncreaseRate;
    public float hpRegenRate;

    private PlayerControl pControl => PlayerMain.pControl;

    public int weaponLevel;
    public int rewardRate; //�ƿ����ӿ����� ���� ������(���� �ɷ� ��) => �̳׶� , ��� ���� ���� ����
    public int rewardRateIncrease; //�ΰ��ӿ����� ���� ������ => other��ų�� ���� ����

    //���� ����
    public int level; //ĳ������ ������ �ƴ� �ΰ��ӿ����� ����
    public int nextExp; //��ǥ exp. curExp >= nextExp ��� ������ ����
    public int curExp; //���� ������ �ִ� exp
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
    public int powerLevel; //����� ����ġ
    public float powerIncreaseRate; //�ʴ� �Ŀ� ������
    public float curPowerValue;
    

    public float specialDamageRate; //����� ��ų�� ������ ������. ������ �����Ƽ�� ���� ����


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

        weaponLevel = 1;

        level = 1;
        nextExp = 5;
        curExp = 0;

        specialCount = 3;
        powerLevel = 0;
        powerIncreaseRate = 1f;
        curPowerValue = 0;
        specialDamageRate = 1f; //������

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
        maxHp = initHp;
        curHp = maxHp;

        ApplyStat();
    }

    /// <summary>
    /// �÷��̾� ���� �����Ϳ��� �ش� id�� �÷��̾���  �ʱ� ������ ����
    /// </summary>
    public void PlayerSet(int id)
    {
        CharData curPlayerChar = DataManager.character.GetData(id);

        ////�ƿ����ӿ��� �޾ƿ� ĳ������ ���� todo -> �̺κ��� Ư�� �����ͺ��̽��� ���� �޾ƿÿ���
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
    /// �нú긦 ���� ��ȭ�� ����
    /// </summary>
    public void ApplyStat()
    {
        damage = initDamage * damageIncreaseRate;
        defence = initDefence * defenceIncreaseRate;
        moveSpeed = initMoveSpeed * moveSpeedIncreaseRate;
        attackSpeed = initAttackSpeed * attackSpeedIncreaseRate;
    }

    /// <summary>
    /// �÷��̾ �������� ���� ��� ������ ���� �� �������׼� ����
    /// </summary>
    public void PlayerDamaged(float damage, GameObject attackObj)
    {
        if (!InvincibleState)
        {
            float applyDamage = damage * (1 - (0.01f * defence)); //todo => ���� ���� �ٽ� üũ

            curHp -= applyDamage;
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
        Image playerSprite = GetComponent<Image>();
        InvincibleState = true;
        playerSprite.color = new Color(1, 1, 1, 0.5f);
        yield return new WaitForSeconds(invincible_time);
        InvincibleState = false;
        playerSprite.color = new Color(1, 1, 1, 1f);
    }

    public void LevelUP()
    {
        level++;
        nextExp += 5; //�ӽ� ó��. ���� ��Ȯ�� ���� ����

        selectSkillUI.SetActive(true);

        Time.timeScale = 0f;
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