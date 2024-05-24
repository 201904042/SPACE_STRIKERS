using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Special_Num : MonoBehaviour
{
    PlayerSpecialSkill specialScript;
    TextMeshProUGUI text;

    private int curNum;
    private void Awake()
    {
        specialScript = GameObject.Find("Player").GetComponent<PlayerSpecialSkill>();
        curNum = specialScript.specialCount;
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        curNum = specialScript.specialCount;
        text.text = curNum.ToString();
    }
}
