using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Special_Num : MonoBehaviour
{
    PlayerStat pStat => PlayerMain.pStat;
    TextMeshProUGUI text;

    private int curNum;
    private void Awake()
    {
        curNum = pStat.USkillCount;
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        curNum = pStat.USkillCount;
        text.text = curNum.ToString();
    }
}
