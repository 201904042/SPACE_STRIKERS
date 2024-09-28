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
    public GameObject enemyUIPref; //todo-> 나중에 동적으로 경로를 통해 프리팹을 찾도록

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

        //변수 초기화
        result = null;

        //확인 취소. 버튼 핸들러 세팅
        acceptBtn.onClick.RemoveAllListeners();
        closeBtn.onClick.RemoveAllListeners();
        acceptBtn.onClick.AddListener(() => OnConfirm(true));
        closeBtn.onClick.AddListener(() => OnConfirm(false));

        // 사용자가 버튼을 누를 때까지 대기
        yield return new WaitUntil(() => result.HasValue);

        CloseInterface();

        //확인을 눌렀을때 반환할 변수
        yield return result.Value;
    }

    /// <summary>
    /// 해당 인터페이스 초기화
    /// </summary>
    public void SetInterface(int stageCode)
    {
        //curStageCode = (parentUI.curPlanet - 1) * 10 + parentUI.curStage;
        Debug.Log(stageCode);
        curStageData = DataManager.stage.GetData(stageCode);
        
        curPlanet = (stageCode / 10) + 1;
        curStage = stageCode % 10;

        StageNamePanel.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"Stage\n{curPlanet} - {curStage}";

        //등장 적과 획득 가능재화는 나중에 가능해지면 text 대신 이미지들로 처리할것
        SetEnemyUI();
        SetRewardUI();
    }

    private void SetRewardUI()
    {
        bool isFirstClear = CheckIfFirstClear(); // 이 함수는 첫 클리어 여부를 반환하는 함수입니다.

        string rewardText = "획득 가능 재화\n";
        MasterData item = new MasterData();
        if (isFirstClear)
        {
            foreach (StageItemReward reward in curStageData.firstReward)
            {
                item = DataManager.master.GetData(reward.itemId);
                rewardText += $"{item.name} x {reward.quantity}\n";
            }
        }
        else
        {
            foreach (StageItemReward reward in curStageData.defaultReward)
            {
                item = DataManager.master.GetData(reward.itemId);
                rewardText += $"{item.name} x {reward.quantity}\n";
            }
        }

        // 보상 정보를 UI에 설정합니다.
        ContentPanel.GetChild(0).GetComponent<TextMeshProUGUI>().text = rewardText;
    }

    private void SetEnemyUI()
    {
        if (curStageData == null)
        {
            return;
        }

        //기존에 남아있는 프리팹들삭제
        for (int i = enemyScrollContent.childCount - 1; i >= 0; i--)
        {
            Destroy(enemyScrollContent.GetChild(i).gameObject);
        }


        foreach (StageEnemyData enemyData in curStageData.stageEnemy)
        {
            EnemyUI enemyUI = Instantiate(enemyUIPref, enemyScrollContent).GetComponent<EnemyUI>();
            enemyUI.SetEnemyUI(enemyData.enemyId, enemyData.quantity);
        }


        //EnemyListPanel.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"등장 적\n{string.Join(", ", curStageData.enemyCode)}";
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
