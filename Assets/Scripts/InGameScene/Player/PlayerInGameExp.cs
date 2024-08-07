using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInGameExp : MonoBehaviour
{
    public GameObject selectSkillUI;
    public GameObject Canvas;

    [Header("expAmount ¿ä¼Ò")]
    public int InGameLv;
    public float maxExp;
    public float curExp;
    public float ExpGet
    {
        get => curExp;
        set
        {
            curExp += value;
            if (curExp >= maxExp)
            {
                LevelUP();
            }
        }
    }

    private void Awake()
    {
        InGameLv = 1;
        maxExp = 5f;
        curExp = 0;
    }

    public void LevelUP()
    {
        InGameLv++;
        curExp = 0;
        maxExp = maxExp +5;

        selectSkillUI.SetActive(true);

        Time.timeScale = 0f;
    }

    
}
