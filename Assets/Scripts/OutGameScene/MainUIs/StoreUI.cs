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

        for (int i = 0; i < PageBtns.childCount; i++)
        {
            StoreBtns[i] = PageBtns.GetChild(i).GetComponent<Button>();
        }

        for (int i = 0; i < Stores.childCount; i++)
        {
            StorePanels[i] = Stores.GetChild(i).gameObject;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
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
        if (cost > DataManager.inven.GetData(1).quantity) //�̳׶��� �� �˻�
        {
            //���� �Ұ� �Ұ��. �˸� �������̽� ����
            UIManager.alertInterface.SetAlert("���� �Ұ�/n�̳׶��� �����մϴ�");
            return;
        }
        MasterData targetData = DataManager.master.GetData(targetMasterId);
        InvenData ownMineral = DataManager.inven.GetDataWithMasterId(1).Value;
        ownMineral.quantity = ownMineral.quantity - (cost * amount);

        DataManager.inven.UpdateData(ownMineral.id, ownMineral);

        //�ش� �������� �κ��丮�� �����ϸ� ���� ����  ������ �߰�
        InvenData? checkData = DataManager.inven.GetDataWithMasterId(targetMasterId);
        if (checkData != null)
        {
            InvenData invenData = (InvenData)checkData;
            invenData.quantity += amount;
            DataManager.inven.UpdateData(invenData.id, invenData);
        }
        else
        {
            InvenData newData = new InvenData
            {
                id = DataManager.inven.GetLastKey()+1,
                masterId = targetMasterId,
                quantity = amount,
                name = targetData.name
            };

            DataManager.inven.AddData(newData);
        }

        DataManager.inven.SaveData();
        //�Ϸ�� ���̾�̽��� ����
        //DB_Firebase.UpdateFirebaseNodeFromJson(Auth_Firebase.Instance.UserId,nameof(InvenData),DataManager.inven.GetFilePath());

        //���� ������ �˸� �������̽� ����
        UIManager.alertInterface.SetAlert("�������� �����Ͽ����ϴ�");
    }
}
 