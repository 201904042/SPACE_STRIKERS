using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

class player_character
{
    public int level;
    public float damage;
    public float defence;
    public float move_speed;
    public float attack_speed;
    public float hp;
}

public class PlayerStat : PlayerjsonReader
{
    public bool is_firstSet_done;
    public GameManager gameManager;
    [Header("�⺻ ����")]
    public int cur_playerID;
    public int level;

    [Header("�ʱ⽺�� + �нú� ���� ���� \n �� ��ҿ� ���� ������")]
    public float damage; //�߰����� ���� ���� ���� Ȥ�� �̵��� ���� ���� ����(�нú� ��ų�� �߰��� ����)
    public float defence;
    public float move_speed;
    public float attack_speed;
    public float hp;

    public float cur_hp;

    [Header("�ʱ⽺��(�⺻+��������+��ü��������)")]
    public float init_damage; //�߰������� ������� ���� �⺻�� ���� ����(��ü ���� �߰����� + ���� ������ �����)
    public float init_defence;
    public float init_moveSpeed;
    public float init_attackSpeed;
    public float init_hp;

    [Header("�нú� ������")]
    public float damage_increaseRate;
    public float defence_increaseRate;
    public float move_speed_increaseRate;
    public float attack_speed_increaseRate;
    public float hp_RegenerateRate;

    [Header("����")]
    public bool is_shootable;
    public bool is_hitted;

    PlayerControl p_control;
    player_character player1 = new player_character();
    player_character player2 = new player_character();
    player_character player3 = new player_character();
    player_character player4 = new player_character();

    private void Awake()
    {
        p_control = GameObject.Find("Player").GetComponent<PlayerControl>();
        is_firstSet_done = false;
        is_shootable = false;
        is_hitted = false;
        cur_playerID =1;
        LoadData();
        setStat(cur_playerID);
    }
    private void Update()
    {
        if (gameManager.isBattleStart)
        {
            is_shootable = gameManager.isBattleStart;
        }
    }
    public void setStat(int cur_id)
    {
        
        if(is_firstSet_done == false)
        {
            player1_set();
            player2_set();
            player3_set();
            player4_set();
            
            is_firstSet_done = true;
        }


        if (cur_id == 1)
        {
            level = player1.level;
            init_damage = player1.damage;
            init_defence = player1.defence;
            init_moveSpeed = player1.move_speed;
            init_attackSpeed = player1.attack_speed;
            init_hp = player1.hp;
        }
        else if (cur_id == 2)
        {
            level = player2.level;
            init_damage = player2.damage;
            init_defence = player2.defence;
            init_moveSpeed = player2.move_speed;
            init_attackSpeed = player2.attack_speed;
            init_hp = player2.hp;
        }
        else if (cur_id == 3)
        {
            level = player3.level;
            init_damage = player3.damage;
            init_defence = player3.defence;
            init_moveSpeed = player3.move_speed;
            init_attackSpeed = player3.attack_speed;
            init_hp = player3.hp;
        }
        else if (cur_id == 4)
        {
            level = player4.level;
            init_damage = player4.damage;
            init_defence = player4.defence;
            init_moveSpeed = player4.move_speed;
            init_attackSpeed = player4.attack_speed;
            init_hp = player4.hp;
        }

        hp = init_hp;
        cur_hp = hp;

        damage_increaseRate = 1;
        defence_increaseRate = 1;
        move_speed_increaseRate = 1;
        attack_speed_increaseRate = 1;
        hp_RegenerateRate = 0;

        applyStat();
    }

    public void applyStat()
    {
        damage = init_damage * damage_increaseRate; 
        defence = init_defence * defence_increaseRate;
        move_speed = init_moveSpeed * move_speed_increaseRate;
        attack_speed= init_attackSpeed* attack_speed_increaseRate;
        
    }

    public void player1_set()
    {
        foreach (var player in myPlayerList.player)
        {
            
            if (player.id == 1)
            {
                player1.level = player.level;
                player1.damage = player.damage;
                player1.defence = player.defence;
                player1.move_speed = player.move_speed;
                player1.attack_speed = player.attack_speed;
                player1.hp = player.hp;
            }
        }
    }

    public void player2_set()
    {
        foreach (var player in myPlayerList.player)
        {
            if (player.id == 2)
            {
                player2.level = player.level;
                player2.damage = player.damage;
                player2.defence = player.defence;
                player2.move_speed = player.move_speed;
                player2.attack_speed = player.attack_speed;
                player2.hp = player.hp;
            }
        }
    }

    public void player3_set()
    {
        foreach (var player in myPlayerList.player)
        {
            if (player.id == 3)
            {
                player3.level = player.level;
                player3.damage = player.damage;
                player3.defence = player.defence;
                player3.move_speed = player.move_speed;
                player3.attack_speed = player.attack_speed;
                player3.hp = player.hp;
            }
        }
    }

    public void player4_set()
    {
        foreach (var player in myPlayerList.player)
        {
            if (player.id == 4)
            {
                player4.level = player.level;
                player4.damage = player.damage;
                player4.defence = player.defence;
                player4.move_speed = player.move_speed;
                player4.attack_speed = player.attack_speed;
                player4.hp = player.hp;
            }
        }
    }


    //�÷��̾��� �������� ��ȭ
    public void Player_damaged(float damage, GameObject attackObj)
    {
        if (!p_control.is_invincible)
        {
            is_hitted = true;
            float applyDamage = damage * (1 - (defence / 100));
            cur_hp -= applyDamage;
            Debug.Log(attackObj.name + " �� ���� " + applyDamage + " �� �������� ����");
            p_control.player_Attacked(attackObj); //�˹� �� ���� �ο�
        }
    }

    
}


