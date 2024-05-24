using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public bool isFirstSetDone;
    private GameManager gameManager;
    private PlayerJsonReader playerData;
    [Header("�⺻ ����")]
    public int curPlayerID;
    public int level;

    [Header("�ʱ⽺�� + �нú� ���� ���� \n �� ��ҿ� ���� ������")]
    public float damage; //�߰����� ���� ���� ���� Ȥ�� �̵��� ���� ���� ����(�нú� ��ų�� �߰��� ����)
    public float defence;
    public float moveSpeed;
    public float attackSpeed;
    public float hp;

    public float cur_hp;

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

    [Header("����")]
    public bool isShootable;
    public bool isHitted;

    private PlayerControl p_control;

    private void Awake()
    {
        p_control = GameObject.Find("Player").GetComponent<PlayerControl>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerData = GameObject.Find("DataManager").GetComponent<PlayerJsonReader>();
        isFirstSetDone = false;
        isShootable = false;
        isHitted = false;
        curPlayerID =1;
        SetStat(curPlayerID);
    }
    private void Update()
    {
        if (gameManager.isBattleStart)
        {
            isShootable = gameManager.isBattleStart;
        }
    }
    public void SetStat(int cur_id)
    {
        PlayerSet(cur_id);

        hp = initHp;
        cur_hp = hp;

        damageIncreaseRate = 1;
        defenceIncreaseRate = 1;
        moveSpeedIncreaseRate = 1;
        attackSpeedIncreaseRate = 1;
        hpRegenRate = 0;

        ApplyStat();
    }

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

    public void ApplyStat()
    {
        damage = initDamage * damageIncreaseRate;
        defence = initDefence * defenceIncreaseRate;
        moveSpeed = initMoveSpeed * moveSpeedIncreaseRate;
        attackSpeed = initAttackSpeed * attackSpeedIncreaseRate;
    }

    //�÷��̾��� �������� ��ȭ
    public void PlayerDamaged(float damage, GameObject attackObj)
    {
        if (!p_control.isInvincible)
        {
            isHitted = true;
            float applyDamage = damage * (1 - (defence / 100));
            cur_hp -= applyDamage;
            Debug.Log(attackObj.name + " �� ���� " + applyDamage + " �� �������� ����");
            p_control.PlayerAttacked(attackObj); //�˹� �� ���� �ο�
        }
    }
}