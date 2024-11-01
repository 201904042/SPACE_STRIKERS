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
    //    public int costInvenId;   //�밡�� ���ҵ� ������ ���̵�
    //    public int costAmount;    //�밡�� ���ҵ� ������ ��
    //    public int targetMasterId; //��ȯ���� ������ ������ ���̵�
    //    public int tradeAmount; //��ȯ���� ������ �������� ��
    //    public bool isMultiTrade; //������ �ŷ� ����
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
        costText.text = $"��� : {costAmount}"; //���ϻ������� ���ϵ� �����ϼ� ����

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(PurchaseBtnHandler);

        button.interactable = true;
    }

    private void PurchaseBtnHandler()
    {
        StartCoroutine(ConfiremPurchase());
        OG_UIManager.purchaseInterface.SetPurchaseData(tradeData);
    }

    private IEnumerator ConfiremPurchase()
    {
        PurchaseInterface purchaseInterface = OG_UIManager.purchaseInterface;

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
        OG_UIManager.tfInterface.SetTFContent("������ �����Ͻðڽ��ϱ�?");
        StartCoroutine(TFCheck());
    }

    private IEnumerator TFCheck()
    {
        TFInterface tFInterface = OG_UIManager.tfInterface;

        yield return StartCoroutine(tFInterface.GetValue());

        if ((bool)tFInterface.result)
        {
            //����üũ �Ϸ�� ���� ����
            StoreUI.TradeItem(tradeData);
            if (!tradeData.isMultiTrade) 
            { 
                button.interactable = false;
            }
            OG_UIManager.alertInterface.SetAlert($"���Ű� �Ϸ�Ǿ����ϴ�");
        }
        else
        {
            OG_UIManager.alertInterface.SetAlert($"���Ű� ��ҵǾ����ϴ�");
        }
    }
}
