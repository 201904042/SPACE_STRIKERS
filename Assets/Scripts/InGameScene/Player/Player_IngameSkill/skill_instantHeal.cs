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
        playerStat =PlayerMain.pStat;
        
    }

    private void OnEnable()
    {
        healAmount = playerStat.IG_Hp * 0.3f; //힐량은 최대체력의 30%

        instantHeal();
    }

    private void instantHeal()
    {
        playerStat.CurHp += healAmount;
        Mathf.Min(playerStat.CurHp, playerStat.IG_Hp);

        gameObject.SetActive(false);
    }
}
