using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherStat : MonoBehaviour
{
    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public PlayerStat playerStat;

    [Header("런쳐 기본 스텟")]
    public GameObject projObj;

    public float basicSpeed;
    public float shootSpeed;
    public float curStatspeed;
    public bool LauncherShootable;

    protected virtual void Awake()
    {
        player = GameObject.Find("Player");
        playerStat = player.transform.GetComponent<PlayerStat>();
        LauncherShootable = playerStat.isShootable;

        curStatspeed = playerStat.attackSpeed;
    }

    protected virtual void Update()
    {
        if (playerStat.isShootable)
        {
            LauncherShootable = playerStat.isShootable;
        }
        if (curStatspeed != playerStat.attackSpeed)
        {
            shootSpeed = basicSpeed - (playerStat.attackSpeed / 100);
        }
    }
}
