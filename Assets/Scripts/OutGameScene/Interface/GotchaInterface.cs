using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class GotchaInterface : UIInterface
{
    public TextMeshProUGUI gotchaContentText;

    public Transform Btns;
    public Button closeBtn;
    public Button acceptByMoney;
    public Button acceptByCoupon;

    public ItemAmountPref ItemAmountForMoney;
    public ItemAmountPref ItemAmountForCoupon;

    public GotchaData curGotchaData;
    public GotchaCost moneyCost;
    public GotchaCost couponCost;
    public int gotchaTier = 0; // 1하급 2중급 3상급

    public int acceptCostId;
    public int acceptCostAmount;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void SetComponent()
    {
        base.SetComponent();
        gotchaContentText = transform.GetChild(2).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        Btns = transform.GetChild(3);
        closeBtn = Btns.GetChild(0).GetComponent<Button>();
        acceptByMoney = Btns.GetChild(1).GetComponent<Button>();
        ItemAmountForMoney = acceptByMoney.transform.GetChild(0).GetComponent<ItemAmountPref>();
        acceptByCoupon = Btns.GetChild(2).GetComponent<Button>();
        ItemAmountForCoupon = acceptByCoupon.transform.GetChild(0).GetComponent<ItemAmountPref>();
    }

    public void SetGotchaInterface(int tier)
    {
        gotchaTier = tier;
        Debug.Log($"{tier}번 가챠 실행");
        curGotchaData = DataManager.gotcha.GetData(gotchaTier);

    }
    public override IEnumerator GetValue()
    {
        yield return base.GetValue();

        result = null;
        TextSet(gotchaTier);

        acceptByMoney.onClick.RemoveAllListeners();
        acceptByCoupon.onClick.RemoveAllListeners();
        closeBtn.onClick.RemoveAllListeners();
        acceptByMoney.onClick.AddListener(() => AcceptBtn(true, moneyCost.id, moneyCost.amount));
        acceptByCoupon.onClick.AddListener(() => AcceptBtn(true, couponCost.id, couponCost.amount));
        closeBtn.onClick.AddListener(() => OnConfirm(false));

        // 사용자가 버튼을 누를 때까지 대기
        yield return new WaitUntil(() => result.HasValue);

        CloseInterface(); // 인터페이스 숨기기

        yield return result;
    }

    private void TextSet(int grade)
    {
        StringBuilder sb = new StringBuilder();
        foreach(GotchaInform gi in curGotchaData.items)
        {
            string itemName = TransText(gi.type);
            sb.Append($"{itemName} : {gi.rate}%\n");
        }
        gotchaContentText.text = sb.ToString();

        foreach (GotchaCost gc in curGotchaData.cost)
        {
            if(gc.id == 1)
            {
                ItemAmountForMoney.SetAmountUI(gc.id, gc.amount);
                moneyCost = gc;
            }
            else
            {
                ItemAmountForCoupon.SetAmountUI(gc.id, gc.amount);
                couponCost = gc;
            }
        }
    }

    private string TransText(string type)
    {
        string transString = "알 수 없음";
        switch (type)
        {
            case "S": transString = "S급 파츠"; break; 
            case "A": transString = "A급 파츠"; break;
            case "B": transString = "B급 파츠"; break;
            case "C": transString = "C급 파츠"; break;
            case "D": transString = "D급 파츠"; break;
            case "consume": transString = "소비아이템"; break;
        }

        return transString;
    }

    private void AcceptBtn(bool result, int costId, int costAmount)
    {
        OnConfirm(result);
        acceptCostId = costId;
        acceptCostAmount = costAmount;
    }
}
