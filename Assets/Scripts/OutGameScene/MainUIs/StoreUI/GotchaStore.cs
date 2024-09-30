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
        // TF 인터페이스에서 결과를 기다림
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
        UIManager.tfInterface.SetTFContent("정말로 가챠를 진행하시겠습니까?");
        StartCoroutine(TFCheck(tier,paidCode));
    }

    private IEnumerator TFCheck(int tier, int paidCode)
    {
        TFInterface tFInterface = UIManager.tfInterface;

        yield return StartCoroutine(tFInterface.GetValue());

        if ((bool)tFInterface.result)
        {
            //더블체크 완료시 가챠 실행
            Gotcha(tier, paidCode);
        }
        else
        {
            UIManager.alterInterface.SetAlert($"가챠가 취소되었습니다");
        }
    }

    public void Gotcha(int tier, int paidCod)
    {
        //todo -> 가챠 시스템 추가할것
        //해당 재화를 가지고 있는지 확인

        //가챠를 실행 -> 랜덤으로 생성된 파츠를 인벤토리에 추가

        //(필요시) 생성된 파츠를 보여줌

    }
}
