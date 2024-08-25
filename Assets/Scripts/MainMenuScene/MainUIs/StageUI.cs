using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class StageUI : MainUIs
{
    public int clearedStageNum;
    public int curStage;
    public int curPlanet;

    private Transform Stages;


    protected override void OnEnable()
    {
        base.OnEnable();

        curStage = 0;
        curPlanet = PlayerPrefs.GetInt("ChosenPlanet");
        Stages = transform.GetChild(0).GetChild(curPlanet - 1).GetChild(0);
        PlanetsUISet();
        FindMaxStageInData();
        StageBtnSet();
        SetListener();
    }

    private void PlanetsUISet()
    {
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            if (i == curPlanet - 1)
            {
                transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    private void FindMaxStageInData()
    {
        if (curPlanet == 1)
        {
            clearedStageNum = DataManager.dataInstance.accountData.playerAccountList.Account[0].clearedPlanet1Stage;
        }
        else if (curPlanet == 2)
        {
            clearedStageNum = DataManager.dataInstance.accountData.playerAccountList.Account[0].clearedPlanet2Stage;
        }
        else if (curPlanet == 3)
        {
            clearedStageNum = DataManager.dataInstance.accountData.playerAccountList.Account[0].clearedPlanet3Stage;
        }
        else if (curPlanet == 4)
        {
            clearedStageNum = DataManager.dataInstance.accountData.playerAccountList.Account[0].clearedPlanet4Stage;
        }
    }

    private void SetListener()
    {
        for (int i = 0; i < Stages.childCount; i++)
        {
            int stageNumber = i + 1; // 스테이지 번호 설정 (1부터 시작)
            Button stageButton = Stages.GetChild(i).GetComponentInChildren<Button>();

            if (stageButton != null)
            {
                // AddListener를 사용하여 OnStageButtonClicked 메서드에 현재 스테이지 번호를 전달
                stageButton.onClick.RemoveAllListeners();
                stageButton.onClick.AddListener(() => OnStageButtonClicked(stageNumber));
            }
        }

        Transform Buttons = transform.GetChild(1);
        Buttons.GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
        Buttons.GetChild(0).GetComponent<Button>().onClick.AddListener(GotoPlanet);
    }

    /// <summary>
    /// stage의 버튼 셋
    /// </summary>
    private void StageBtnSet()
    {
        for (int i = 0; i < Stages.childCount; i++)
        {
            int stageNum = i + 1;
            if (stageNum <= clearedStageNum) // clearedStageNum 이전의 스테이지들은 녹색
            {
                Stages.GetChild(i).GetComponent<Button>().interactable = true;
                Stages.GetChild(i).GetChild(0).GetComponentInChildren<Image>().color = Color.green;
            }
            else
            {
                // clearedStageNum 이후의 스테이지들에 대한 처리
                

                // 첫 번째 잠금 해제되지 않은 스테이지는 활성화
                if (stageNum == clearedStageNum+1)
                {
                    Stages.GetChild(i).GetComponent<Button>().interactable = true;
                    if (stageNum % 5 == 0)
                    {
                        Stages.GetChild(i).GetChild(0).GetComponentInChildren<Image>().color = Color.red; // 5의 배수면 빨간색
                    }
                    else
                    {
                        Stages.GetChild(i).GetChild(0).GetComponentInChildren<Image>().color = Color.white; // 그 외에는 흰색
                    }
                }
                else
                {
                    Stages.GetChild(i).GetComponent<Button>().interactable = false; // 나머지 스테이지들은 비활성화
                }
            }
        }
    }





    public void GotoPlanet()
    {
        curPlanet = 0;
        curStage = 0;
        ChangeUI(UIManager.UIInstance.PlanetUIObj);
    }
    private void OpenStageInterface()
    {
        PlayerPrefs.SetInt("ChosenStage", curStage);
        OpenInterface(UIManager.UIInstance.StageInterface);
    }

    public void CloseStageInterace()
    {
        CloseInterface(UIManager.UIInstance.StageInterface);
    }

    public void GotoReady()
    {
        ChangeUI(UIManager.UIInstance.ReadyUIObj);
    }

    private void OnStageButtonClicked(int stageNumber)
    {
        curStage = stageNumber;
        StageBtnSet(); 
        Stages.GetChild(curStage - 1).GetChild(0).GetComponentInChildren<Image>().color = Color.yellow;
        OpenStageInterface();
    }

}
