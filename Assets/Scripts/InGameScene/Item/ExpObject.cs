using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Exp_object : MonoBehaviour
{
    GameObject player;
    private float exp;
    private float expSpeed;
    private void Awake()
    {
        player = GameObject.Find("Player");
        
        exp = 1f;
        expSpeed = 5f;
    }

    private void Update()
    {
        if (player != null)
        {
            Vector2 direction = player.transform.position - transform.position;
            transform.up = direction;

            Rigidbody2D rigid = transform.GetComponent<Rigidbody2D>();
            rigid.velocity = direction * expSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<PlayerInGameExp>().curExp += exp;
            Destroy(gameObject);
        }
    }
}
