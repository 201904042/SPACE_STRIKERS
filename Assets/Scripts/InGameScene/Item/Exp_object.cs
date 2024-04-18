using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Exp_object : MonoBehaviour
{
    GameObject player;
    float exp;
    float exp_speed;
    private void Awake()
    {
        player = GameObject.Find("Player");
        
        exp = 1f;
        exp_speed = 5;
    }

    private void Update()
    {
        if (player != null)
        {
            Vector2 direction = player.transform.position - transform.position;
            transform.up = direction;

            Rigidbody2D rigid = transform.GetComponent<Rigidbody2D>();
            rigid.velocity = direction * exp_speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<Player_InGame_Exp>().cur_exp += exp;
            Destroy(gameObject);
        }
    }
}
