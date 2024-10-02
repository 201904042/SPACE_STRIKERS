using Firebase.Auth;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LoginInterface : UIInterface
{
    
    public TMP_InputField emailField;
    public TMP_InputField pwField;

    public Transform Btns;
    public Button cancelBtn;
    public Button logInBtn;
    public Button homePageBtn; //ȸ������ ��ư

    protected override void Awake()
    {
        base.Awake();
        ResetUI();
    }

    
    protected override void OnConfirm(bool isConfirmed)
    {
        base.OnConfirm(isConfirmed);
    }

    public override void SetComponent()
    {
        base.SetComponent();
        emailField = transform.GetChild(1).GetChild(0).GetComponentInChildren<TMP_InputField>();
        pwField = transform.GetChild(1).GetChild(1).GetComponentInChildren<TMP_InputField>();
    }

    public void ResetUI()
    {
        emailField.text = "";
        pwField.text = "";
    }

    public void LoginHandler()
    {
        if(emailField.text == null || pwField.text == null)
        {
            Debug.Log("���̵� Ȥ�� ��й�ȣ�� Ȯ���ϼ���");
        }

        Auth_Firebase.Instance.Login(emailField.text,pwField.text);
    }
   
    public void LogOutHandler()
    {
        Auth_Firebase.Instance.LogOut();
    }

    public void CreateAuthHandler()
    {
        Application.OpenURL("https://naver.com"); //Ȩ�������� ȸ������ �������� �̵�
    }


}
