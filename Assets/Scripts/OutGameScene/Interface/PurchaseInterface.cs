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

    public MasterData itemData;
    public int resultPrice;
    public int itemAmount;
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

        itemData = new MasterData();
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

    /// <summary>
    /// 지정된 매개변수로 인터페이스를 구성함. 
    /// </summary>
    public bool SetPurchaseInterface(int itemMasterCode, int itemPrice, int itemAmount = 1)
    {
        bool success = DataManager.masterData.masterDic.TryGetValue(itemMasterCode, out itemData);
        if (!success) 
        {
            Debug.Log($"해당 코드를 검색하지 못함 {itemMasterCode}");
            return false;
        }

        itemImage.sprite = Resources.Load<Sprite>(itemData.spritePath);
        itemText.text = itemData.description;
        resultPrice = itemPrice* itemAmount; //todo 이것을 해결해야함. 구매 가격이 일일상점에서 더 싼 경우가 있음
        this.itemAmount  = itemAmount;
        purchaseBtn.GetComponentInChildren<TextMeshProUGUI>().text = $"구 매\n{resultPrice}";

        return true;
    }
}
