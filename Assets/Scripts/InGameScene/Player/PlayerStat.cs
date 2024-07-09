using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    
    private PlayerJsonReader playerData;
    [Header("기본 정보")]
    public int curPlayerID;
    public int level;
    public bool isFirstSetDone;

    [Header("초기스텟 + 패시브 스텟 적용 \n 각 요소에 직접 참조됨")]
    public float damage; //추가스텟 적용 이후 공격 혹은 이동에 직접 쓰일 변수(패시브 스킬이 추가된 스텟)
    public float defence;
    public float moveSpeed;
    public float attackSpeed;
    public float hp;

    public float cur_hp;

    [Header("초기스텟(기본+파츠스텟+기체레벨스텟)")]
    private float initDamage; //추가스텟이 적용되지 않은 기본의 스텟 변수(기체 레벨 추가스텟 + 파츠 레벨이 적용됨)
    private float initDefence;
    private float initMoveSpeed;
    private float initAttackSpeed;
    private float initHp;

    [Header("패시브 증가율")]
    public float damageIncreaseRate;
    public float defenceIncreaseRate;
    public float moveSpeedIncreaseRate;
    public float attackSpeedIncreaseRate;
    public float hpRegenRate;

    [Header("상태")]
    public bool isShootable;
    public bool isHitted;

    private PlayerControl p_control;

    private void Awake()
    {
        p_control = GameObject.Find("Player").GetComponent<PlayerControl>();
        playerData = GameObject.Find("DataManager").GetComponent<PlayerJsonReader>();
        isFirstSetDone = false;
        isShootable = false;
        isHitted = false;
        curPlayerID =1;
        SetStat(curPlayerID);
    }
    private void Update()
    {
        if (GameManager.gameInstance.isBattleStart)
        {
            isShootable = GameManager.gameInstance.isBattleStart;
        }
    }
    public void SetStat(int cur_id)
    {
        PlayerSet(cur_id);

        hp = initHp;
        cur_hp = hp;

        damageIncreaseRate = 1;
        defenceIncreaseRate = 1;
        moveSpeedIncreaseRate = 1;
        attackSpeedIncreaseRate = 1;
        hpRegenRate = 0;

        ApplyStat();
    }

    public void PlayerSet(int id)
    {
        foreach (var player in playerData.PlayerList.player)
        {
            if (player.id == id)
            {
                level = player.level;
                initDamage = player.damage;
                initDefence = player.defence;
                initMoveSpeed = player.move_speed;
                initAttackSpeed = player.attack_speed;
                initHp = player.hp;
            }
        }
    }

    public void ApplyStat()
    {
        damage = initDamage * damageIncreaseRate;
        defence = initDefence * defenceIncreaseRate;
        moveSpeed = initMoveSpeed * moveSpeedIncreaseRate;
        attackSpeed = initAttackSpeed * attackSpeedIncreaseRate;
    }

    //플레이어의 가변스텟 변화
    public void PlayerDamaged(float damage, GameObject attackObj)
    {
        if (!p_control.isInvincible)
        {
            isHitted = true;
            float applyDamage = damage * (1 - (defence / 100));
            cur_hp -= applyDamage;
            Debug.Log(attackObj.name + " 에 의해 " + applyDamage + " 의 데미지를 입음");
            p_control.PlayerAttacked(attackObj); //넉백 및 무적 부여
        }
    }
}