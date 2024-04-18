using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : ItemBasic
{
    Player_InGame_Exp p_exp;
    protected override void Awake()
    {
        base.Awake();
        p_exp = player.GetComponent<Player_InGame_Exp>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject.CompareTag("Player"))
        {
            p_exp.LevelUP();
            Destroy(gameObject);
        }
    }
}
