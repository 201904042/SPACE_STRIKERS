using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    private PlayerJsonReader playerData;

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
        playerData = GameObject.Find("DataManager").GetComponent<PlayerJsonReader>();
        Init();
    }

    private void Init()
    {
        isFirstSetDone = false;
        
        curPlayerID = 1;
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
        foreach (var player in playerData.PlayerList.player)
        {
            if (player.id == id)
            {
                level = player.level;
                initDamage = player.damage;
                initDefence = player.defence;
                initMoveSpeed = player.move_speed;
                initAttackSpeed = player.attack_speed;
                initHp = player.hp;
            }
        }
    }

    /// <summary>
    /// �ʱ⽺�ݿ� �������� ���Ͽ� �ٸ� Ŭ������ ������ ���� ����.
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