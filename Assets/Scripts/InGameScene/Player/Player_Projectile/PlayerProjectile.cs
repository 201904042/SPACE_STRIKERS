using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using UnityEngine.Timeline;

public class PlayerProjectile : MonoBehaviour
{
    //�÷��̾ �߻��ϴ� ��� �߻�ü�� ��ũ��Ʈ
    public  PlayerStat playerStat;
    public GameObject launcher;

    public bool hasHit; //�߻�ü�� �ߺ��ؼ� �������� �԰ԵǴ°� ����

    protected virtual void Awake()
    {
        playerStat = GameManager.game.myPlayer.GetComponent<PlayerStat>();
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
            //Destroy(gameObject);
            Managers.Instance.Pool.ReleasePool(gameObject);
        }
    }
}
