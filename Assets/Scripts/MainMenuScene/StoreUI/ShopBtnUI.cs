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


    private void Awake()
    {
        itemImage = GetComponentInChildren<Image>();
        costText = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();

    }

    public bool SetButtons(int itemMasterId ,Sprite image ,int price)
    {
        targetItemId = itemMasterId;
        itemImage.sprite = image;
        costText.text = $"비용 : {price}"; //일일상점에서 세일된 가격일수 있음

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(ShowPurchaseInterface);

        return true;
    }

    public void ShowPurchaseInterface()
    {
        UIManager.PurchaseInterface.GetComponent<PurchaseInterface>().SetPurchaseInterface(targetItem.masterId);
    }
}
