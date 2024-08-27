using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GotchaInterface : MonoBehaviour
{
    private int gotchaGrade = 0; // 1�ϱ� 2�߱� 3���
    public GameObject gotchaInterfaceObj;
    private TextMeshProUGUI ContentsText;
    private TextMeshProUGUI CostText;

    private string curContentsText = " ";
    private string curCostText = " ";


    private void Awake()
    {
        ContentsText = gotchaInterfaceObj.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        CostText = gotchaInterfaceObj.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();

    }

    private void TextSet() 
    {
        ContentsText.text = curContentsText;
        CostText.text = curCostText;
    }

    private void TextbyTier()
    {
        if (gotchaGrade == 1)
        {
            curContentsText = " E�� ���� 40%\r\nD�� ���� 25%\r\nC�� ���� 15%\r\nB�� ���� 10%\r\n�Һ� ������ 10%";
            curCostText = "�̳׶� : 1000\r\n�ϱ� ��í�� : 1";
        }
        else if (gotchaGrade == 2)
        {
            curContentsText = " C�� ���� 35%\r\nB�� ���� 20%\r\nA�� ���� 20%\r\n�Һ� ������ 25%\r\n�̳׶� 1000~2000";
            curCostText = "��� : 25\r\n�߱� ��í�� : 1";
        }
        else if (gotchaGrade == 3)
        {
            curContentsText = " B�� ���� 30%\r\nA�� ���� 30%\r\nS�� ���� 30%\r\n�Һ� ������ 10%\r\n�̳׶� 1000~5000";
            curCostText = "��� : 100\r\n��� ��í�� : 1";
        }
        else {
            Debug.Log("error");
        }
    }

    public void tier3Btn() {
        gotchaGrade = 1;
        TextbyTier();
        TextSet();
        gotchaInterfaceObj.SetActive(true);

    }
    public void tier2Btn()
    {
        gotchaGrade = 2;
        TextbyTier();
        TextSet();
        gotchaInterfaceObj.SetActive(true);

    }
    public void tier1Btn()
    {
        gotchaGrade = 3;
        TextbyTier();
        TextSet();
        gotchaInterfaceObj.SetActive(true);
    }

    public void GotchaBackBtn() {
        gotchaInterfaceObj.SetActive(false);
    }
    public void GotchaActBtn()
    {
        //��í���� �޼���
        Debug.Log(gotchaGrade + "���� ����");
        gotchaInterfaceObj.SetActive(false);
    }
}
