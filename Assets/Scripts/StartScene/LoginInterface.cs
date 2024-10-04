using Firebase.Auth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LoginInterface : UIInterface
{

    public Transform InputFields;
    public TMP_InputField emailField;
    public TMP_InputField pwField;

    public Transform Btns;
    public Button cancelBtn;
    public Button logInBtn;
    public Button createAccount; //ȸ������ ��ư

    protected override void Awake()
    {
        base.Awake();
        SetBtnHandler();
    }

    private void OnEnable()
    {
        ResetAll();
    }

    private void SetBtnHandler()
    {
        cancelBtn.onClick.RemoveAllListeners();
        logInBtn.onClick.RemoveAllListeners();
        createAccount.onClick.RemoveAllListeners();
        cancelBtn.onClick.AddListener(CloseInterface);
        logInBtn.onClick.AddListener(LoginHandler);
        createAccount.onClick.AddListener(CreateAuthHandler);
    }

    protected override void OnConfirm(bool isConfirmed)
    {
        base.OnConfirm(isConfirmed);
    }

    public override void SetComponent()
    {
        base.SetComponent();
        InputFields = transform.GetChild(1);
        emailField = InputFields.GetChild(0).GetComponentInChildren<TMP_InputField>();
        pwField = InputFields.GetChild(1).GetComponentInChildren<TMP_InputField>();
        Btns = transform.GetChild(2);
        cancelBtn = Btns.GetChild(0).GetComponent<Button>();
        logInBtn = Btns.GetChild(1).GetComponent<Button>();
        createAccount = Btns.GetChild(2).GetComponent<Button>();
    }

    public void ResetAll()
    {
        emailField.text = "";
        pwField.text = "";
    }

    //ȸ������ ��ư
    public void CreateAuthHandler()
    {
        CreateDoubleCheck();
        //Application.OpenURL("https://naver.com"); //Ȩ�������� ȸ������ �������� �̵�
    }

    private void CreateDoubleCheck()
    {
        StartUI.tfInterface.SetTFContent("�ش� ���̵�� ��й�ȣ�� ���̵� �����Ͻðڽ��ϱ�?");
        StartCoroutine(CreateTFCheck());
    }

    private IEnumerator CreateTFCheck()
    {
        TFInterface tFInterface = StartUI.tfInterface;

        if ((bool)tFInterface.result)
        {
            if (string.IsNullOrEmpty(emailField.text) || string.IsNullOrEmpty(pwField.text))
            {
                StartUI.alertInterface.SetAlert("���̵� Ȥ�� ��й�ȣ�� Ȯ���ϼ���");
                yield break;
            }

            Task<bool> createTask = Auth_Firebase.Instance.CreateAccountAsync(emailField.text, pwField.text);
            yield return new WaitUntil(() => createTask.IsCompleted);

            if (createTask.Result)
            {
                ResetAll();
            }
            else
            {
                StartUI.alertInterface.SetAlert("ȸ������ ����");
                ResetAll();
            }
        }
        else
        {
            StartUI.alertInterface.SetAlert("ȸ������ ��ҵ�");
        }
    }


    //�α��� ��ư
    public void LoginHandler()
    {
        LoginDoubleCheck();
    }

    private void LoginDoubleCheck()
    {
        StartUI.tfInterface.SetTFContent("�ش� ���̵�� ��й�ȣ�� �α����Ͻðڽ��ϱ�?");
        StartCoroutine(LoginTFCheck());
    }

    private IEnumerator LoginTFCheck()
    {
        TFInterface tFInterface = StartUI.tfInterface;

        yield return StartCoroutine(tFInterface.GetValue());

        if ((bool)tFInterface.result)
        {
            if (string.IsNullOrEmpty(emailField.text) || string.IsNullOrEmpty(pwField.text))
            {
                StartUI.alertInterface.SetAlert("���̵� Ȥ�� ��й�ȣ�� Ȯ���ϼ���");
                yield break;
            }

            Task<bool> loginTask = Auth_Firebase.Instance.LoginAsync(emailField.text, pwField.text);
            yield return new WaitUntil(() => loginTask.IsCompleted);

            if (loginTask.Result)
            {
                CloseInterface();
            }
            else
            {
                StartUI.alertInterface.SetAlert("�α��� ����");
                ResetAll();
            }
        }
        else
        {
            StartUI.alertInterface.SetAlert("�α����� ��ҵ�");
        }
    }


}
