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

    public bool SetUIValue(int itemMasterId ,Sprite image ,int price= 1000, bool isActive = true)
    {
        targetItemId = itemMasterId;
        itemImage.sprite = image;
        itemPrice = price;


        costText.text = $"비용 : {itemPrice}"; //일일상점에서 세일된 가격일수 있음

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(PurchaseBtnHandler);

        button.interactable = isActive; //이미 구매가 완료되었다면 false로

        return true;
    }

    private void PurchaseBtnHandler()
    {
        StartCoroutine(ConfiremPurchase());
        UIManager.purchaseInterface.SetPurchaseInterface(targetItemId, itemPrice);
    }

    private IEnumerator ConfiremPurchase()
    {
        TFInterface tfInterface = UIManager.tfInterface.GetComponent<TFInterface>();
        // TF 인터페이스에서 결과를 기다림
        yield return StartCoroutine(tfInterface.GetValue());

        // 결과에 따라 아이템 판매 처리
        if ((bool)tfInterface.result)
        {
            StoreUI.ItemPurchase(targetItemId, itemPrice);
        }
        else
        {
            Debug.Log("구매가 취소되었습니다.");
        }
    }
}
