using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialUp : ItemBasic
{
    private PlayerSpecialSkill playerSpecial;
    protected override void Awake()
    {
        base.Awake();
        playerSpecial = player.GetComponent<PlayerSpecialSkill>();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject.CompareTag("Player"))
        {
            playerSpecial.specialCount++;
        }
    }
}
