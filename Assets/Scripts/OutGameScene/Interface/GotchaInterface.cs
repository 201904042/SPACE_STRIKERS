using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class GotchaInterface : MonoBehaviour
{
    public TextMeshProUGUI gotchaContentText;

    public Transform Btns;
    public Button closeBtn;
    public Button acceptByMoney;
    public Button acceptByCoupon;

    public TextMeshProUGUI acceptByMoneyText;
    public TextMeshProUGUI acceptByCouponText;

    private int gotchaGrade = 0; // 1하급 2중급 3상급


    private void Awake()
    {
        gotchaContentText = transform.GetChild(2).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        Btns = transform.GetChild(3);
        closeBtn = Btns.GetChild(0).GetComponent<Button>();
        acceptByMoney = Btns.GetChild(1).GetComponent<Button>();
        acceptByMoneyText = acceptByMoney.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        acceptByCoupon = Btns.GetChild(2).GetComponent<Button>();
        acceptByCouponText = acceptByCoupon.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void SetGotchaInterface(int tier)
    {
        gotchaGrade = tier;

        TextSet();
        BtnSet();
    }

    private void TextSet()
    {

        if (gotchaGrade == 1)
        {
            gotchaContentText.text =  "E급 파츠 40%\r\nD급 파츠 25%\r\nC급 파츠 15%\r\nB급 파츠 10%\r\n소비 아이템 10%"; //하급
            acceptByMoneyText.text = "미네랄\n1000";
            acceptByCouponText.text = "하급뽑기권\n1";
        }
        else if (gotchaGrade == 2)
        {
            gotchaContentText.text= " C급 파츠 35%\r\nB급 파츠 20%\r\nA급 파츠 20%\r\n소비 아이템 25%\r\n미네랄 1000~2000"; //중급
            acceptByMoneyText.text = "미네랄\n10000";
            acceptByCouponText.text = "중급뽑기권\n1";

        }
        else if (gotchaGrade == 3)
        {
            gotchaContentText.text=  " B급 파츠 30%\r\nA급 파츠 30%\r\nS급 파츠 30%\r\n소비 아이템 10%\r\n미네랄 1000~5000"; //상급
            acceptByMoneyText.text = "미네랄\n100000";
            acceptByCouponText.text = "고급뽑기권\n1";
        }
    }


    private void BtnSet()
    {

        closeBtn.onClick.RemoveAllListeners();
        closeBtn.onClick.AddListener(CloseBtn);
        acceptByMoney.onClick.RemoveAllListeners();
        acceptByMoney.onClick.AddListener(GotchaByMoney);
        acceptByCoupon.onClick.RemoveAllListeners();
        acceptByCoupon.onClick.AddListener(GotchaByCoupon);
    }

    public void CloseBtn()
    {
        gameObject.SetActive(false);
    }
    public void GotchaByMoney()
    {
        //플레이어가 가챠에 필요한 돈이 충분한지 체크


        //가챠진행 메서드
        Debug.Log(gotchaGrade + "머니 가샤 진행");
        gameObject.SetActive(false);
    }

    public void GotchaByCoupon()
    {
        //플레이어가 가챠에 필요한 쿠폰이 충분한지 체크

        //가챠진행 메서드
        Debug.Log(gotchaGrade + "쿠폰 가샤 진행");
        gameObject.SetActive(false);
    }
}
