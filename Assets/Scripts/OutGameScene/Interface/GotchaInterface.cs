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

    public int gotchaTier = 0; // 1�ϱ� 2�߱� 3���
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
        paidTypeCode = -1;  // �̳׶�1, ����4,5,6
        TextSet(gotchaTier);

        switch (gotchaTier)
        {
            case 1 : gotchaTier = 4; break;
            case 2: gotchaTier = 5; break;
            case 3: gotchaTier = 6; break;
            default: Debug.Log($"Ƽ� ���� ���� {gotchaTier}");  break;
        }

        acceptByMoney.onClick.RemoveAllListeners();
        acceptByCoupon.onClick.RemoveAllListeners();
        closeBtn.onClick.RemoveAllListeners();
        acceptByMoney.onClick.AddListener(() => AcceptBtn(true, 0));
        acceptByCoupon.onClick.AddListener(() => AcceptBtn(true, gotchaTier));
        closeBtn.onClick.AddListener(() => OnConfirm(false));

        // ����ڰ� ��ư�� ���� ������ ���
        yield return new WaitUntil(() => result.HasValue);

        CloseInterface(); // �������̽� �����

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
            gotchaContentText.text =  "E�� ���� 40%\r\nD�� ���� 25%\r\nC�� ���� 15%\r\nB�� ���� 10%\r\n�Һ� ������ 10%"; //�ϱ�
            acceptByMoneyText.text = "�̳׶�\n1000";
            acceptByCouponText.text = "�ϱ޻̱��\n1";
        }
        else if (grade == 2)
        {
            gotchaContentText.text= " C�� ���� 35%\r\nB�� ���� 20%\r\nA�� ���� 20%\r\n�Һ� ������ 25%\r\n�̳׶� 1000~2000"; //�߱�
            acceptByMoneyText.text = "�̳׶�\n10000";
            acceptByCouponText.text = "�߱޻̱��\n1";

        }
        else if (grade == 3)
        {
            gotchaContentText.text=  " B�� ���� 30%\r\nA�� ���� 30%\r\nS�� ���� 30%\r\n�Һ� ������ 10%\r\n�̳׶� 1000~5000"; //���
            acceptByMoneyText.text = "�̳׶�\n100000";
            acceptByCouponText.text = "��޻̱��\n1";
        }
    }

    private void AcceptBtn(bool result, int paidType)
    {
        OnConfirm(result);
        paidTypeCode = paidType;
    }
}
