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
    protected Coroutine launcherCoroutine;

    [Header("런쳐 기본 스텟")]
    public GameObject projObj;

    public float basicSpeed;
    public float shootSpeed;
    public float curStatspeed
    {
        get => playerStat.attackSpeed;
        set
        {
            shootSpeed = basicSpeed - (playerStat.attackSpeed / 100);
        }
    }

    public bool LauncherShootable { get => playerControl.isShootable; }

    protected Vector2 fireDirection;

    protected virtual void Awake()
    {
        playerStat = GameManager.gameInstance.myPlayer.GetComponent<PlayerStat>();
        playerControl = GameManager.gameInstance.myPlayer.GetComponent<PlayerControl>();
        fireDirection = transform.up;
    }

    protected virtual void Update()
    {
    }

    protected virtual void Fire()
    {
        //기타 요소 추가(사운드)
    }
}
