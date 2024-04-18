using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialUp : ItemBasic
{
    private Player_specialSkill p_special;
    protected override void Awake()
    {
        base.Awake();
        p_special = player.GetComponent<Player_specialSkill>();
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
            p_special.special_count++;
            Destroy(gameObject);
        }
    }
}
