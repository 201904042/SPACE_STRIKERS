using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Ingame_Active : MonoBehaviour
{
    private GameObject player;
    private PlayerStat playerStat;
    protected SkillInterface skillInterface;

    [Header("인게임 스킬 공통 스텟")]
    public int level; //스킬의 레벨
    public float Stat_Damage; //플레이어의 공격력
    public float damageRate; //스킬의 계수
    public float coolTime; //스킬의 쿨타임
    public int projNum; //스킬의 발사체 개수
    public bool isLevelUp; //레벨업 UI에서 스킬의 레벨을 바꿔줄때 true로 바꿔주기
    public bool activated;

    protected virtual void Awake()
    {
        player = GameObject.Find("Player");
        playerStat = player.GetComponent<PlayerStat>();
        skillInterface = transform.GetComponent<SkillInterface>();
        level = skillInterface.level;
    }

    protected virtual void OnEnable()
    {

    }

    protected virtual void Update()
    {
        if(level != skillInterface.level)
        {
            level = skillInterface.level;
            isLevelUp = true;
        }
    }

    protected virtual void Init()
    {
        activated = false;
        isLevelUp = false;
        Stat_Damage = playerStat.damage;
    }

    protected virtual void LevelSet(int level)
    {
        Debug.Log($"{gameObject.name} : 스킬 레벨업. 현재레벨 {level} ");
    }

}
