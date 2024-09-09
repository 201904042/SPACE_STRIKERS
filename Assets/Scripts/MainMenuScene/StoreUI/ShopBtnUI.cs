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
        costText.text = $"��� : {price}"; //���ϻ������� ���ϵ� �����ϼ� ����

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(ShowPurchaseInterface);

        return true;
    }

    public void ShowPurchaseInterface()
    {
        UIManager.PurchaseInterface.GetComponent<PurchaseInterface>().SetPurchaseInterface(targetItem.masterId);
    }
}
