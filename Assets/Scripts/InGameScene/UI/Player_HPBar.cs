using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_HPBar : MonoBehaviour
{
    PlayerStat player_stat;
    Slider hp_bar;

    private void Awake()
    {
        hp_bar = transform.GetComponent<Slider>();
    }

    private void Start()
    {
        player_stat = GameManager.Instance.myPlayer.GetComponent<PlayerStat>();

    }

    private void Update()
    {
        hp_bar.value = player_stat.curHp / player_stat.maxHp;
    }
}
