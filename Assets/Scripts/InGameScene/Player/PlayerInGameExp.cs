using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInGameExp : MonoBehaviour
{
    public GameObject UI_levelUp;
    public GameObject Canvas;

    [Header("exp ¿ä¼Ò")]
    public int InGameLv;
    public float maxExp;
    public float curExp;


    // Start is called before the first frame update
    void Start()
    {
        InGameLv = 1;
        maxExp = 5f;
        curExp = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(curExp >= maxExp)
        {
            LevelUP();
        }
            
    }
    public void LevelUP()
    {
        InGameLv++;
        curExp = 0;
        maxExp = maxExp +5;

        
        Instantiate(UI_levelUp, Canvas.transform);
        Time.timeScale = 0f;
    }

    
}
