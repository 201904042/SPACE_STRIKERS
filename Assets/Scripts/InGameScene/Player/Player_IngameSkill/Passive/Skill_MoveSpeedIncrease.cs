using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_MoveSpeedIncrease : Ingame_Passive
{
    protected override void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    void Update()
    {
        if (passive_level != transform.GetComponent<skill_interface>().level)
        {
            passive_level = transform.GetComponent<skill_interface>().level;
            LevelSet(passive_level);
        }
        
    }

    protected override void LevelSet(int level)
    {
        base.LevelSet(level);

        p_stat.move_speed_increaseRate = increase_Rate;
        p_stat.applyStat();
    }
}
