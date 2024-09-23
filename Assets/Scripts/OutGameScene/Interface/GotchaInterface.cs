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

    private int gotchaGrade = 0; // 1�ϱ� 2�߱� 3���


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
            gotchaContentText.text =  "E�� ���� 40%\r\nD�� ���� 25%\r\nC�� ���� 15%\r\nB�� ���� 10%\r\n�Һ� ������ 10%"; //�ϱ�
            acceptByMoneyText.text = "�̳׶�\n1000";
            acceptByCouponText.text = "�ϱ޻̱��\n1";
        }
        else if (gotchaGrade == 2)
        {
            gotchaContentText.text= " C�� ���� 35%\r\nB�� ���� 20%\r\nA�� ���� 20%\r\n�Һ� ������ 25%\r\n�̳׶� 1000~2000"; //�߱�
            acceptByMoneyText.text = "�̳׶�\n10000";
            acceptByCouponText.text = "�߱޻̱��\n1";

        }
        else if (gotchaGrade == 3)
        {
            gotchaContentText.text=  " B�� ���� 30%\r\nA�� ���� 30%\r\nS�� ���� 30%\r\n�Һ� ������ 10%\r\n�̳׶� 1000~5000"; //���
            acceptByMoneyText.text = "�̳׶�\n100000";
            acceptByCouponText.text = "��޻̱��\n1";
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
        //�÷��̾ ��í�� �ʿ��� ���� ������� üũ


        //��í���� �޼���
        Debug.Log(gotchaGrade + "�Ӵ� ���� ����");
        gameObject.SetActive(false);
    }

    public void GotchaByCoupon()
    {
        //�÷��̾ ��í�� �ʿ��� ������ ������� üũ

        //��í���� �޼���
        Debug.Log(gotchaGrade + "���� ���� ����");
        gameObject.SetActive(false);
    }
}
