using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using UnityEngine.Timeline;

public class PlayerShoot : MonoBehaviour
{
    //플레이어가 발사하는 모든 발사체의 스크립트
    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public  PlayerStat playerStat;
    [HideInInspector]
    public bool isFirstSet;
    protected virtual void Awake()
    {
        player = GameObject.Find("Player");
        playerStat = player.GetComponent<PlayerStat>();
        isFirstSet = false;
    }
    
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BulletBorder")
        {
            Destroy(gameObject);
        }
    }
}
