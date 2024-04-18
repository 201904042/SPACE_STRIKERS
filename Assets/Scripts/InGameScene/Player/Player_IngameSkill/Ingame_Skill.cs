using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Ingame_Skill : MonoBehaviour
{
    private GameObject player;
    private PlayerStat player_stat;

    public GameObject skill_proj;
    [HideInInspector]
    public GameObject[] enemies;

    [Header("인게임 스킬 공통 스텟")]
    public int level;
    public float Stat_Damage;
    public float DamageRate;
    public float coolTime;
    public int proj_num;
    public float timer;

    protected virtual void Awake()
    {
        player = GameObject.Find("Player");
        player_stat = player.GetComponent<PlayerStat>();

        level = transform.GetComponent<skill_interface>().level;
        Stat_Damage = player_stat.damage;
    }
    

    public void findEnemyWithTag()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    public IEnumerator WaitForSecond(float time)
    {
        Debug.Log("stop");
        yield return new WaitForSeconds(time);
    }
}
