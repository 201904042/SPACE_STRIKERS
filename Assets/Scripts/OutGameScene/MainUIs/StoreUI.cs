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

    //StoreUI���� �������̽��� ���� ��ü�� ItmeBtnUI�� �ش� ��ư�� Ŭ���ҽ� Interface GetValue�� ����

    private void SetUIBtn()
    {
        webStoreBtn.onClick.RemoveAllListeners();
        webStoreBtn.onClick.AddListener(GotoWebStore);

        backBtn.onClick.RemoveAllListeners();
        backBtn.onClick.AddListener(GotoMain);
    }

    private void SetStoreBtns()
    {
        //���� ��ư���� ��Ÿ�� �г� �ڵ� ���
        for (int i = 0; i < StoreBtns.Length; i++)
        {
            int code = i; //�̰� ������ �� �Ȱ��� �ε����� ��ϵ�...
            StoreBtns[i].onClick.RemoveAllListeners();
            StoreBtns[i].onClick.AddListener(() => ActivatePanel(code));
        }

        ActivatePanel(0); //ù ȭ���� ��í ������
    }
    

    /// <summary>
    /// �ش� ��ȣ�� ����� �г� Ȱ��ȭ
    /// </summary>
    /// <param name="index"></param>
    private void ActivatePanel(int index)
    {
        DeactivateAllPanels(); // ��� �г� ��Ȱ��ȭ

        if (index >= 0 && index < StorePanels.Length)
        {
            StorePanels[index].SetActive(true); // ������ �гθ� Ȱ��ȭ
        }
    }

    /// <summary>
    /// ��� ����� �г��� ��Ȱ��ȭ�ϴ� �޼ҵ�
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
        Application.OpenURL("https://www.naver.com"); //web�� ��η� �����Ұ�
    }


    /// <summary>
    /// ������ ������ ���� , ���� ���� ����, ���� ����
    /// </summary>
    public static async void TradeItem(TradeData data, bool isPurchase = true)
    {
        MasterData tradeTargetMaster = DataManager.master.GetData(data.targetMasterId); //������ ������
        InvenData tradeCostItem = new InvenData(); //������ ������

        if (data.tradeCost == TradeType.Item)
        {
            tradeCostItem = DataManager.inven.GetDataWithMasterId(data.costInvenId);
            
            if (!DataManager.inven.IsEnoughItem(tradeCostItem.id, data.costAmount)) //�̳׶��� �� �˻�
            {
                //���� �Ұ� �Ұ��. �˸� �������̽� ����
                OG_UIManager.alertInterface.SetAlert("���� �Ұ�/ ��� �������� �����մϴ�");
                return;
            }
        }
        else if (data.tradeCost == TradeType.Cash)
        {
            //���� ����� ����
            OG_UIManager.alertInterface.SetAlert("���� ĳ�����Ŵ� �Ұ��մϴ�");
            return;
        }
        else
        {
            OG_UIManager.alertInterface.SetAlert("�ùٸ��� ���� �����Դϴ�");
            return;
        }

        //�ŷ� �밡 ����
        DataManager.inven.DataUpdateOrDelete(tradeCostItem.id, data.costAmount);

        //������ �������� �κ��丮�� �����ϸ� ���� ����  ������ �߰�
        DataManager.inven.DataAddOrUpdate(data.targetMasterId, data.tradeAmount);


        await DataManager.inven.SaveData();

        if(DataManager.master.GetData(tradeCostItem.masterId).type == MasterType.Parts||
            tradeTargetMaster.type == MasterType.Parts) // -> ������ �ŷ��Ұ�� ������ ������ �Ͼ�Ƿ� 
            await DataManager.parts.SaveData();

        //���� ������ �˸� �������̽� ����
        string text;
        if (isPurchase)
        {
            text = "�������� �����Ͽ����ϴ�";
        }
        else
        {
            text = "�������� �Ǹ��Ͽ����ϴ�";
        }
        OG_UIManager.alertInterface.SetAlert(text);

        
    }
}
 