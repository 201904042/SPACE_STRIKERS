using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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

    public MasterItem itemData;

    public void Awake()
    {
        Content = transform.GetChild(2);
        itemImage = Content.GetChild(0).GetComponent<Image>();
        itemText = Content.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        Btns = transform.GetChild(3);
        cancelBtn = Btns.GetChild(0).GetComponent<Button>();
        purchaseBtn = Btns.GetChild(1).GetComponent<Button>();

        itemData = new MasterItem();
        SetButtons();
    }

    private void SetButtons()
    {
        cancelBtn.onClick.RemoveAllListeners();
        cancelBtn.onClick.AddListener(CloseBtn);
        purchaseBtn.onClick.RemoveAllListeners();
        purchaseBtn.onClick.AddListener(PurchaseBtn);
    }

    public bool SetPurchaseInterface(int itemMasterCode)
    {
        bool success = DataManager.masterData.masterItemDic.TryGetValue(itemMasterCode, out itemData);
        if (!success) 
        {
            Debug.Log($"�ش� �ڵ带 �˻����� ���� {itemMasterCode}");
            return false;
        }

        itemImage.sprite = Resources.Load<Sprite>(itemData.spritePath);
        itemText.text = itemData.description;
        purchaseBtn.GetComponentInChildren<TextMeshProUGUI>().text = $"�� ��\n{itemData.buyPrice}";

        UIManager.PurchaseInterface.SetActive(true);
        return true;
    }


    public void CloseBtn()
    {
        gameObject.SetActive(false);
    }


    public void PurchaseBtn()
    {
        //������ ���ǿ� �����ϴ��� üũ
        if(itemData.buyPrice > DataManager.inventoryData.InvenItemDic[0].amount)
        {
            //���� �Ұ� �Ұ��. �˸� �������̽� ����
            UIManager.AlertInterface.GetComponent<AlertInterface>().SetAlert("���� �Ұ�/n�̳׶��� �����մϴ�");
            return;
        }

        InventoryItem ownMineral = DataManager.inventoryData.FindByMasterId(0).Value ;

        //���� �κ��丮�� �̳׶��� ���ҽ�Ű�� 
        DataManager.inventoryData.ModifyItem(ownMineral.storageId, ownMineral.amount - itemData.buyPrice);

        //�ش� �������� �κ��丮�� �����ϸ� ���� ����  ������ �߰�
        DataManager.inventoryData.AddNewItem(itemData.type, itemData.masterId, itemData.name, 1); //�ϴ��� �ѹ��� �Ѱ��� ����

        //���� ������ �˸� �������̽� ����
        UIManager.AlertInterface.GetComponent<AlertInterface>().SetAlert("�������� �����Ͽ����ϴ�");

    }

}
