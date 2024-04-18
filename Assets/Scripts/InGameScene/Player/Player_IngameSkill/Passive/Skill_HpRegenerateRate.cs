using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_HpRegenerateRate : Ingame_Passive
{
    private bool is_active;
    private float generateTime;
    private float timer;
    private float hitTime;
    private float hitTimer;
    protected override void Awake()
    {
        base.Awake();
        generateTime = 1;
        timer = generateTime;
        hitTime = 3; //타격된 후 3초 뒤부터 회복
        hitTimer = hitTime;
        is_active = false;
    }

    void Update()
    {
        if (passive_level != transform.GetComponent<skill_interface>().level)
        {
            passive_level = transform.GetComponent<skill_interface>().level;
            LevelSet(passive_level);
        }
        if (p_stat.is_hitted)
        {
            hitTimer -= Time.deltaTime;
            if(hitTimer <= 0)
            {
                p_stat.is_hitted = false;
                hitTimer = hitTime;
            }
        }

        if (is_active && !p_stat.is_hitted)
        {
            timer -= Time.deltaTime;
            if(timer <= 0 && p_stat.cur_hp < p_stat.hp)
            {
                p_stat.cur_hp = p_stat.cur_hp + p_stat.hp * (increase_Rate);
                timer = generateTime;
            }
           
        }
    }

    protected override void LevelSet(int level)
    {
        if (level == 0)
        {
            increase_Rate = 0f;
            is_active = false;
        }
        else if (level == 1)
        {
            increase_Rate = 0.002f;
            is_active = true;
        }
        else if (level == 2)
        {
            increase_Rate = 0.004f;
            is_active = true;
        }
        else if (level == 3)
        {
            increase_Rate = 0.006f;
            is_active = true;
        }
        else if (level == 4)
        {
            increase_Rate = 0.008f;
            is_active = true;
        }
        else if (level == 5)
        {
            increase_Rate = 0.01f;
            is_active = true;
        }
        else
        {
            Debug.Log("Already Max or Min");
        }
    }
}
