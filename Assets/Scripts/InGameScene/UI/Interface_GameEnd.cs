using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Android;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Random = UnityEngine.Random;


public class Interface_GameEnd : UIInterface
{
    private const int defaultRewardRate = 100;
    private const int defaultPerfactAddRate = 50;
    private const int phase1Clear= 100;
    private const int phase2Clear = 250;
    private const int phase3Clear = 400;
    private const int phase4Clear = 1000;

    public Transform ResultTexts;
    public Transform Content;
    public TextMeshProUGUI StageText;
    public ScrollRect RewardScroll;
    public TextMeshProUGUI RewardText;
    public Transform Buttons;
    public Button GotoMainSceneBtn;
    public Button RestartBtn;
    private GameManager Game => GameManager.Game;
    private StageManager Stage => GameManager.Game.Stage;

    List<StageReward> rewardList = new List<StageReward>();

    private float RewardRate;

    public override void SetComponent()
    {
        ResultTexts = transform.GetChild(2);
        Content = transform.GetChild(3);
        StageText = Content.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        RewardScroll =  Content.GetComponentInChildren<ScrollRect>();
        RewardText = RewardScroll.content.GetComponentInChildren<TextMeshProUGUI>();
        Buttons = Content.GetChild(2);
        GotoMainSceneBtn = Buttons.GetChild(0).GetComponent<Button>();
        RestartBtn = Buttons.GetChild(1).GetComponent<Button>();
        SetButtons();
    }

    private void SetButtons()
    {
        GotoMainSceneBtn.onClick.RemoveAllListeners();
        RestartBtn.onClick.RemoveAllListeners();

        GotoMainSceneBtn.onClick.AddListener(GotoMainScene);
        RestartBtn.onClick.AddListener(ReStartGame);
    }

    private void OnEnable()
    {
        rewardList.Clear();
        RewardRate = PlayerMain.pStat.IG_RewardRate;
        GetRewards();
        SetInterface();
    }

    private async void GetRewards()
    {
        if (!Game.IsClear) //클리어 실패시 보상 없음
        {
            return;
        }

        //현재 스테이지 코드가 완료한 스테이지코드보다 크다면 스테이지코드 업데이트
        if(GameManager.Game.Stage.stageCode > DataManager.account.GetStageProgress())
        {
            DataManager.account.SetStageProgress(GameManager.Game.Stage.stageCode);
        }
        

        if (Stage.curMode == GameMode.Infinite)
        {
            int increase = defaultRewardRate;
            switch(Game.phase) 
            {
                case 2: increase = phase1Clear; break; //클리어 시점에서는 페이즈 2상태
                case 3: increase = phase2Clear; break; //페이즈2 클리어
                case 4: increase = phase3Clear; break; //페이즈3 클리어
                case 5: increase = phase4Clear; break; //페이즈4 클리어
            }

            RewardRate += increase;
        }
        else
        {
            if (Game.IsPerfectClear)
            {
                RewardRate += defaultPerfactAddRate;
            }

        }

        foreach (StageReward reward in Stage.ClearReward)
        {
            if(reward.itemId == 7) //보상이 랜덤 재료일경우 => 추후 분리할것
            {
                for (int i = 0; i < reward.quantity * (RewardRate / 100); i++)
                {
                    List<MasterData> targetDatas = DataManager.master.GetItemsByType(MasterType.Ingredient);
                    int randomInt = Random.Range(0, targetDatas.Count - 1);

                    var existingReward = rewardList.Find(reward => reward.itemId == targetDatas[randomInt].id);

                    if (existingReward != null)
                    {
                        existingReward.quantity += 1;
                    }
                    else
                    {
                        rewardList.Add(new StageReward(targetDatas[randomInt].id, 1));
                    }

                    DataManager.inven.DataAddOrUpdate(targetDatas[randomInt].id, 1);
                }
                continue;
            }

            rewardList.Add(new StageReward(reward.itemId, (int)(reward.quantity * (RewardRate / 100))));
            DataManager.inven.DataAddOrUpdate(reward.itemId, (int)(reward.quantity * (RewardRate / 100)));
        }

        await DataManager.inven.SaveData();
    }

    public void SetInterface()
    {
        SetStageText();
        SetResultText();
        SetRewardText();
    }

    private void SetStageText()
    {
        StageText.text = $"Stage : {Stage.planet}-{Stage.stage}";
    }

    private void SetResultText()
    {
        if (Game.IsClear)
        {
            ResultTexts.GetChild(0).gameObject.SetActive(false);
            ResultTexts.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            ResultTexts.GetChild(0).gameObject.SetActive(true);
            ResultTexts.GetChild(1).gameObject.SetActive(false);
        }
    }


    private void SetRewardText()
    {
        if (!Game.IsClear) //클리어 실패시 보상 없음
        {
            RewardText.text = "전리품이 우주로 날라갔다!!";
            return;
        }
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("획득 아이템");

        foreach (StageReward reward in rewardList)
        {
            MasterData data = DataManager.master.GetData(reward.itemId);
            sb.AppendLine($"{data.name} : {reward.quantity}");
        }

        RewardText.text = sb.ToString();
    }

    private void ReStartGame()
    {
        SceneManager.LoadScene("InGameTest");
    }

    public void GotoMainScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
}