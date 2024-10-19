using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Exp_object : MonoBehaviour
{
    private float expAmount;
    private float expSpeed;
    private void Awake()
    {
        expAmount = 1f;
        expSpeed = 5f;
    }

    private void Update()
    {
        if (GameManager.Instance.myPlayer != null)
        {
            Vector2 direction = GameManager.Instance.myPlayer.transform.position - transform.position;
            transform.up = direction;

            Rigidbody2D rigid = transform.GetComponent<Rigidbody2D>();
            rigid.velocity = direction * expSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<PlayerInGameExp>().ExpGet += expAmount;
            GameManager.Instance.Pool.ReleasePool(gameObject);
        }
    }
}
