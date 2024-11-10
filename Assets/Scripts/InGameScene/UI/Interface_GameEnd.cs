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

    private void GetRewards()
    {
        if (!Game.IsClear) //Ŭ���� ���н� ���� ����
        {
            return;
        }

        if (Stage.curMode == GameMode.Infinite)
        {
            int increase = defaultRewardRate;
            switch(Game.phase) 
            {
                case 2: increase = phase1Clear; break; //Ŭ���� ���������� ������ 2����
                case 3: increase = phase2Clear; break; //������2 Ŭ����
                case 4: increase = phase3Clear; break; //������3 Ŭ����
                case 5: increase = phase4Clear; break; //������4 Ŭ����
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
            if(reward.itemId == 7) //������ ���� ����ϰ�� => ���� �и��Ұ�
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
        DataManager.inven.SaveData();
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
        if (!Game.IsClear) //Ŭ���� ���н� ���� ����
        {
            RewardText.text = "����ǰ�� ���ַ� ���󰬴�!!";
            return;
        }
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("ȹ�� ������");

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