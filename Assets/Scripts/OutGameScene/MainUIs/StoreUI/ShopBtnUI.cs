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
    public int targetId; //�ش� �������� ������ �ڵ�
    public int tradeAmount; //�ѹ��� ���ŷ� �־��� ��
    public int price; //�ŷ����� (Ÿ���� Ʈ���̵� Ÿ������ ����)
    public bool isMultiTrade; //������ �ŷ� ����
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
        costText.text = $"��� : {tradePrice}"; //���ϻ������� ���ϵ� �����ϼ� ����

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
            //������ ���� �������� Ȯ�� TFâ ���
            DoubleCheck();
        }
        else
        {
            purchaseInterface.CloseInterface();
        }
    }

    private void DoubleCheck()
    {
        UIManager.tfInterface.SetTFContent("������ �����Ͻðڽ��ϱ�?");
        StartCoroutine(TFCheck());
    }

    private IEnumerator TFCheck()
    {
        TFInterface tFInterface = UIManager.tfInterface;

        yield return StartCoroutine(tFInterface.GetValue());

        if ((bool)tFInterface.result)
        {
            //����üũ �Ϸ�� ���� ����
            StoreUI.ItemPurchase(tradeData);
            if (!isMultiTrade) 
            { 
                button.interactable = false;
            }
            UIManager.alertInterface.SetAlert($"���Ű� �Ϸ�Ǿ����ϴ�");
        }
        else
        {
            UIManager.alertInterface.SetAlert($"���Ű� ��ҵǾ����ϴ�");
        }
    }
}
