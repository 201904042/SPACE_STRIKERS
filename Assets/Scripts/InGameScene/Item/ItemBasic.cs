using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBasic : MonoBehaviour
{
    public PlayerStat playerStat;

    private Rigidbody2D itemRigid;
    public float liveTime = 10f; // Default liveTime value

    private Coroutine liveTimeCoroutine;

    protected virtual void Awake()
    {
        playerStat = GameManager.Instance.myPlayer.GetComponent<PlayerStat>();
        itemRigid = GetComponent<Rigidbody2D>();
        liveTime = 10f;
    }

    private void OnEnable()
    {
        ApplyRandomForce();
        // Start the coroutine to handle liveTime
        if (liveTimeCoroutine != null)
        {
            StopCoroutine(liveTimeCoroutine);
        }
        liveTimeCoroutine = StartCoroutine(LiveTimeCoroutine());
    }

    private void OnDisable()
    {
        // Ensure coroutine is stopped if the object is disabled before liveTime expires
        if (liveTimeCoroutine != null)
        {
            StopCoroutine(liveTimeCoroutine);
        }
    }

    private IEnumerator LiveTimeCoroutine()
    {

        yield return new WaitForSeconds(liveTime);
        Debug.Log("return item");
        Managers.Instance.Pool.ReleasePool(gameObject);
    }

    private void ApplyRandomForce()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized; // Random direction
        float force = 5f;
        itemRigid.AddForce(randomDirection * force, ForceMode2D.Impulse);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Managers.Instance.Pool.ReleasePool(gameObject);
        }
        if (collision.gameObject.CompareTag("Border"))
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
