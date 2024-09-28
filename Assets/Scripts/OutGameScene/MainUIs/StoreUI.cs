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

    protected override void Awake()
    {
        base.Awake();
    }

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
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        SetStoreBtns();
        SetBtn();
    }

    private void SetBtn()
    {
        webStoreBtn.onClick.RemoveAllListeners();
        webStoreBtn.onClick.AddListener(GotoWebStore);

        backBtn.onClick.RemoveAllListeners();
        backBtn.onClick.AddListener(GotoMain);
    }

    private void SetStoreBtns()
    {
        for (int i = 0; i < PageBtns.childCount; i++)
        {
            StoreBtns[i] = PageBtns.GetChild(i).GetComponent<Button>();
        }

        for (int i = 0; i < Stores.childCount; i++)
        {
            StorePanels[i] = Stores.GetChild(i).gameObject;
        }

        BtnInits();
    }

    private void BtnInits()
    {
        for (int i = 0; i < StoreBtns.Length; i++)
        {
            int code = i; //이거 없으면 다 똑같은 인덱스로 등록됨...
            StoreBtns[i].onClick.RemoveAllListeners();
            StoreBtns[i].onClick.AddListener(() => ActivatePanel(code));
        }

        ActivatePanel(0);
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
        ChangeUI(UIManager.UIInstance.mainUI);
    }

    public void GotoWebStore()
    {
        Application.OpenURL("https://www.naver.com"); //web의 경로로 변경할것
    }


    /// <summary>
    /// 구매할 아이템 정보 , 개당 구매 가격, 구매 개수
    /// </summary>
    public static void ItemPurchase(int targetMasterId, int cost, int amount = 1)
    {
        //아이템 구매 시스템, 데이터변형
        ////구매의 조건에 부합하는지 체크
        //if (cost > DataManager.inven.InvenItemDic[0].quantity)
        //{
        //    //구매 불가 할경우. 알림 인터페이스 오픈
        //    UIManager.alterInterface.SetAlert("구매 불가/n미네랄이 부족합니다");
        //    return;
        //}
        //MasterData targetData = new MasterData();
        //bool success = DataManager.master.masterDic.TryGetValue(targetMasterId, out targetData);
        //if (!success)
        //{
        //    Debug.Log("마스터 데이터를 찾지못함");
        //    return;
        //}

        //InvenData ownMineral = DataManager.inven.GetDataWithMasterId(0).Value;

        ////구매 인벤토리의 미네랄을 감소시키고 
        //DataManager.inven.ModifyItem(ownMineral.id, ownMineral.quantity - (cost * amount));

        ////해당 아이템이 인벤토리에 존재하면 개수 증가  없으면 추가
        //DataManager.inven.AddNewItem(targetData.type, targetData.id, targetData.name, amount); //일단은 한번에 한개만 증가

        ////구매 성공시 알림 인터페이스 오픈
        //UIManager.alterInterface.SetAlert("아이템을 구매하였습니다");
    }
}
 