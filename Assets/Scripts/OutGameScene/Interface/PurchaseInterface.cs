using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseInterface : UIInterface
{
    public Transform Content;
    public Image itemImage;
    public TextMeshProUGUI itemText;
    public Transform Btns;
    public Button cancelBtn;
    public Button purchaseBtn;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void SetComponent()
    {
        base.SetComponent();
        Content = transform.GetChild(2);
        itemImage = Content.GetChild(0).GetComponent<Image>();
        itemText = Content.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        Btns = transform.GetChild(3);
        cancelBtn = Btns.GetChild(0).GetComponent<Button>();
        purchaseBtn = Btns.GetChild(1).GetComponent<Button>();

    }

    /// <summary>
    /// ������ �Ű������� �������̽��� ������. 
    /// </summary>
    public bool SetPurchaseData(TradeData tradeData)
    {
        MasterData itemData = DataManager.master.GetData(tradeData.targetMasterId);

        itemImage.sprite = Resources.Load<Sprite>(itemData.spritePath);
        itemText.text = itemData.description;
        purchaseBtn.GetComponentInChildren<TextMeshProUGUI>().text = $"�� ��\n{tradeData.costAmount}";

        return true;
    }

    /// <summary>
    /// ���� ��ũ��Ʈ���� ���.
    /// </summary>
    public override IEnumerator GetValue()
    {
        yield return base.GetValue();

        //���� �ʱ�ȭ
        result = null;

        //Ȯ�� ���. ��ư �ڵ鷯 ����
        purchaseBtn.onClick.RemoveAllListeners();
        cancelBtn.onClick.RemoveAllListeners();
        purchaseBtn.onClick.AddListener(() => OnConfirm(true));
        cancelBtn.onClick.AddListener(() => OnConfirm(false));

        // ����ڰ� ��ư�� ���� ������ ���
        yield return new WaitUntil(() => result.HasValue);

        CloseInterface();

        //Ȯ���� �������� ��ȯ�� ����
        yield return result.Value;
    }

    
}
