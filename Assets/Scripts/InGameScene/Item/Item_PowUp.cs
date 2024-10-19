using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_PowUp : Ingame_Item
{
    PlayerSpecialSkill PlayerSkill;
    protected override void Awake()
    {
        base.Awake();
        PlayerSkill = GameManager.game.myPlayer.GetComponent<PlayerSpecialSkill>();
    }


    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject.CompareTag("Player"))
        {
            Managers.Instance.Pool.ReleasePool(gameObject);
            PowUP();
        }
    }
    void PowUP()
    {
        switch (PlayerSkill.powerLevel)
        {
            case 0:
                PlayerSkill.powerLevel = 1; PlayerSkill.powerIncrease = 5; break;
            case 1:
                PlayerSkill.powerLevel = 2; PlayerSkill.powerIncrease = 15; break;
            case 2:
                PlayerSkill.powerLevel = 3; PlayerSkill.powerIncrease = 30; break;
        }
    }
}
