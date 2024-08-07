using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using UnityEngine.Timeline;

public class PlayerShoot : MonoBehaviour
{
    //플레이어가 발사하는 모든 발사체의 스크립트
    public GameObject player;
    public  PlayerStat playerStat;
    public GameObject launcher;

    public bool hasHit; //발사체에 중복해서 데미지를 입게되는것 방지

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
