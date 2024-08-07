using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerExpBar : MonoBehaviour
{
    GameObject player;
    PlayerInGameExp playerExpScr;
    Slider exp_bar;


    private void Awake()
    {
        player = GameObject.Find("Player");
        playerExpScr = player.GetComponent<PlayerInGameExp>();

        exp_bar = transform.GetComponent<Slider>();

    }

    private void Update()
    {
        exp_bar.value = (playerExpScr.curExp / playerExpScr.maxExp);
    }

   
}
