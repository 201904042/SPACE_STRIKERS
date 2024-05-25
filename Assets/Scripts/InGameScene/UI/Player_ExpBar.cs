using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerExpBar : MonoBehaviour
{
    GameObject player;
    PlayerInGameExp player_exp;
    Slider exp_bar;

    private float player_max_exp;
    private float cur_player_exp;
    private void Awake()
    {
        player = GameObject.Find("Player");
        player_exp = player.GetComponent<PlayerInGameExp>();
        player_max_exp = player_exp.maxExp;
        cur_player_exp = player_exp.curExp;
        exp_bar = transform.GetComponent<Slider>();

    }

    private void Update()
    {
        player_max_exp = player_exp.maxExp;
        cur_player_exp = player_exp.curExp;
        exp_bar.value = (cur_player_exp / player_max_exp);

    }
}
