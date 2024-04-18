using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Special_Num : MonoBehaviour
{
    Player_specialSkill specialScript;
    TextMeshProUGUI text;

    private int curNum;
    private void Awake()
    {
        specialScript = GameObject.Find("Player").GetComponent<Player_specialSkill>();
        curNum = specialScript.special_count;
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        curNum = specialScript.special_count;
        text.text = curNum.ToString();
    }
}
