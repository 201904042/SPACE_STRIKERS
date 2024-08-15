using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginUI : BasicUI
{
    public GameObject logInBtn;
    public GameObject homePageBtn; //ȸ������ ��ư

    public TMP_InputField idField;
    public TMP_InputField pwField;

    private string idInput;
    private string pwInput;

    private void Awake()
    {
        
    }

    private void Init()
    {
        idField.text = "";
        pwField.text = "";
        idInput = "";
        pwInput = "";
    }

    public void Login()
    {
        idInput = idField.text;
        pwInput = pwField.text;

        Debug.Log($"id : {idInput}, pw : {pwInput}");
    }

    public void GoToHomePage()
    {
        Application.OpenURL("https://naver.com"); //Ȩ�������� ȸ������ �������� �̵�
    }


}
