using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public const int basicStat = 10; //����1�� ���� �⺻ ����

    [Header("�⺻ ����")]
    public int curPlayerID;
    public int level;
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

    private PlayerControl playerController;

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

        PlayerSkillManager ps = GameManager.Instance.psManager;

        ps.AddActiveSkill((NewActiveSkill)ps.FindSkillByCode(607));
        ps.AddActiveSkill((NewActiveSkill)ps.FindSkillByCode(607));
        ps.AddActiveSkill((NewActiveSkill)ps.FindSkillByCode(607));
        ps.AddActiveSkill((NewActiveSkill)ps.FindSkillByCode(607));
        ps.AddActiveSkill((NewActiveSkill)ps.FindSkillByCode(607));
        ps.AddActiveSkill((NewActiveSkill)ps.FindSkillByCode(607));
        ps.AddActiveSkill((NewActiveSkill)ps.FindSkillByCode(607));
        //ps.AddPassiveSkill((InGamePassiveSkill)ps.FindSkillByCode(641));

        Debug.Log("�÷��̾� ���� �ʱ�ȭ �Ϸ�");
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
    /// �÷��̾ �������� ���� ���
    /// </summary>
    public void PlayerDamaged(float damage, GameObject attackObj)
    {
        if (!playerController.isInvincibleState)
        {
            playerController.isHitted = true;
            float applyDamage = damage * (1 - (0.01f * defence));

            curHp -= applyDamage;
            Debug.Log(attackObj.name + " �� ���� " + applyDamage + " �� �������� ����");
            playerController.PlayerDamagedAction(attackObj); //�˹� �� ���� �ο�
        }
    }
}