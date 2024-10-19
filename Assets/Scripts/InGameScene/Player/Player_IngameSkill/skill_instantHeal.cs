using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_InstantHeal : MonoBehaviour
{
    PlayerStat playerStat;

    private float healAmount;
    private float damagedHP;
    private void Awake()
    {
        playerStat = GameManager.game.myPlayer.GetComponent<PlayerStat>();
        
    }

    private void OnEnable()
    {
        healAmount = playerStat.maxHp * 0.3f; //힐량은 최대체력의 30%

        instantHeal();
    }

    private void instantHeal()
    {
        playerStat.curHp += healAmount;
        Mathf.Min(playerStat.curHp, playerStat.maxHp);

        gameObject.SetActive(false);
    }
}
