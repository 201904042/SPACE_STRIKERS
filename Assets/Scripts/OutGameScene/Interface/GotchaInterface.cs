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

    public int gotchaTier = 0; // 1하급 2중급 3상급
    public int paidTypeCode;

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


    public override IEnumerator GetValue()
    {
        yield return base.GetValue();

        result = null;
        paidTypeCode = -1;  // 미네랄1, 쿠폰4,5,6
        TextSet(gotchaTier);

        switch (gotchaTier)
        {
            case 1 : gotchaTier = 4; break;
            case 2: gotchaTier = 5; break;
            case 3: gotchaTier = 6; break;
            default: Debug.Log($"티어가 맞지 않음 {gotchaTier}");  break;
        }

        acceptByMoney.onClick.RemoveAllListeners();
        acceptByCoupon.onClick.RemoveAllListeners();
        closeBtn.onClick.RemoveAllListeners();
        acceptByMoney.onClick.AddListener(() => AcceptBtn(true, 0));
        acceptByCoupon.onClick.AddListener(() => AcceptBtn(true, gotchaTier));
        closeBtn.onClick.AddListener(() => OnConfirm(false));

        // 사용자가 버튼을 누를 때까지 대기
        yield return new WaitUntil(() => result.HasValue);

        CloseInterface(); // 인터페이스 숨기기

        yield return result;
    }


    public void SetGotchaInterface(int tier)
    {
        gotchaTier = tier;
    }

    private void TextSet(int grade)
    {
        GotchaData gotchaData = DataManager.gotcha.GetData(grade);
        StringBuilder sb = new StringBuilder();
        foreach(GotchaInform gi in gotchaData.items)
        {
            string itemName = TransText(gi.type);
            sb.Append($"{itemName} : {gi.rate}%");
        }
        gotchaContentText.text = sb.ToString();

        foreach (GotchaCost gc in gotchaData.cost)
        {
            if(gc.id == 1)
            {
                ItemAmountForMoney.SetAmountUI(gc.id, gc.amount);
            }
            else
            {
                ItemAmountForCoupon.SetAmountUI(gc.id, gc.amount);
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

    private void AcceptBtn(bool result, int paidType)
    {
        OnConfirm(result);
        paidTypeCode = paidType;
    }
}
