using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBasic : MonoBehaviour
{
    public GameObject player;
    public PlayerStat playerStat;

    private Rigidbody2D itemRigid;
    public float liveTime;

    protected virtual void Awake()
    {
        player = GameObject.Find("Player");
        playerStat = player.GetComponent<PlayerStat>();

        itemRigid = GetComponent<Rigidbody2D>();
        liveTime = 10f;
        ApplyRandomForce();
    }

    protected virtual void Update()
    {
        liveTime -= Time.deltaTime;
        if (liveTime < 0)
        {
            ObjectPool.poolInstance.ReleasePool(gameObject);
        }
    }
    void ApplyRandomForce()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized; //·£´ýÇÑ ¹æÇâ
        float force = 5f;
        itemRigid.AddForce(randomDirection * force, ForceMode2D.Impulse);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ObjectPool.poolInstance.ReleasePool(gameObject);
        }
        if (collision.gameObject.CompareTag("Border"))
        {
            if(collision.gameObject.name=="Top"|| collision.gameObject.name == "Bottom")
            {
                itemRigid.velocity = new Vector2(itemRigid.velocity.x, -itemRigid.velocity.y);
            }
            else if (collision.gameObject.name == "Right" || collision.gameObject.name == "Left")
            {
                itemRigid.velocity = new Vector2(-itemRigid.velocity.x, itemRigid.velocity.y);
            }
        }
    }
    
}
