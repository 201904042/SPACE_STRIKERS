using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingame_Passive : MonoBehaviour
{
    private GameObject player;
    [HideInInspector]
    public PlayerStat p_stat;

    [Header("패시브 스킬 공통")]
    public int passive_level;
    public float increase_Rate;
    protected virtual void Awake()
    {
        player = GameObject.Find("Player");
        p_stat= player.GetComponent<PlayerStat>();

        passive_level = transform.GetComponent<skill_interface>().level;
        increase_Rate = 1;
    }
    
    protected virtual void LevelSet(int level)
    {
        if (level == 1)
        {
            increase_Rate = 1.2f;
        }
        else if (level == 2)
        {
            increase_Rate = 1.5f;
        }
        else if (level == 3)
        {
            increase_Rate = 2f;
        }
        else if (level == 4)
        {
            increase_Rate = 2.5f;
        }
        else if (level == 5)
        {
            increase_Rate = 3f;
        }
        else
        {
            Debug.Log("Already Max or Min");
        }

    }

    public void LevelUp()
    {
        passive_level += 1;
        LevelSet(passive_level);
    }

}
