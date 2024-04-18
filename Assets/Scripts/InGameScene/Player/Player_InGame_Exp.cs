using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_InGame_Exp : MonoBehaviour
{
    public GameObject LevelUp_UI;
    public GameObject Canvas;

    [Header("exp ¿ä¼Ò")]
    public int InGame_Lv;
    public float max_exp;
    public float cur_exp;

    // Start is called before the first frame update
    void Start()
    {
        InGame_Lv = 1;
        max_exp = 5f;
        cur_exp = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(cur_exp >= max_exp)
        {
            LevelUP();
        }
            
    }
    public void LevelUP()
    {
        InGame_Lv++;
        cur_exp = 0;
        max_exp = max_exp +5;

        
        Instantiate(LevelUp_UI,Canvas.transform);
        Time.timeScale = 0f;
    }

    
}
