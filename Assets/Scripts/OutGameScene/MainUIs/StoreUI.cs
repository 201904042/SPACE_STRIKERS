using Firebase.Auth;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UI;

public class StoreUI : MainUIs
{
    public Transform PageBtns;

    public Transform Stores;
    public GameObject gotchaPage;
    public GameObject dailyPage;
    public GameObject itemPage;
    public GameObject moneyPage;

    public Transform Btn;
    public Button webStoreBtn;
    public Button backBtn;

    public GameObject[] StorePanels; 
    public Button[] StoreBtns;

   
    public override void SetComponent()
    {
        base.SetComponent();
        PageBtns = transform.GetChild(0).GetChild(0);
        Stores = transform.GetChild(0).GetChild(1);
        StorePanels = new GameObject[4];
        StoreBtns = new Button[4];
        Btn = transform.GetChild(1);
        webStoreBtn = Btn.GetChild(0).GetComponent<Button>();
        backBtn = Btn.GetChild(1).GetComponent<Button>();

        for (int i = 0; i < PageBtns.childCount; i++)
        {
            StoreBtns[i] = PageBtns.GetChild(i).GetComponent<Button>();
        }

        for (int i = 0; i < Stores.childCount; i++)
        {
            StorePanels[i] = Stores.GetChild(i).gameObject;
        }
    }

    public override IEnumerator SetUI()
    {
        yield return base.SetUI();
        SetStoreBtns();
        SetUIBtn();
    }

    //StoreUI에서 인터페이스를 띄우는 객체는 ItmeBtnUI로 해당 버튼을 클릭할시 Interface GetValue를 받음

    private void SetUIBtn()
    {
        webStoreBtn.onClick.RemoveAllListeners();
        webStoreBtn.onClick.AddListener(GotoWebStore);

        backBtn.onClick.RemoveAllListeners();
        backBtn.onClick.AddListener(GotoMain);
    }

    private void SetStoreBtns()
    {
        //상점 버튼별로 나타낼 패널 코드 등록
        for (int i = 0; i < StoreBtns.Length; i++)
        {
            int code = i; //이거 없으면 다 똑같은 인덱스로 등록됨...
            StoreBtns[i].onClick.RemoveAllListeners();
            StoreBtns[i].onClick.AddListener(() => ActivatePanel(code));
        }

        ActivatePanel(0); //첫 화면은 가챠 페이지
    }
    

    /// <summary>
    /// 해당 번호의 스토어 패널 활성화
    /// </summary>
    /// <param name="index"></param>
    private void ActivatePanel(int index)
    {
        DeactivateAllPanels(); // 모든 패널 비활성화

        if (index >= 0 && index < StorePanels.Length)
        {
            StorePanels[index].SetActive(true); // 선택한 패널만 활성화
        }
    }

    /// <summary>
    /// 모든 스토어 패널을 비활성화하는 메소드
    /// </summary>
    private void DeactivateAllPanels()
    {
        foreach (GameObject panel in StorePanels)
        {
            panel.SetActive(false);
        }
    }


    public void GotoMain()
    {
        ChangeUI(OG_UIManager.UIInstance.mainUI);
    }

    public void GotoWebStore()
    {
        Application.OpenURL("https://www.naver.com"); //web의 경로로 변경할것
    }


    /// <summary>
    /// 구매할 아이템 정보 , 개당 구매 가격, 구매 개수
    /// </summary>
    public static async void TradeItem(TradeData data, bool isPurchase = true)
    {
        MasterData tradeTargetMaster = DataManager.master.GetData(data.targetMasterId); //증가할 아이템
        InvenData tradeCostItem = new InvenData(); //감소할 아이템

        if (data.tradeCost == TradeType.Item)
        {
            tradeCostItem = DataManager.inven.GetDataWithMasterId(data.costInvenId);
            
            if (!DataManager.inven.IsEnoughItem(tradeCostItem.id, data.costAmount)) //미네랄의 양 검사
            {
                //구매 불가 할경우. 알림 인터페이스 오픈
                OG_UIManager.alertInterface.SetAlert("구매 불가/ 비용 아이템이 부족합니다");
                return;
            }
        }
        else if (data.tradeCost == TradeType.Cash)
        {
            //현금 결재는 보류
            OG_UIManager.alertInterface.SetAlert("아직 캐쉬구매는 불가합니다");
            return;
        }
        else
        {
            OG_UIManager.alertInterface.SetAlert("올바르지 않은 구매입니다");
            return;
        }

        //거래 대가 감소
        DataManager.inven.DataUpdateOrDelete(tradeCostItem.id, data.costAmount);

        //구매한 아이템이 인벤토리에 존재하면 개수 증가  없으면 추가
        DataManager.inven.DataAddOrUpdate(data.targetMasterId, data.tradeAmount);


        await DataManager.inven.SaveData();

        if(DataManager.master.GetData(tradeCostItem.masterId).type == MasterType.Parts||
            tradeTargetMaster.type == MasterType.Parts) // -> 파츠를 거래할경우 무조건 삭제가 일어나므로 
            await DataManager.parts.SaveData();

        //구매 성공시 알림 인터페이스 오픈
        string text;
        if (isPurchase)
        {
            text = "아이템을 구매하였습니다";
        }
        else
        {
            text = "아이템을 판매하였습니다";
        }
        OG_UIManager.alertInterface.SetAlert(text);

        
    }
}
 