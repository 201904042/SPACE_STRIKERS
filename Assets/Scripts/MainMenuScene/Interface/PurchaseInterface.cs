using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseInterface : MonoBehaviour
{
    public Transform Content;
    public Image itemImage;
    public TextMeshProUGUI itemText;
    public Transform Btns;
    public Button cancelBtn;
    public Button purchaseBtn;

    public MasterItemData itemData;
    public int resultPrice;
    public int itemAmount;


    public void Awake()
    {
        Content = transform.GetChild(2);
        itemImage = Content.GetChild(0).GetComponent<Image>();
        itemText = Content.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        Btns = transform.GetChild(3);
        cancelBtn = Btns.GetChild(0).GetComponent<Button>();
        purchaseBtn = Btns.GetChild(1).GetComponent<Button>();

        itemData = new MasterItemData();
        SetButtons();
    }

    private void SetButtons()
    {
        cancelBtn.onClick.RemoveAllListeners();
        cancelBtn.onClick.AddListener(CloseBtn);
        purchaseBtn.onClick.RemoveAllListeners();
        purchaseBtn.onClick.AddListener(PurchaseBtn);
    }

    public bool SetPurchaseInterface(int itemMasterCode, int itemPrice, int itemAmount = 1)
    {
        bool success = DataManager.masterData.masterItemDic.TryGetValue(itemMasterCode, out itemData);
        if (!success) 
        {
            Debug.Log($"�ش� �ڵ带 �˻����� ���� {itemMasterCode}");
            return false;
        }

        itemImage.sprite = Resources.Load<Sprite>(itemData.spritePath);
        itemText.text = itemData.description;
        resultPrice = itemPrice* itemAmount;
        this.itemAmount  = itemAmount;
        purchaseBtn.GetComponentInChildren<TextMeshProUGUI>().text = $"�� ��\n{this.resultPrice}";

        UIManager.PurchaseInterface.gameObject.SetActive(true);
        return true;
    }


    public void CloseBtn()
    {
        gameObject.SetActive(false);
    }


    public void PurchaseBtn()
    {
        //������ ���ǿ� �����ϴ��� üũ
        if(resultPrice > DataManager.inventoryData.InvenItemDic[0].amount)
        {
            //���� �Ұ� �Ұ��. �˸� �������̽� ����
            UIManager.AlertInterface.GetComponent<AlertInterface>().SetAlert("���� �Ұ�/n�̳׶��� �����մϴ�");
            return;
        }

        InvenItemData ownMineral = DataManager.inventoryData.FindByMasterId(0).Value ;

        //���� �κ��丮�� �̳׶��� ���ҽ�Ű�� 
        DataManager.inventoryData.ModifyItem(ownMineral.storageId, ownMineral.amount - resultPrice);

        //�ش� �������� �κ��丮�� �����ϸ� ���� ����  ������ �߰�
        DataManager.inventoryData.AddNewItem(itemData.type, itemData.masterId, itemData.name, itemAmount); //�ϴ��� �ѹ��� �Ѱ��� ����

        //���� ������ �˸� �������̽� ����
        UIManager.AlertInterface.GetComponent<AlertInterface>().SetAlert("�������� �����Ͽ����ϴ�");

    }

}
