using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GotchaStore : MonoBehaviour
{
    public Transform gotchas;
    public Button gotcha1;
    public Button gotcha2;
    public Button gotcha3;

    public Button[] gotchaBtns;

    private void Awake()
    {
        gotchas = transform.GetChild(0);
        gotchaBtns = new Button[3];
    }

    private void OnEnable()
    {
        for (int i = 0; i < gotchas.childCount; i++)
        {
            gotchaBtns[i] = gotchas.GetChild(i).GetComponent<Button>();
        }

        for (int i = 0; i < gotchaBtns.Length; i++)
        {
            int code = i+1; 
            gotchaBtns[i].onClick.RemoveAllListeners();
            gotchaBtns[i].onClick.AddListener(() => OpenGotchaUI(code));
        }
    }

    private void OpenGotchaUI(int tier)
    {
        UIManager.gotchaInterface.SetGotchaInterface(tier);
        ActiveGotchaInterface();
    }

    private void ActiveGotchaInterface()
    {
        StartCoroutine(GetGotchaInterface());
    }

    private IEnumerator GetGotchaInterface()
    {
        GotchaInterface gotcha = UIManager.gotchaInterface;
        // TF �������̽����� ����� ��ٸ�
        yield return StartCoroutine(gotcha.GetValue());

        if ((bool)gotcha.result)
        {
            DoubleCheck(gotcha.gotchaTier, gotcha.paidTypeCode);
        }
        else
        {
            gotcha.CloseInterface();
        }
    }

    private void DoubleCheck(int tier, int paidCode)
    {
        UIManager.tfInterface.SetTFContent("������ ��í�� �����Ͻðڽ��ϱ�?");
        StartCoroutine(TFCheck(tier,paidCode));
    }

    private IEnumerator TFCheck(int tier, int paidCode)
    {
        TFInterface tFInterface = UIManager.tfInterface;

        yield return StartCoroutine(tFInterface.GetValue());

        if ((bool)tFInterface.result)
        {
            //����üũ �Ϸ�� ��í ����
            Gotcha(tier, paidCode);
        }
        else
        {
            UIManager.alterInterface.SetAlert($"��í�� ��ҵǾ����ϴ�");
        }
    }

    public void Gotcha(int tier, int paidCod)
    {
        //todo -> ��í �ý��� �߰��Ұ�
        //�ش� ��ȭ�� ������ �ִ��� Ȯ��

        //��í�� ���� -> �������� ������ ������ �κ��丮�� �߰�

        //(�ʿ��) ������ ������ ������

    }
}
