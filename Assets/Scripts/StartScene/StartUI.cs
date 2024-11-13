using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{
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

    private void Awake()
    {
        Managers.Instance.FB_Auth.LoginState += OnChangeState;
    }
    protected void OnEnable()
    {
        // �α��� ���� �ʱ�ȭ
        if (Managers.Instance.FB_Auth.user == null)
        {
            Managers.Instance.FB_Auth.Init();
        }

        // UI �ʱ�ȭ
        SetComponent();
        SetInterfaces();
        SetButtons();

        // �α��� ���� �ݿ�
        UpdateLoginState(Managers.Instance.FB_Auth.user != null);
    }

    private void OnDisable()
    {
        Managers.Instance.FB_Auth.LoginState -= OnChangeState;
    }

    private void OnChangeState(bool sign)
    {
        Debug.Log("�α��� ���°� ����Ǿ����ϴ�.");
        UpdateLoginState(sign); // �α��� ���� ������Ʈ
    }


    // �α��� ���¿� ���� UI�� ������Ʈ�ϴ� �޼���
    private void UpdateLoginState(bool isLoggedIn)
    {
        if (isLoggedIn)
        {
            loginBtnText.text = "LogOut";
            userInformText.text = Managers.Instance.FB_Auth.UserId;
            isLogin = true;
            if(loginInterface.gameObject.activeSelf == true)
            {
                loginInterface.CloseInterface();
            }
            
        }
        else
        {
            loginBtnText.text = "LogIn";
            userInformText.text = "LogOut";
            isLogin = false;
        }

        startBtn.interactable = isLogin;
    }

    


    public void SetComponent()
    {
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
        optionInterface = transform.parent.GetComponentInChildren<OptionInterface>();

        loginInterface.SetComponent();
        tfInterface.SetComponent();
        alertInterface.SetComponent();
        optionInterface.SetComponent();
        
        loginInterface.gameObject.SetActive(false);
        tfInterface.gameObject.SetActive(false);
        alertInterface.gameObject.SetActive(false);
        optionInterface.gameObject.SetActive(false);
    }

    private void SetButtons()
    {
        optionBtn.onClick.RemoveAllListeners();
        optionBtn.onClick.AddListener(OptionBtnHandler);
        for (int i = 0; i < Btns.childCount; i++)
        {
            Btns.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();   
        }


        startBtn.onClick.AddListener(StartBtn);
        logInBtn.onClick.AddListener(LogInBtn);

        //if (Managers.Instance.FB_Auth.user != null)
        //{

        //    loginBtnText.text = "LogOut";
        //    userInformText.text = Managers.Instance.FB_Auth.UserId;
        //    isLogin = true;
        //}
        //else
        //{
        //    loginBtnText.text = "LogIn";
        //    userInformText.text = "LogOut";
        //    isLogin = false;
        //}
        
        homePageBtn.onClick.AddListener(HomePageBtn);
        quitBtn.onClick.AddListener(EndBtn);

    }

    public void OptionBtnHandler()
    {
        optionInterface.OpenInterface();
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
        Application.OpenURL("https://naver.com"); //�츮 Ȩ��������
    }


    //�α׾ƿ���ư
    public void LogOutHandler()
    {
        LogoutDoubleCheck();
    }

    private void LogoutDoubleCheck()
    {
        tfInterface.SetTFContent("������ �α׾ƿ� �Ͻðڽ��ϱ�?");
        StartCoroutine(LogOutTFCheck());
    }

    private IEnumerator LogOutTFCheck()
    {
        TFInterface tFInterface = tfInterface;

        yield return StartCoroutine(tFInterface.GetValue());

        if ((bool)tFInterface.result)
        {
            Managers.Instance.FB_Auth.LogOut();
        }
        else
        {
            alertInterface.SetAlert($"�α׾ƿ� ���");
        }
    }
}
