using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : ItemBasic
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
            playerExp.LevelUP();
        }
    }
}
