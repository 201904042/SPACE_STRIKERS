using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using UnityEngine.Timeline;

public class PlayerProjectile : MonoBehaviour
{
    public  PlayerStat playerStat;

    public bool hasHit; //�߻�ü�� �ߺ��ؼ� �������� �԰ԵǴ°� ����

    protected virtual void Awake()
    {
        playerStat = GameManager.Instance.myPlayer.GetComponent<PlayerStat>();
    }

    protected virtual void OnEnable()
    {

    }

    protected virtual void OnDisable()
    {
         
    }

    protected virtual void Init()
    {
        hasHit = false;
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BulletBorder")
        {
            GameManager.Instance.Pool.ReleasePool(gameObject);
        }
    }
}
