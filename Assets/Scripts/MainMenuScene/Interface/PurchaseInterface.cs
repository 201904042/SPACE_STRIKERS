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
            Debug.Log($"해당 코드를 검색하지 못함 {itemMasterCode}");
            return false;
        }

        itemImage.sprite = Resources.Load<Sprite>(itemData.spritePath);
        itemText.text = itemData.description;
        resultPrice = itemPrice* itemAmount;
        this.itemAmount  = itemAmount;
        purchaseBtn.GetComponentInChildren<TextMeshProUGUI>().text = $"구 매\n{this.resultPrice}";

        UIManager.PurchaseInterface.SetActive(true);
        return true;
    }


    public void CloseBtn()
    {
        gameObject.SetActive(false);
    }


    public void PurchaseBtn()
    {
        //구매의 조건에 부합하는지 체크
        if(resultPrice > DataManager.inventoryData.InvenItemDic[0].amount)
        {
            //구매 불가 할경우. 알림 인터페이스 오픈
            UIManager.AlertInterface.GetComponent<AlertInterface>().SetAlert("구매 불가/n미네랄이 부족합니다");
            return;
        }

        InvenItemData ownMineral = DataManager.inventoryData.FindByMasterId(0).Value ;

        //구매 인벤토리의 미네랄을 감소시키고 
        DataManager.inventoryData.ModifyItem(ownMineral.storageId, ownMineral.amount - resultPrice);

        //해당 아이템이 인벤토리에 존재하면 개수 증가  없으면 추가
        DataManager.inventoryData.AddNewItem(itemData.type, itemData.masterId, itemData.name, itemAmount); //일단은 한번에 한개만 증가

        //구매 성공시 알림 인터페이스 오픈
        UIManager.AlertInterface.GetComponent<AlertInterface>().SetAlert("아이템을 구매하였습니다");

    }

}
