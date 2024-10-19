using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartUI : MainUIs
{
    private static StartUI instance;
    public static StartUI Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new StartUI();
            }

            return instance;
        }
    }
    public static LoginInterface loginInterface;
    public static TFInterface tfInterface;
    public static AlertInterface alertInterface;
    public static OptionInterface optionInterface;

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
        SetInterfaces();

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
        startBtn.interactable = false;
    }

    private void SetInterfaces()
    {
        loginInterface = transform.parent.GetComponentInChildren<LoginInterface>();
        tfInterface = transform.parent.GetComponentInChildren<TFInterface>();
        alertInterface = transform.parent.GetComponentInChildren<AlertInterface>();
        loginInterface.SetComponent();
        tfInterface.SetComponent();
        alertInterface.SetComponent();
        loginInterface.gameObject.SetActive(false);
        tfInterface.gameObject.SetActive(false);
        alertInterface.gameObject.SetActive(false);
    }

    private void OnChangeState(bool sign)
    {
        Debug.Log("로그인에 변화발생");
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
            loginInterface.ResetAll();
            loginBtnText.text = "LogIn";
            userInformText.text = "LogOut";
            isLogin = false;
        }

        startBtn.interactable = isLogin;
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
            LogOutHandler();
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
        Application.OpenURL("https://naver.com"); //우리 홈페이지로
    }


    //로그아웃버튼
    public void LogOutHandler()
    {
        LogoutDoubleCheck();
    }

    private void LogoutDoubleCheck()
    {
        tfInterface.SetTFContent("정말로 로그아웃 하시겠습니까?");
        StartCoroutine(LogOutTFCheck());
    }

    private IEnumerator LogOutTFCheck()
    {
        TFInterface tFInterface = tfInterface;

        yield return StartCoroutine(tFInterface.GetValue());

        if ((bool)tFInterface.result)
        {
            Auth_Firebase.Instance.LogOut();
        }
        else
        {
            alertInterface.SetAlert($"로그아웃 취소");
        }
    }
}
