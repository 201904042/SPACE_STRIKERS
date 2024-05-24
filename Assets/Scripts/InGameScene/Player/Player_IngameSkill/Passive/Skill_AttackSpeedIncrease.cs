using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_AttackSpeedIncrease : Ingame_Passive
{
    protected override void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    void Update()
    {
        if (passiveLevel != transform.GetComponent<SkillInterface>().level)
        {
            passiveLevel = transform.GetComponent<SkillInterface>().level;
            LevelSet(passiveLevel);
        }
    }

    protected override void LevelSet(int level)
    {
        base.LevelSet(level);

        playerStat.attackSpeedIncreaseRate = increaseRate;
        playerStat.ApplyStat();
    }
}
