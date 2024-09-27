using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Search;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;



public class DailyStore : MonoBehaviour
{
    public Transform ItemBtns;
    public TextMeshProUGUI timerText;

    public string dateIndex;
    public DateTime curDate;

    public StoreItemData[] registStoreItem = new StoreItemData[4]; 

    private void Awake()
    {
        ItemBtns = transform.GetChild(0).GetChild(0);
        timerText = transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        //시작시 아이템을 바꿔야하는지 여부 결정
        if (PlayerPrefs.GetString("DateIndex") != null) //저장된 날짜 인덱스가 존재하는지 체크
        {
            dateIndex = PlayerPrefs.GetString("DateIndex");
            if (dateIndex != DateTime.Now.ToString("yyyy-MM-dd")) //저장된 날짜와 현재 날짜를 체크함
            {
                //저장된 날짜 인덱스가 없거나 저장된 날짜와 현재 날짜가 다르다면 리스트를 변경함
                ChangeItemList();
            }
            else
            {
                //지금은 임시로 하급뽑기권만 todo -> 저장된 아이템을 불러오도록
                registStoreItem[0] = DataManager.storeData.storeItemDic[0]; //하급뽑기권
                registStoreItem[1] = DataManager.storeData.storeItemDic[0]; //하급뽑기권
                registStoreItem[2] = DataManager.storeData.storeItemDic[0]; //하급뽑기권
                registStoreItem[3] = DataManager.storeData.storeItemDic[0]; //하급뽑기권
            }
        }
        else
        {
            ChangeItemList();
        }


        SetShopBtns();
    }

    private void Update()
    {
        if (timerText.gameObject.activeSelf)
        {
            ShowRestTime();
        }
        
    }


    private void ChangeItemList()
    {
        //상점에 보여질 아이템의 리스트를 새로운 데이터로 변경하고, 날짜 인덱스를 업데이트
        registStoreItem = new StoreItemData[4];

        //임시로 하급뽑기권만 todo-> 랜덤한 아이템을 불러오도록
        registStoreItem[0] = DataManager.storeData.storeItemDic[0]; //하급뽑기권
        registStoreItem[1] = DataManager.storeData.storeItemDic[0]; //하급뽑기권
        registStoreItem[2] = DataManager.storeData.storeItemDic[0]; //하급뽑기권
        registStoreItem[3] = DataManager.storeData.storeItemDic[0]; //하급뽑기권

        PlayerPrefs.SetString("DateIndex", DateTime.Now.ToString("yyyy-MM-dd"));
    }

    public void SetShopBtns()
    {
        for(int i =0; i< ItemBtns.childCount; i++)
        {
            //4개의 인터페이스에 각각의 마스터 아이템 아이디를 이미지, 가격을 부여해줘야함
            //최초에는 새로운 PlayerPref를 지정하며 24시간 마다 PlayerPref가 변화
            //PlayerPref에 마스터 아이디를 저장하여 해당 마스터 아이디로 검색 및 아이템 저장
            MasterData target = DataManager.masterData.masterDic[DataManager.storeData.storeItemDic[registStoreItem[i].storeItemId].masterId];
            Sprite targetImage = Resources.Load<Sprite>(target.spritePath);
            ItemBtns.GetChild(i).GetComponent<ShopBtnUI>().SetUIValue(target.id, targetImage, 1000* (3/4), true); //id, 이미지, 가격 , 구매가능여부
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
        DateTime now = DateTime.Now; //현재 날짜와 시간
        DateTime midnight = DateTime.Today.AddDays(1); //다음날 자정의 시간
        TimeSpan timeLeft = midnight - now; 
        timerText.text = $"{timeLeft.Hours}:{timeLeft.Minutes}:{timeLeft.Seconds}";
    }



}


