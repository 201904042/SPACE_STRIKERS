using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static PlayerjsonReader;

public class EnemyStat : EnemyJsonReader
{
    public int cur_EnemyID;
    public string Name;
    public string Grade;
    public bool MoveAttack;
    public bool is_Aiming;
    public float MaxHP;
    public float CurHP;
    public float Damage;
    public float MoveSpeed;
    public float AttackSpeed;
    public float exp_amount;
    public float score_amount;

    GameManager manager;

    private void Awake()
    {
        cur_EnemyID = 0;
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (transform.name == "Enemy_common(Clone)")
        {
            cur_EnemyID = Random.Range(1, 3);
        }
        else if (transform.name == "Enemy_elite(Clone)")
        {
            cur_EnemyID = Random.Range(3, 5);
        }
        else if (transform.name == "Sandbag")
        {
            cur_EnemyID = 0;
        }

        LoadData();
        setStat(cur_EnemyID);
       
    }

    public void setStat(int cur_id)
    {
        foreach (var enemy in myEnemyList.enemy)
        {
            if(enemy.e_id == cur_id)
            {
                Name = enemy.e_name;
                Grade = enemy.e_grade;
                MoveAttack = enemy.e_move_attack;
                is_Aiming = enemy.e_is_aiming;
                MaxHP = enemy.e_maxHP;
                Damage = enemy.e_damage;
                MoveSpeed = enemy.e_move_speed;
                AttackSpeed=enemy.e_attack_speed;
                exp_amount = enemy.e_exp_amount;
                score_amount = enemy.e_score_amount;
            }
            CurHP = MaxHP;
        }
    }

    
}
