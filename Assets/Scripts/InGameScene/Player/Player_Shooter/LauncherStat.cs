using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherStat : MonoBehaviour
{
    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public PlayerStat playerStat;
    protected PlayerControl playerControl;

    [Header("런쳐 기본 스텟")]
    public GameObject projObj;

    public float basicSpeed;
    public float shootSpeed;
    public float curStatspeed;

    public bool LauncherShootable;

    public Vector2 fireDirection;

    protected virtual void Awake()
    {
        playerStat = GameManager.gameInstance.myPlayer.GetComponent<PlayerStat>();
        playerControl = GameManager.gameInstance.myPlayer.GetComponent<PlayerControl>();
        LauncherShootable = playerControl.isShootable;
        curStatspeed = playerStat.attackSpeed;
        fireDirection = transform.up;
    }

    protected virtual void Update()
    {
        if (playerControl.isShootable)
        {
            LauncherShootable = playerControl.isShootable;
        }
        if (curStatspeed != playerStat.attackSpeed)
        {
            shootSpeed = basicSpeed - (playerStat.attackSpeed / 100);
        }
    }

    protected virtual void Fire()
    {
        //기타 요소 추가(사운드)
    }
}
