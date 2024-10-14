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
    /// 지정된 매개변수로 인터페이스를 구성함. 
    /// </summary>
    public bool SetPurchaseData(TradeData tradeData)
    {
        MasterData itemData = DataManager.master.GetData(tradeData.targetId);

        itemImage.sprite = Resources.Load<Sprite>(itemData.spritePath);
        itemText.text = itemData.description;
        purchaseBtn.GetComponentInChildren<TextMeshProUGUI>().text = $"구 매\n{tradeData.costAmount}";

        return true;
    }

    /// <summary>
    /// 사용될 스크립트에서 사용.
    /// </summary>
    public override IEnumerator GetValue()
    {
        yield return base.GetValue();

        //변수 초기화
        result = null;

        //확인 취소. 버튼 핸들러 세팅
        purchaseBtn.onClick.RemoveAllListeners();
        cancelBtn.onClick.RemoveAllListeners();
        purchaseBtn.onClick.AddListener(() => OnConfirm(true));
        cancelBtn.onClick.AddListener(() => OnConfirm(false));

        // 사용자가 버튼을 누를 때까지 대기
        yield return new WaitUntil(() => result.HasValue);

        CloseInterface();

        //확인을 눌렀을때 반환할 변수
        yield return result.Value;
    }

    
}
