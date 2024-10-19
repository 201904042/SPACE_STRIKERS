using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerExpBar : MonoBehaviour
{
    PlayerInGameExp playerExpScr;
    Slider exp_bar;


    private void Awake()
    {
        exp_bar = transform.GetComponent<Slider>();
    }

    private void Start()
    {
        playerExpScr = GameManager.game.myPlayer.GetComponent<PlayerInGameExp>();
    }

    private void Update()
    {
        exp_bar.value = (playerExpScr.curExp / playerExpScr.maxExp);
    }

   
}
