using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skill_instantHeal : MonoBehaviour
{
    GameObject player;
    PlayerStat p_stat;

    private float healAmount;
    private float damagedHP;
    private void Awake()
    {
        player = GameObject.Find("Player");
        p_stat = player.GetComponent<PlayerStat>();
        healAmount = p_stat.hp * 0.3f; //힐량은 최대체력의 30%
        damagedHP = p_stat.hp - p_stat.cur_hp;

        instantHeal();
    }

    private void instantHeal()
    {
        if(p_stat.cur_hp < p_stat.hp)
        {
            if(healAmount > damagedHP)
            {
                 healAmount = damagedHP;
            }
            p_stat.cur_hp += healAmount;
        }
        Destroy(gameObject);
    }
}
