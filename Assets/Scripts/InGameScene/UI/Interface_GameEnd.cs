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


public class Interface_GameEnd : UIInterface
{
    public Transform ResultTexts;
    public Transform Context;
    public TextMeshProUGUI StageText;
    public ScrollRect RewardScroll;
    public TextMeshProUGUI RewardText;
    public Transform Buttons;
    public Button GotoMainSceneBtn;
    public Button RestartBtn;
    private GameManager Game => GameManager.Game;
    private StageManager Stage => GameManager.Game.Stage;

    private int RewardRate;

    public override void SetComponent()
    {
        ResultTexts = transform.GetChild(2);
        Context = transform.GetChild(3);
        StageText = Context.GetChild(0).GetComponent<TextMeshProUGUI>();
        RewardScroll =  Context.GetComponentInChildren<ScrollRect>();
        RewardText = RewardScroll.content.GetComponentInChildren<TextMeshProUGUI>();
        Buttons = Context.GetChild(2);
        GotoMainSceneBtn = Buttons.GetChild(0).GetComponent<Button>();
        RestartBtn = Buttons.GetChild(1).GetComponent<Button>();
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
        GetRewards();
        SetInterface();
    }

    private void GetRewards()
    {
        if (!Game.IsClear)
        {
            return;
        }

        RewardRate = PlayerMain.pStat.IG_RewardRate;

        if (Stage.curMode == GameMode.Infinite)
        {
            float increase = 100;
            switch(Game.phase)
            {
                case 1:  break;
                case 2: increase = 150; break;
                case 3: increase = 250; break;
                case 4: increase = 400; break;
                case 5: increase = 650; break;
                case 6: increase = 1000; break;
            }

            RewardRate = (int)(RewardRate * (increase/100));
        }
        else
        {
            if (Game.IsPerfectClear)
            {
                RewardRate = (int)(RewardRate * 1.5f); //∆€∆Â∆Æ ≈¨∏ÆæÓΩ√ 1.5πË
            }

        }

        foreach (StageReward reward in Stage.ClearReward)
        {
            DataManager.inven.DataAddOrUpdate(reward.itemId, reward.quantity * (RewardRate / 100));
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
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("»πµÊ æ∆¿Ã≈€");

        foreach (StageReward reward in Stage.ClearReward)
        {
            MasterData data = DataManager.master.GetData(reward.itemId);
            sb.AppendLine($"{data.name} : {reward.quantity * (RewardRate / 100)}");
        }

        RewardText.text = sb.ToString();
    }

    private void ReStartGame()
    {
        GameManager.Game.RestartGame();
    }

    public void GotoMainScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
}