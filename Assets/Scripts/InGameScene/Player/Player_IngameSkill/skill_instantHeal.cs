using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_InstantHeal : MonoBehaviour
{
    GameObject player;
    PlayerStat playerStat;

    private float healAmount;
    private float damagedHP;
    private void Awake()
    {
        player = GameObject.Find("Player");
        playerStat = player.GetComponent<PlayerStat>();
        healAmount = playerStat.hp * 0.3f; //힐량은 최대체력의 30%
        damagedHP = playerStat.hp - playerStat.cur_hp;

        instantHeal();
    }

    private void instantHeal()
    {
        if(playerStat.cur_hp < playerStat.hp)
        {
            if(healAmount > damagedHP)
            {
                 healAmount = damagedHP;
            }
            playerStat.cur_hp += healAmount;
        }
        Destroy(gameObject);
    }
}
