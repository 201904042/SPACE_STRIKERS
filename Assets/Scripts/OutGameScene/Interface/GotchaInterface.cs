using System;
using System.Collections;
using System.Collections.Generic;
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

    public TextMeshProUGUI acceptByMoneyText;
    public TextMeshProUGUI acceptByCouponText;

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
        acceptByMoneyText = acceptByMoney.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        acceptByCoupon = Btns.GetChild(2).GetComponent<Button>();
        acceptByCouponText = acceptByCoupon.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
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
        if (grade == 1)
        {
            gotchaContentText.text =  "E급 파츠 40%\r\nD급 파츠 25%\r\nC급 파츠 15%\r\nB급 파츠 10%\r\n소비 아이템 10%"; //하급
            acceptByMoneyText.text = "미네랄\n1000";
            acceptByCouponText.text = "하급뽑기권\n1";
        }
        else if (grade == 2)
        {
            gotchaContentText.text= " C급 파츠 35%\r\nB급 파츠 20%\r\nA급 파츠 20%\r\n소비 아이템 25%\r\n미네랄 1000~2000"; //중급
            acceptByMoneyText.text = "미네랄\n10000";
            acceptByCouponText.text = "중급뽑기권\n1";

        }
        else if (grade == 3)
        {
            gotchaContentText.text=  " B급 파츠 30%\r\nA급 파츠 30%\r\nS급 파츠 30%\r\n소비 아이템 10%\r\n미네랄 1000~5000"; //상급
            acceptByMoneyText.text = "미네랄\n100000";
            acceptByCouponText.text = "고급뽑기권\n1";
        }
    }

    private void AcceptBtn(bool result, int paidType)
    {
        OnConfirm(result);
        paidTypeCode = paidType;
    }
}
