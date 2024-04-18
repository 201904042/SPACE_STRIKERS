using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_exp_bar : MonoBehaviour
{
    GameObject player;
    Player_InGame_Exp player_exp;
    Slider exp_bar;

    private float player_max_exp;
    private float cur_player_exp;
    private void Awake()
    {
        player = GameObject.Find("Player");
        player_exp = player.GetComponent<Player_InGame_Exp>();
        player_max_exp = player_exp.max_exp;
        cur_player_exp = player_exp.cur_exp;
        exp_bar = transform.GetComponent<Slider>();

    }

    private void Update()
    {
        player_max_exp = player_exp.max_exp;
        cur_player_exp = player_exp.cur_exp;
        exp_bar.value = (cur_player_exp / player_max_exp);

    }
}
