using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingame_Passive : MonoBehaviour
{
    public PlayerStat playerStat;
    protected PlayerControl playerControl;

    [Header("패시브 스킬 공통")]
    public int passiveLevel;
    public float increaseRate;
    protected virtual void Awake()
    {
        playerStat = GameManager.game.myPlayer.GetComponent<PlayerStat>();
        playerControl = GameManager.game.myPlayer.GetComponent<PlayerControl>();
        passiveLevel = transform.GetComponent<SkillInterface>().level;

        increaseRate = 1;
    }
    
    protected virtual void LevelSet(int level)
    {
        if (level == 1)
        {
            increaseRate = 1.2f;
        }
        else if (level == 2)
        {
            increaseRate = 1.5f;
        }
        else if (level == 3)
        {
            increaseRate = 2f;
        }
        else if (level == 4)
        {
            increaseRate = 2.5f;
        }
        else if (level == 5)
        {
            increaseRate = 3f;
        }
        else
        {
            Debug.Log("Already Max or Min");
        }
    }

    public void PassiveLevelUp()
    {
        passiveLevel += 1;
        LevelSet(passiveLevel);
    }

}
