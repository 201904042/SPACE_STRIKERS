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


        costText.text = $"��� : {itemPrice}"; //���ϻ������� ���ϵ� �����ϼ� ����

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(ShowPurchaseInterface);

        button.interactable = isActive; //�̹� ���Ű� �Ϸ�Ǿ��ٸ� false��

        return true;
    }

    public void ShowPurchaseInterface()
    {
        UIManager.PurchaseInterface.GetComponent<PurchaseInterface>().SetPurchaseInterface(targetItemId, itemPrice);
    }
}
