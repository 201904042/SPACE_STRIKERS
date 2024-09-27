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
            int code = i; //�̰� ������ �� �Ȱ��� �ε����� ��ϵ�...
            StoreBtns[i].onClick.RemoveAllListeners();
            StoreBtns[i].onClick.AddListener(() => ActivatePanel(code));
        }

        ActivatePanel(0);
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
        ChangeUI(UIManager.UIInstance.mainUI);
    }

    public void GotoWebStore()
    {
        Application.OpenURL("https://www.naver.com"); //web�� ��η� �����Ұ�
    }


    /// <summary>
    /// ������ ������ ���� , ���� ���� ����, ���� ����
    /// </summary>
    public static void ItemPurchase(int targetMasterId, int cost, int amount = 1)
    {
        //������ ���ǿ� �����ϴ��� üũ
        if (cost > DataManager.inventoryData.InvenItemDic[0].quantity)
        {
            //���� �Ұ� �Ұ��. �˸� �������̽� ����
            UIManager.alterInterface.SetAlert("���� �Ұ�/n�̳׶��� �����մϴ�");
            return;
        }
        MasterData targetData = new MasterData();
        bool success = DataManager.masterData.masterDic.TryGetValue(targetMasterId, out targetData);
        if (!success)
        {
            Debug.Log("������ �����͸� ã������");
            return;
        }

        InvenData ownMineral = DataManager.inventoryData.FindByMasterId(0).Value;

        //���� �κ��丮�� �̳׶��� ���ҽ�Ű�� 
        DataManager.inventoryData.ModifyItem(ownMineral.id, ownMineral.quantity - (cost * amount));

        //�ش� �������� �κ��丮�� �����ϸ� ���� ����  ������ �߰�
        DataManager.inventoryData.AddNewItem(targetData.type, targetData.id, targetData.name, amount); //�ϴ��� �ѹ��� �Ѱ��� ����

        //���� ������ �˸� �������̽� ����
        UIManager.alterInterface.SetAlert("�������� �����Ͽ����ϴ�");
    }
}
 