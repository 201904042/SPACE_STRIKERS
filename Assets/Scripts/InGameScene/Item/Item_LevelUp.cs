using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_LevelUp : Ingame_Item
{
    PlayerInGameExp playerExp;
    protected override void Awake()
    {
        base.Awake();
        playerExp = GameManager.Instance.myPlayer.GetComponent<PlayerInGameExp>();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject.CompareTag("Player"))
        {
            Managers.Instance.Pool.ReleasePool(gameObject);
            playerExp.LevelUP();
        }
    }
}
