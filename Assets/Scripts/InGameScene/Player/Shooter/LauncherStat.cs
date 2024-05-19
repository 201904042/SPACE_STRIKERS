using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerjsonReader;

public class LauncherStat : MonoBehaviour
{
    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public PlayerStat player_stat;

    [Header("런쳐 기본 스텟")]
    public GameObject proj_obj;

    public float basic_speed;
    public float shootSpeed;
    public float cur_statspeed;
    public bool Launcher_shootable;

    protected virtual void Awake()
    {
        player = GameObject.Find("Player");
        player_stat = player.transform.GetComponent<PlayerStat>();
        Launcher_shootable = player_stat.is_shootable;

        cur_statspeed = player_stat.attack_speed;
    }

    protected virtual void Update()
    {
        if (player_stat.is_shootable)
        {
            Launcher_shootable = player_stat.is_shootable;
        }
        if (cur_statspeed != player_stat.attack_speed)
        {
            shootSpeed = basic_speed - (player_stat.attack_speed / 100);
        }
    }
}
