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
    public  PlayerStat player_stat;
    [HideInInspector]
    public bool is_firstSet;
    protected virtual void Awake()
    {
        player = GameObject.Find("Player");
        player_stat = player.GetComponent<PlayerStat>();
        is_firstSet = false;
    }
    
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BulletBorder")
        {
            Destroy(gameObject);
        }
    }
}
