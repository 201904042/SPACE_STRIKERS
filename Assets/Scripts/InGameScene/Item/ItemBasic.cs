using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBasic : MonoBehaviour
{
    public GameObject player;
    public PlayerStat p_stat;

    private Rigidbody2D i_rigid;
    public float liveTime;

    protected virtual void Awake()
    {
        player = GameObject.Find("Player");
        p_stat = player.GetComponent<PlayerStat>();

        i_rigid = GetComponent<Rigidbody2D>();
        liveTime = 10f;
        ApplyRandomForce();
    }

    protected virtual void Update()
    {
        liveTime -= Time.deltaTime;
        if (liveTime < 0)
        {
            Destroy(gameObject);
        }
    }
    void ApplyRandomForce()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized; //·£´ýÇÑ ¹æÇâ
        float force = 5f;
        i_rigid.AddForce(randomDirection * force, ForceMode2D.Impulse);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Border"))
        {
            if(collision.gameObject.name=="Top"|| collision.gameObject.name == "Bottom")
            {
                i_rigid.velocity = new Vector2(i_rigid.velocity.x, -i_rigid.velocity.y);
            }
            else if (collision.gameObject.name == "Right" || collision.gameObject.name == "Left")
            {
                i_rigid.velocity = new Vector2(-i_rigid.velocity.x, i_rigid.velocity.y);
            }
        }
    }
    
}
