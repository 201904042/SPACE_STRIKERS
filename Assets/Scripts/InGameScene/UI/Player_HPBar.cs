using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_HPBar : MonoBehaviour
{
    GameObject player;
    PlayerStat player_stat;
    Slider hp_bar;

    private float player_max_hp;
    private float cur_player_hp;
    private void Awake()
    {
        player = GameObject.Find("Player");
        player_stat = player.GetComponent<PlayerStat>();
        player_max_hp = player_stat.maxHp;
        cur_player_hp = player_stat.curHp;
        hp_bar = transform.GetComponent<Slider>();
        
    }

    private void Update()
    {
        player_max_hp = player_stat.maxHp;
        cur_player_hp = player_stat.curHp;
        hp_bar.value = cur_player_hp / player_max_hp;

    }
}
