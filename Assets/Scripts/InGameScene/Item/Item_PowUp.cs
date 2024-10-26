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
            GameManager.Instance.Pool.ReleasePool(gameObject);
            PowUP();
        }
    }
    void PowUP()
    {
        switch (pStat.powerLevel)
        {
            case 0:
                pStat.powerLevel = 1; pStat.curPowerValue = 5; break;
            case 1:
                pStat.powerLevel = 2; pStat.curPowerValue = 15; break;
            case 2:
                pStat.powerLevel = 3; pStat.curPowerValue = 30; break;
        }
    }
}
