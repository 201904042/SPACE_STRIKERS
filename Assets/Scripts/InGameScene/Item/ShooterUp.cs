using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShooterUp : ItemBasic
{
    player_ShooterUpgrade p_shooter;
    protected override void Awake()
    {
        base.Awake();
        p_shooter = player.transform.GetChild(0).GetComponent<player_ShooterUpgrade>();
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
            p_shooter.level_UP();
            Destroy(gameObject);
        }
    }
}
