using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StageInterface : UIInterface
{
    public Transform StageNamePanel;

    public Transform EnemyListPanel;
    public Transform enemyScrollContent;
    public GameObject enemyUIPref; //todo-> ���߿� �������� ��θ� ���� �������� ã����

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
        enemyScrollContent = EnemyListPanel.GetComponent<ScrollRect>().content;
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

    /// <summary>
    /// �ش� �������̽� �ʱ�ȭ
    /// </summary>
    public void SetInterface(int stageCode)
    {
        //curStageCode = (parentUI.curPlanet - 1) * 10 + parentUI.curStage;
        Debug.Log(stageCode);
        curStageData = DataManager.stage.GetData(stageCode);
        
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
        MasterData item = new MasterData();
        if (isFirstClear)
        {
            foreach (StageReward reward in curStageData.firstReward)
            {
                item = DataManager.master.GetData(reward.itemId);
                rewardText += $"{item.name} x {reward.quantity}\n";
            }
        }
        else
        {
            foreach (StageReward reward in curStageData.defaultReward)
            {
                item = DataManager.master.GetData(reward.itemId);
                rewardText += $"{item.name} x {reward.quantity}\n";
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

        //������ �����ִ� �����յ����
        for (int i = enemyScrollContent.childCount - 1; i >= 0; i--)
        {
            Destroy(enemyScrollContent.GetChild(i).gameObject);
        }


        foreach (StageEnemyData enemyData in curStageData.stageEnemy)
        {
            EnemyUI enemyUI = Instantiate(enemyUIPref, enemyScrollContent).GetComponent<EnemyUI>();
            enemyUI.SetEnemyUI(enemyData.enemyId, enemyData.quantity);
        }


        //EnemyListPanel.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"���� ��\n{string.Join(", ", curStageData.enemyCode)}";
    }

    private bool CheckIfFirstClear()
    {
        if (curStage <= DataManager.account.GetData(0).stageProgress)
        {
            return false;
        }
        return true;
    }
}
