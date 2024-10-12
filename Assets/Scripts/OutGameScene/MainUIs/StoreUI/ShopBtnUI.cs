using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum TradeType
{
    Mineral,
    Ruby,
    Cash
}

[System.Serializable]
public class TradeData
{
    public TradeType tradeCost;
    public int targetId; //해당 아이템의 마스터 코드
    public int tradeAmount; //한번의 구매로 주어질 양
    public int price; //거래가격 (타입은 트레이드 타입으로 결정)
    public bool isMultiTrade; //여러번 거래 가능
}

public class ShopBtnUI : MonoBehaviour
{
    public TradeData tradeData;

    public Image itemImage;
    public TextMeshProUGUI costText;
    public Button button;

    public int tradeId;
    public int tradeAmount;
    public int tradePrice;
    public bool isMultiTrade;

    private void Awake()
    {
        itemImage = transform.GetChild(0).GetComponent<Image>();
        costText = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();
    }

    private void Start()
    {
        if (tradeData != null)
        {
            SetUIValue(tradeData.targetId, tradeData.tradeAmount, tradeData.price, tradeData.isMultiTrade);
        }
    }

    public void SetTradeData(TradeType tradeCost, int targetMasterId, int targetAmount, int targetPrice = 1000, bool multiTrade = true)
    {
        tradeData = new TradeData{
            tradeCost = tradeCost,
            targetId = targetMasterId,
            tradeAmount = targetAmount,
            price = targetPrice,
            isMultiTrade = multiTrade,
        };

        SetUIValue(tradeData.targetId, tradeData.tradeAmount, tradeData.price, tradeData.isMultiTrade);
    }

    private void SetUIValue(int targetMasterId, int targetAmount,int targetPrice = 1000, bool multiTrade = true)
    {
        tradeId = targetMasterId;
        tradeAmount = targetAmount;
        tradePrice = targetPrice;
        isMultiTrade = multiTrade;
        MasterData masterDate = DataManager.master.GetData(tradeId);
        itemImage.sprite = Resources.Load<Sprite>(masterDate.spritePath);
        costText.text = $"비용 : {tradePrice}"; //일일상점에서 세일된 가격일수 있음

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(PurchaseBtnHandler);

        button.interactable = true;
    }

    private void PurchaseBtnHandler()
    {
        StartCoroutine(ConfiremPurchase());
        UIManager.purchaseInterface.SetPurchaseInterface(tradeId, tradePrice);
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
            StoreUI.ItemPurchase(tradeData);
            if (!isMultiTrade) 
            { 
                button.interactable = false;
            }
            UIManager.alertInterface.SetAlert($"구매가 완료되었습니다");
        }
        else
        {
            UIManager.alertInterface.SetAlert($"구매가 취소되었습니다");
        }
    }
}
