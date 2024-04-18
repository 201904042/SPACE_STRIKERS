using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player_pow_Bar : MonoBehaviour
{
    GameObject player;
    Player_specialSkill player_specialScr;
    Slider hp_bar;
    Transform fill;
    Image fill_image;
    TextMeshProUGUI pow_text;

    private int power_level;
    private float player_max_time;
    private float cur_player_time;
    private void Awake()
    {
        player = GameObject.Find("Player");
        player_specialScr = player.GetComponent<Player_specialSkill>();
        fill = transform.GetChild(1).GetChild(0);
        fill_image = fill.GetComponent<Image>();
        pow_text = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        power_level = player_specialScr.power_level;
        player_max_time = player_specialScr.power_increaseMax;
        cur_player_time = player_specialScr.power_increase;
        hp_bar = transform.GetComponent<Slider>();

    }

    private void Update()
    {
        power_level = player_specialScr.power_level;
        player_max_time = player_specialScr.power_increaseMax;
        cur_player_time = player_specialScr.power_increase;

        hp_bar.value = cur_player_time / player_max_time;
        if(power_level == 0)
        {
            fill_image.color = Color.white;
            pow_text.text = "POW Lv 0";
        }
        else if(power_level == 1)
        {
            fill_image.color = Color.green;
            pow_text.text = "POW Lv 1";
        }
        else if (power_level == 2)
        {
            fill_image.color = Color.yellow;
            pow_text.text = "POW Lv 2";
        }
        else if (power_level == 3)
        {
            fill_image.color = Color.red;
            pow_text.text = "POW Lv MAX";
        }

    }
}
