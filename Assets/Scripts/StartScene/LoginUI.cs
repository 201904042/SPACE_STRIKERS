using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginUI : BasicUI
{
    public GameObject logInBtn;
    public GameObject homePageBtn; //회원가입 버튼

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
        Application.OpenURL("https://naver.com"); //홈페이지의 회원가입 페이지로 이동
    }


}
