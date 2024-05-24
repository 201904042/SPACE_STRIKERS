using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_HpRegenerateRate : Ingame_Passive
{
    private bool isActive;
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
        isActive = false;
    }

    void Update()
    {
        if (passiveLevel != transform.GetComponent<SkillInterface>().level)
        {
            passiveLevel = transform.GetComponent<SkillInterface>().level;
            LevelSet(passiveLevel);
        }
        if (playerStat.isHitted)
        {
            hitTimer -= Time.deltaTime;
            if(hitTimer <= 0)
            {
                playerStat.isHitted = false;
                hitTimer = hitTime;
            }
        }

        if (isActive && !playerStat.isHitted)
        {
            timer -= Time.deltaTime;
            if(timer <= 0 && playerStat.cur_hp < playerStat.hp)
            {
                playerStat.cur_hp = playerStat.cur_hp + playerStat.hp * (increaseRate);
                timer = generateTime;
            }
           
        }
    }

    protected override void LevelSet(int level)
    {
        if (level == 0)
        {
            increaseRate = 0f;
            isActive = false;
        }
        else if (level == 1)
        {
            increaseRate = 0.002f;
            isActive = true;
        }
        else if (level == 2)
        {
            increaseRate = 0.004f;
            isActive = true;
        }
        else if (level == 3)
        {
            increaseRate = 0.006f;
            isActive = true;
        }
        else if (level == 4)
        {
            increaseRate = 0.008f;
            isActive = true;
        }
        else if (level == 5)
        {
            increaseRate = 0.01f;
            isActive = true;
        }
        else
        {
            Debug.Log("Already Max or Min");
        }
    }
}
