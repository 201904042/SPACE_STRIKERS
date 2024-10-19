using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item_ShooterUp : Ingame_Item
{
    private playerShooterUpgrade playerShooter;
    protected override void Awake()
    {
        base.Awake();
        playerShooter = GameManager.game.myPlayer.transform.GetChild(0).GetComponent<playerShooterUpgrade>();
    }
    

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject.CompareTag("Player"))
        {
            Managers.Instance.Pool.ReleasePool(gameObject);
            playerShooter.ShooterUPBtn();
        }
    }
}
