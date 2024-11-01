using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Interface_StartCount : UIInterface
{
    public TextMeshProUGUI countText;
    private const int startTime = 3;
    private int count;

    protected override void Awake()
    {
        base.Awake();
    }


    public override void SetComponent()
    {
        base.SetComponent();
        countText = transform.GetComponentInChildren<TextMeshProUGUI>();
    }

    public IEnumerator StartCountdown()
    {
        OpenInterface();
        int count = startTime;

        
        while (count > 0)
        {
            // UI 텍스트에 카운트다운 숫자 표시
            countText.text = count.ToString();
            yield return new WaitForSeconds(1f);  // 1초 대기
            count--;
        }

        // 마지막 카운트 후 게임 시작 전 "Start!" 표시
        countText.text = "Go!";
        yield return new WaitForSeconds(1f);  // 1초 대기 후 시작
        CloseInterface();
    }
}
