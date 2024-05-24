using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShooterUp : ItemBasic
{
    private playerShooterUpgrade playerShooter;
    protected override void Awake()
    {
        base.Awake();
        playerShooter = player.transform.GetChild(0).GetComponent<playerShooterUpgrade>();
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
            playerShooter.ShooterUPBtn();
            Destroy(gameObject);
        }
    }
}
