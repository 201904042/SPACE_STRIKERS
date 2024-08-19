using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowUp : ItemBasic
{
    PlayerSpecialSkill PlayerSkill;
    protected override void Awake()
    {
        base.Awake();
        PlayerSkill = GameManager.gameInstance.myPlayer.GetComponent<PlayerSpecialSkill>();
    }


    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject.CompareTag("Player"))
        {
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
