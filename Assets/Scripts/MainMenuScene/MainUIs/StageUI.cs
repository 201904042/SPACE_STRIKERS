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
        int accountLastStage = DataManager.accountData.account.stageProgress;

        int lastPlanet = (accountLastStage / 10) +1;  //0���Ͱ� �ƴ� 1���� ����
        int lastStage = (accountLastStage % 10) +1 ;

        if(curPlanet > lastPlanet)
        {
            Debug.Log("���������� ����");
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
            int stageNumber = i + 1; // �������� ��ȣ ���� (1���� ����)
            Button stageButton = Stages.GetChild(i).GetComponentInChildren<Button>();

            if (stageButton != null)
            {
                // AddListener�� ����Ͽ� OnStageButtonClicked �޼��忡 ���� �������� ��ȣ�� ����
                stageButton.onClick.RemoveAllListeners();
                stageButton.onClick.AddListener(() => OnStageButtonClicked(stageNumber));
            }
        }

        Transform Buttons = transform.GetChild(1);
        Buttons.GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
        Buttons.GetChild(0).GetComponent<Button>().onClick.AddListener(GotoPlanet);
    }

    /// <summary>
    /// stage�� ��ư ��
    /// </summary>
    private void StageBtnSet()
    {
        for (int i = 0; i < Stages.childCount; i++)
        {
            int stageNum = i + 1;
            if (stageNum <= clearedStageNum) // clearedStageNum ������ ������������ ���
            {
                Stages.GetChild(i).GetComponent<Button>().interactable = true;
                Stages.GetChild(i).GetChild(0).GetComponentInChildren<Image>().color = Color.green;
            }
            else
            {
                // clearedStageNum ������ ���������鿡 ���� ó��
                

                // ù ��° ��� �������� ���� ���������� Ȱ��ȭ
                if (stageNum == clearedStageNum+1)
                {
                    Stages.GetChild(i).GetComponent<Button>().interactable = true;
                    if (stageNum % 5 == 0)
                    {
                        Stages.GetChild(i).GetChild(0).GetComponentInChildren<Image>().color = Color.red; // 5�� ����� ������
                    }
                    else
                    {
                        Stages.GetChild(i).GetChild(0).GetComponentInChildren<Image>().color = Color.white; // �� �ܿ��� ���
                    }
                }
                else
                {
                    Stages.GetChild(i).GetComponent<Button>().interactable = false; // ������ ������������ ��Ȱ��ȭ
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
        CloseStageInterace();
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
