using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;

public class DailyStore : MonoBehaviour
{
    public Transform ItemBtns;
    public TextMeshProUGUI timerText;

    private void Awake()
    {
        ItemBtns = transform.GetChild(0).GetChild(0);
        timerText = transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetShopBtns()
    {
        for(int i =0; i< ItemBtns.childCount; i++)
        {
            //4개의 인터페이스에 각각의 마스터 아이템 아이디를 이미지, 가격을 부여해줘야함
            //최초에는 새로운 PlayerPref를 지정하며 24시간 마다 PlayerPref가 변화
            //PlayerPref에 마스터 아이디를 저장하여 해당 마스터 아이디로 검색 및 아이템 저장

            PlayerPrefs.GetInt($"DailyItme{i}");
        }
    }

    public void ChangeDailyItem()
    {
        //이부분은 아마 서버에서 실행해야 할듯..
        //자정이 지나면 일일 상점 초기화
    }

    public void ShowRestTime()
    {
        //자정까지 남은 시간을 보여줌
    }



}


