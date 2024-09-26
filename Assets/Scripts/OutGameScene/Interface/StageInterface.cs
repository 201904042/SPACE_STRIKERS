using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageInterface : UIInterface
{
    public StageUI parentUI;
    public Transform StageNamePanel;
    public Transform EnemyListPanel;
    public Transform ContentPanel;
    public Transform Buttons;

    private int curStageCode;
    StageData curStageData;

    public void OnEnable()
    {
        Init();
        SetInterface();
    }
    protected override void Awake()
    {
        base.Awake();
    }
    public override void SetComponent()
    {
        base.SetComponent();
        parentUI = FindObjectOfType<StageUI>();
        StageNamePanel = transform.GetChild(1);
        EnemyListPanel = transform.GetChild(2);
        ContentPanel = transform.GetChild(3);
        Buttons = transform.GetChild(4);
    }

    private void Init()
    {
        curStageCode = (parentUI.curPlanet - 1) * 10 + parentUI.curStage;
        foreach (StageData data in DataManager.dataInstance.stageData.stageList.stage)
        {
            if (data.stageCode == curStageCode)
            {
                curStageData = data;
                break;
            }
        }

    }

    public void SetInterface()
    {
        StageNamePanel.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"Stage\n{parentUI.curPlanet} - {parentUI.curStage}";

        //���� ���� ȹ�� ������ȭ�� ���߿� ���������� text ��� �̹������ ó���Ұ�

        if(curStageData == null)
        {
            return;
        }
        EnemyListPanel.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"���� ��\n{string.Join(", ", curStageData.enemyCode)}";

        bool isFirstClear = CheckIfFirstClear(); // �� �Լ��� ù Ŭ���� ���θ� ��ȯ�ϴ� �Լ��Դϴ�.

        string rewardText = "ȹ�� ���� ��ȭ\n";
        if (isFirstClear)
        {
            foreach (var reward in curStageData.stageFirstGain)
            {
                rewardText += $"{reward.itemName} x {reward.itemAmount}\n";
            }
        }
        else
        {
            foreach (var reward in curStageData.stageDefaultGain)
            {
                rewardText += $"{reward.itemName} x {reward.itemAmount}\n";
            }
        }

        // ���� ������ UI�� �����մϴ�.
        ContentPanel.GetChild(0).GetComponent<TextMeshProUGUI>().text = rewardText;

        SetBtnListener();
    }

    private bool CheckIfFirstClear()
    {
        if (parentUI.curStage <= parentUI.clearedStageNum)
        {
            return false;
        }
        return true;
    }

    private void SetBtnListener()
    {
        Buttons.GetChild(0).GetComponent<Button>().onClick.AddListener(DeleteBtn);
        Buttons.GetChild(1).GetComponent<Button>().onClick.AddListener(NextBtn);
    }


    private void DeleteBtn()
    {
        parentUI.CloseStageInterace();
    }

    private void NextBtn()
    {
        parentUI.GotoReady();
    }
}
