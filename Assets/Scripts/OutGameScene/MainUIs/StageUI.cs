using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class StageUI : MainUIs
{
    public int clearedStageNum;
    public int curStage;
    public int curPlanet;

    private Transform Stages;

    public override IEnumerator SetUI()
    {
        yield return base.SetUI();

        curStage = 0;
        curPlanet = DataManager.account.GetPlanet();
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
        int clearedLastStage = DataManager.account.GetStageProgress();

        int lastPlanet = (clearedLastStage / 10) +1;  //0부터가 아닌 1부터 시작
        int lastStage = (clearedLastStage % 10);

        if(curPlanet > lastPlanet)
        {
            Debug.Log("비정상적인 접근");
            GotoPlanet();
            return;
        }

        if(curPlanet < lastPlanet)
        {
            clearedStageNum = 10;
        }
        else
        {
            clearedStageNum = lastStage;
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
        ChangeUI(OG_UIManager.UIInstance.planetUI);
    }

    public void GotoReady()
    {
        DataManager.account.SetStageValue(curStage);
        ChangeUI(OG_UIManager.UIInstance.readyUI);
    }

    private void OnStageButtonClicked(int stageNumber)
    {
        curStage = stageNumber;
        StageBtnSet(); 
        Stages.GetChild(curStage - 1).GetChild(0).GetComponentInChildren<Image>().color = Color.yellow;
        StageBtnHandler();
    }

    public void StageBtnHandler()
    {
        StartCoroutine(OpenStargeInterface());
        OG_UIManager.stageInteface.SetInterface((curPlanet-1)*10 + curStage);
    }

    private IEnumerator OpenStargeInterface()
    {
        StageInterface stageInterface = OG_UIManager.stageInteface;

        yield return StartCoroutine(stageInterface.GetValue());

        if ((bool)stageInterface.result)
        {
            DataManager.account.SetStageValue(curStage);
            GotoReady();
        }
        else
        {
            Debug.Log("스테이지 취소");
        }
    }

}
