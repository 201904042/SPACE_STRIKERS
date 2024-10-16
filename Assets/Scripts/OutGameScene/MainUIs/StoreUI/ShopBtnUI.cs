using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class ShopBtnUI : MonoBehaviour
{
    public TradeData tradeData;

    public Image itemImage;
    public TextMeshProUGUI costText;
    public Button button;
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
            SetUIValue(tradeData.tradeCost, tradeData.costInvenId, tradeData.costAmount, tradeData.targetMasterId, tradeData.tradeAmount, tradeData.isMultiTrade);
        }
    }
    //ublic class TradeData
    //{
    //    public TradeType tradeCost;
    //    public int costInvenId;   //대가로 감소될 아이템 아이디
    //    public int costAmount;    //대가로 감소될 아이템 양
    //    public int targetMasterId; //교환으로 증가될 아이템 아이디
    //    public int tradeAmount; //교환으로 증가될 아이템의 양
    //    public bool isMultiTrade; //여러번 거래 가능
    //}

    public void SetTradeData(TradeType tradeCost, int costId, int costAmount, int targetId, int tradeAmount, bool isMultiTrade)
    {
        tradeData = new TradeData{
            tradeCost = tradeCost, 
            costInvenId = costId, 
            costAmount = costAmount, 
            targetMasterId = targetId, 
            tradeAmount = tradeAmount, 
            isMultiTrade = isMultiTrade
        };

        SetUIValue(tradeData.tradeCost, tradeData.costInvenId, tradeData.costAmount, tradeData.targetMasterId, tradeData.tradeAmount, tradeData.isMultiTrade);
    }

    private void SetUIValue(TradeType tradeCost, int costId, int costAmount, int targetId, int tradeAmount, bool isMultiTrade)
    {
        MasterData masterDate = DataManager.master.GetData(targetId);
        itemImage.sprite = Resources.Load<Sprite>(masterDate.spritePath);
        costText.text = $"비용 : {costAmount}"; //일일상점에서 세일된 가격일수 있음

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(PurchaseBtnHandler);

        button.interactable = true;
    }

    private void PurchaseBtnHandler()
    {
        StartCoroutine(ConfiremPurchase());
        UIManager.purchaseInterface.SetPurchaseData(tradeData);
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
            StoreUI.TradeItem(tradeData);
            if (!tradeData.isMultiTrade) 
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
