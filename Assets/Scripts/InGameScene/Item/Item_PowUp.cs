using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_PowUp : Ingame_Item
{
    PlayerStat pStat => PlayerMain.pStat;
    protected override void Awake()
    {
        base.Awake();
    }


    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Game.Pool.ReleasePool(gameObject);
            PowUP();
        }
    }
    void PowUP()
    {
        switch (pStat.IG_curPowerLevel)
        {
            case 0:
                pStat.IG_curPowerLevel = 1; pStat.CurPow = 5; break;
            case 1:
                pStat.IG_curPowerLevel = 2; pStat.CurPow = 15; break;
            case 2:
                pStat.IG_curPowerLevel = 3; pStat.CurPow = 30; break;
        }
    }
}
