using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_SpecialUp : Ingame_Item
{
    private PlayerStat pStat => PlayerMain.pStat;
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
            pStat.USkillCount++;
        }
    }
}
