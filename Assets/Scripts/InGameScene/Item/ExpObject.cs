using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Exp_object : MonoBehaviour
{
    private int expAmount;
    private float expSpeed;
    private void Awake()
    {
        expAmount = 1;
        expSpeed = 5f;
    }

    private void Update()
    {
        if (PlayerMain.Instance  != null)
        {
            Vector2 direction = PlayerMain.Instance.transform.position - transform.position;
            transform.up = direction;

            Rigidbody2D rigid = transform.GetComponent<Rigidbody2D>();
            rigid.velocity = direction * expSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerStat pstat = collision.GetComponent<PlayerStat>();
            pstat.CurExp = pstat.CurExp + expAmount;
            GameManager.Game.Pool.ReleasePool(gameObject);
        }
    }
}
