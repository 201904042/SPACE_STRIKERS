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
        PurchaseInterface purchaseInterface = UIManager.purchaseInterface;

        yield return StartCoroutine(purchaseInterface.GetValue());

        if ((bool)purchaseInterface.result)
        {
            //구매전 정말 구매할지 확인 TF창 띄움
            DoubleCheck();
        }
        else
        {
            purchaseInterface.CloseInterface();
        }
    }

    private void DoubleCheck()
    {
        UIManager.tfInterface.SetTFContent("정말로 구매하시겠습니까?");
        StartCoroutine(TFCheck());
    }

    private IEnumerator TFCheck()
    {
        TFInterface tFInterface = UIManager.tfInterface;

        yield return StartCoroutine(tFInterface.GetValue());

        if ((bool)tFInterface.result)
        {
            //더블체크 완료시 구매 실행
            StoreUI.ItemPurchase(targetItemId, itemPrice);
            UIManager.alertInterface.SetAlert($"구매가 완료되었습니다");
        }
        else
        {
            UIManager.alertInterface.SetAlert($"구매가 취소되었습니다");
        }
    }
}
