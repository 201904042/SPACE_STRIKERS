using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_SpecialUp : Ingame_Item
{
    private PlayerSpecialSkill playerSpecial;
    protected override void Awake()
    {
        base.Awake();
        playerSpecial = GameManager.Instance.myPlayer.GetComponent<PlayerSpecialSkill>();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject.CompareTag("Player"))
        {
            Managers.Instance.Pool.ReleasePool(gameObject);
            playerSpecial.specialCount++;
        }
    }
}
