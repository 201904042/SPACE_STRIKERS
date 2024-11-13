using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Interface_Option : UIInterface
{
    public Button EndBtn;
    public Button CancelBtn;

    public override void SetComponent()
    {
        Transform Buttons = transform.GetChild(3).GetChild(0);
        EndBtn = Buttons.GetChild(0).GetComponent<Button>();
        CancelBtn = Buttons.GetChild(1).GetComponent<Button>();


        EndBtn.onClick.RemoveAllListeners();
        CancelBtn.onClick.RemoveAllListeners();
        EndBtn.onClick.AddListener(EndbtnHandler);
        CancelBtn.onClick.AddListener(CancelBtnHandler);
    }

    public void EndbtnHandler()
    {
        GameManager game = GameManager.Game;
        GameManager.Game.Restart();
        game.BattleSwitch = false;
        game.StartCoroutine(game.EndGameSequence());
        CloseInterface();
    }

    public void CancelBtnHandler()
    {
        GameManager.Game.Restart();
        CloseInterface();
    }

}
