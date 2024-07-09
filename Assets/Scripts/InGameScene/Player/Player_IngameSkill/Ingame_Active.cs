using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Ingame_Active : MonoBehaviour
{
    private GameObject player;
    private PlayerStat playerStat;
    public GameObject skillProj;

    [HideInInspector]
    public GameObject[] enemies;

    [Header("인게임 스킬 공통 스텟")]
    public int level;
    public float Stat_Damage;
    public float DamageRate;
    public float coolTime;
    public int projNum;
    public float timer;

    protected virtual void Awake()
    {
        player = GameObject.Find("Player");
        playerStat = player.GetComponent<PlayerStat>();

        level = transform.GetComponent<SkillInterface>().level;
        Stat_Damage = playerStat.damage;
    }
    
    public void FindEnemyWithTag()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    public IEnumerator WaitForSecond(float time)
    {
        Debug.Log("stop");
        yield return new WaitForSeconds(time);
    }
}
