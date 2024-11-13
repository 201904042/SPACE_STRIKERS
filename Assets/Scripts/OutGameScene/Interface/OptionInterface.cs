using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Android;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionInterface : UIInterface
{
    public bool is_optionOn;

    public Button CancelBtn;
    public Transform Buttons;
    public Button HomePageBtn;
    public Button StartSceneBtn;
    public Button ShutDownBtn;
    public Button MainSceneBtn;


    protected override void Awake()
    {
        base.Awake();
        
    }

    public override void SetComponent()
    {
        base.SetComponent();
        CancelBtn = transform.GetChild(2).GetComponent<Button>();
        Buttons = transform.GetChild(3);
        HomePageBtn =Buttons.GetChild(0).GetComponent<Button>();
        MainSceneBtn = Buttons.GetChild(1).GetComponent<Button>();
        StartSceneBtn = Buttons.GetChild(2).GetComponent<Button>();
        ShutDownBtn = Buttons.GetChild(3).GetComponent<Button>();

        SetBtnHandler();
    }

    private void SetBtnHandler()
    {
        CancelBtn.onClick.RemoveAllListeners();
        foreach (Button button in Buttons.GetComponentsInChildren<Button>())
        {
            button.onClick.RemoveAllListeners();
        }

        CancelBtn.onClick.AddListener(CancelBtnHandler);
        CancelBtn.interactable = true;
        HomePageBtn.onClick.AddListener(HomePageBtnHandler);
        HomePageBtn.interactable = true;
        StartSceneBtn.onClick.AddListener(StartSceneBtnHandler);
        if (SceneManager.GetActiveScene().name == "LogInScene")
        {
            StartSceneBtn.interactable = false;
            
        }
        else
        {
            StartSceneBtn.interactable = true;
        }
        ShutDownBtn.onClick.AddListener(ShutDownBtnHandler);
        ShutDownBtn.interactable = true;
        MainSceneBtn.onClick.AddListener(MainSceneBtnHandler);
        if (SceneManager.GetActiveScene().name == "LogInScene" || SceneManager.GetActiveScene().name == "MainMenu")
        {
            MainSceneBtn.interactable = false;
        }
        else
        {
            MainSceneBtn.interactable = true;
        }
    }

    public void CancelBtnHandler()
    {
        CloseInterface();
    }

    public void HomePageBtnHandler()
    {
        Application.OpenURL("https://naver.com"); //快府 权其捞瘤肺
    }

    public void StartSceneBtnHandler()
    {
        SceneManager.LoadScene("LogInScene");
    }

    public void ShutDownBtnHandler()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application,Quit();
#endif

    }

    public void MainSceneBtnHandler()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
