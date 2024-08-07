using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using UnityEngine.Timeline;

public class PlayerShoot : MonoBehaviour
{
    //�÷��̾ �߻��ϴ� ��� �߻�ü�� ��ũ��Ʈ
    public GameObject player;
    public  PlayerStat playerStat;
    public GameObject launcher;

    public bool hasHit; //�߻�ü�� �ߺ��ؼ� �������� �԰ԵǴ°� ����

    protected virtual void Awake()
    {
        player = GameObject.Find("Player");
        playerStat = player.GetComponent<PlayerStat>();
    }

    protected virtual void OnEnable()
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
            ObjectPool.poolInstance.ReleasePool(gameObject);
        }
    }
}
