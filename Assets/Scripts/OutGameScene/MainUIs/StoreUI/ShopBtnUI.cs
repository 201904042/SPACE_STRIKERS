using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopBtnUI : MonoBehaviour
{
    public Image itemImage;
    public TextMeshProUGUI costText;
    public Button button;

    public int targetItemId;
    public int itemPrice;


    private void Awake()
    {
        itemImage = transform.GetChild(0).GetComponent<Image>();
        costText = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();

    }

    public bool SetButtons(int itemMasterId ,Sprite image ,int price, bool isActive = true)
    {
        targetItemId = itemMasterId;
        itemImage.sprite = image;
        itemPrice = price;


        costText.text = $"비용 : {itemPrice}"; //일일상점에서 세일된 가격일수 있음

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(ShowPurchaseInterface);

        button.interactable = isActive; //이미 구매가 완료되었다면 false로

        return true;
    }

    public void ShowPurchaseInterface()
    {
        UIManager.purchaseInterface.GetComponent<PurchaseInterface>().SetPurchaseInterface(targetItemId, itemPrice);
    }
}
