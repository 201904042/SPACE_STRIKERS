using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartUI : MainUIs
{
    public LoginInterface loginInterface;

    public Image backGround;
    public Button optionBtn;
    public Transform Btns;
    public Button startBtn;
    public Button logInBtn;
    public Button homePageBtn;
    public Button quitBtn;

    public TextMeshProUGUI loginBtnText;
    public TextMeshProUGUI versionText;
    public TextMeshProUGUI userInformText;

    public bool isLogin = false;


    protected override void Awake()
    {
        base.Awake();

        Auth_Firebase.Instance.LoginState += OnChangeState;
        Auth_Firebase.Instance.Init();

        SetButtons();
    }

    
    public override void SetComponent()
    {
        loginInterface = transform.parent.GetComponentInChildren<LoginInterface>();
        backGround = transform.GetChild(0).GetComponent<Image>();
        optionBtn = transform.GetChild(1).GetComponent<Button>();
        Btns = transform.GetChild(2);
        startBtn = Btns.GetChild(0).GetComponent<Button>();
        logInBtn = Btns.GetChild(1).GetComponent<Button>();
        homePageBtn = Btns.GetChild(2).GetComponent<Button>();
        quitBtn = Btns.GetChild(3).GetComponent<Button>();

        loginBtnText = logInBtn.GetComponentInChildren<TextMeshProUGUI>();
        versionText = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        userInformText = transform.GetChild(4).GetComponent<TextMeshProUGUI>(); 
    }

    private void OnChangeState(bool sign)
    {
        if (sign)
        {
            Debug.Log($"로그인이 완료됨 userId : {Auth_Firebase.Instance.UserId}");
            loginInterface.CloseInterface();
            loginBtnText.text = "LogOut";
            userInformText.text = Auth_Firebase.Instance.UserId;
            isLogin = true;
        }
        else
        {
            loginInterface.ResetUI();
            loginBtnText.text = "LogIn";
            userInformText.text = "LogOut";
            isLogin = false;
        }

        startBtn.interactable = Auth_Firebase.Instance.UserId == null ? false : true;
    }

    private void SetButtons()
    {
        for (int i = 0; i < Btns.childCount; i++)
        {
            Btns.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();   
        }

        startBtn.onClick.AddListener(StartBtn);
        logInBtn.onClick.AddListener(LogInBtn);
        homePageBtn.onClick.AddListener(HomePageBtn);
        quitBtn.onClick.AddListener(EndBtn);

        //startBtn.interactable = Auth_Firebase.Instance.UserId == null ? false : true;
    }


    public void StartBtn()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LogInBtn()
    {
        if(!isLogin)
        {
            loginInterface.gameObject.SetActive(true);
        }
        else
        {
            loginInterface.LogOutHandler();
        }
    }
    public void EndBtn()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application,Quit();
        #endif
    }
    public void HomePageBtn()
    {
        Application.OpenURL("https://naver.com");
    }
}
