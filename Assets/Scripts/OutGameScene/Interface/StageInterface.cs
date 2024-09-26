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

        //등장 적과 획득 가능재화는 나중에 가능해지면 text 대신 이미지들로 처리할것

        if(curStageData == null)
        {
            return;
        }
        EnemyListPanel.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"등장 적\n{string.Join(", ", curStageData.enemyCode)}";

        bool isFirstClear = CheckIfFirstClear(); // 이 함수는 첫 클리어 여부를 반환하는 함수입니다.

        string rewardText = "획득 가능 재화\n";
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

        // 보상 정보를 UI에 설정합니다.
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
