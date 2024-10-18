using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{ 
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

        Init();
    }

    private void Init()
    {
        isFirstSetDone = false; 
        int savedPlayerId = DataManager.account.GetChar();
        Debug.Log(savedPlayerId);
        curPlayerID = savedPlayerId + 100;
        SetStat(curPlayerID);
    }

    public void SetStat(int playerId)
    {
        PlayerSet(playerId);

        maxHp = initHp;
        curHp = maxHp;

        //�� ������ ������ : �нú� ��ų�̳� ���������� ������ ����
        damageIncreaseRate = 1;
        defenceIncreaseRate = 1;
        moveSpeedIncreaseRate = 1;
        attackSpeedIncreaseRate = 1;
        hpRegenRate = 0;

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