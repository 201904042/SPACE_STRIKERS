using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageInterface : UIInterface
{
    public Transform StageNamePanel;
    public Transform EnemyListPanel;
    public Transform ContentPanel;
    public Transform Buttons;
    public Button closeBtn;
    public Button acceptBtn;

    private int curStageCode;
    private int curPlanet;
    private int curStage;
    StageData curStageData;

    
    protected override void Awake()
    {
        base.Awake();
    }

    public override void SetComponent()
    {
        base.SetComponent();
        StageNamePanel = transform.GetChild(1);
        EnemyListPanel = transform.GetChild(2);
        ContentPanel = transform.GetChild(3);
        Buttons = transform.GetChild(4);
        closeBtn = Buttons.GetChild(0).GetComponent<Button>();
        acceptBtn = Buttons.GetChild(1).GetComponent<Button>();
    }

    public override IEnumerator GetValue()
    {
        yield return base.GetValue();

        //���� �ʱ�ȭ
        result = null;

        //Ȯ�� ���. ��ư �ڵ鷯 ����
        acceptBtn.onClick.RemoveAllListeners();
        closeBtn.onClick.RemoveAllListeners();
        acceptBtn.onClick.AddListener(() => OnConfirm(true));
        closeBtn.onClick.AddListener(() => OnConfirm(false));

        // ����ڰ� ��ư�� ���� ������ ���
        yield return new WaitUntil(() => result.HasValue);

        CloseInterface();

        //Ȯ���� �������� ��ȯ�� ����
        yield return result.Value;
    }

    public void SetInterface(int stageCode)
    {
        //curStageCode = (parentUI.curPlanet - 1) * 10 + parentUI.curStage;
        Debug.Log(stageCode);
        foreach (StageData data in DataManager.dataInstance.stageData.stageList.stage)
        {
            if (data.stageCode == stageCode)
            {
                curStageData = data;
                break;
            }
        }
        curPlanet = (stageCode / 10) + 1;
        curStage = stageCode % 10;

        StageNamePanel.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"Stage\n{curPlanet} - {curStage}";

        //���� ���� ȹ�� ������ȭ�� ���߿� ���������� text ��� �̹������ ó���Ұ�
        SetEnemyUI();
        SetRewardUI();
    }

    private void SetRewardUI()
    {
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
    }

    private void SetEnemyUI()
    {
        if (curStageData == null)
        {
            return;
        }
        EnemyListPanel.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"���� ��\n{string.Join(", ", curStageData.enemyCode)}";
    }

    private bool CheckIfFirstClear()
    {
        if (curStage <= DataManager.accountData.account.stageProgress)
        {
            return false;
        }
        return true;
    }
}
