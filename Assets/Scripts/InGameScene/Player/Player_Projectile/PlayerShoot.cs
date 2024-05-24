using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using UnityEngine.Timeline;

public class PlayerShoot : MonoBehaviour
{
    //�÷��̾ �߻��ϴ� ��� �߻�ü�� ��ũ��Ʈ
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
