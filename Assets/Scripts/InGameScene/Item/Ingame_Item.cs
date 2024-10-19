using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingame_Item : MonoBehaviour
{
    public PlayerStat playerStat;

    private Rigidbody2D itemRigid;
    public float liveTime = 10f;

    public bool liveTimeOut;
    private Coroutine liveTimeCoroutine;

    protected virtual void Awake()
    {
        playerStat = GameManager.game.myPlayer.GetComponent<PlayerStat>();
        itemRigid = GetComponent<Rigidbody2D>();
        liveTime = 10f;
    }

    private void OnEnable()
    {
        liveTimeOut = false;

        ApplyRandomForce();

        if (liveTimeCoroutine != null)
        {
            StopCoroutine(liveTimeCoroutine);
        }
        liveTimeCoroutine = StartCoroutine(LiveTimeCoroutine());
    }

    private void OnDisable()
    {
        if (liveTimeCoroutine != null)
        {
            StopCoroutine(liveTimeCoroutine);
        }
    }

    private IEnumerator LiveTimeCoroutine()
    {

        yield return new WaitForSeconds(liveTime);

        //æ∆¿Ã≈€ º“∏Í
        liveTimeOut = true;
    }

    private void ApplyRandomForce()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float force = 5f;
        itemRigid.AddForce(randomDirection * force, ForceMode2D.Impulse);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Border"))
        {
            if (liveTimeOut)
            {
                Managers.Instance.Pool.ReleasePool(gameObject);
            }
            else
            {
                if (collision.gameObject.name == "Top" || collision.gameObject.name == "Bottom")
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
}
