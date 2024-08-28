using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{ 
    [Header("기본 정보")]
    public int curPlayerID;
    public int level;
    public bool isFirstSetDone;

    [Header("초기스텟 + 패시브 스텟 적용 \n 각 요소에 직접 참조됨")]
    public float damage; //추가스텟 적용 이후 공격 혹은 이동에 직접 쓰일 변수(패시브 스킬이 추가된 스텟)
    public float defence;
    public float moveSpeed;
    public float attackSpeed;
    public float maxHp;
    public float curHp;

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

    

    private PlayerControl playerController;

    private void Awake()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerControl>();

        Init();
    }

    private void Init()
    {
        isFirstSetDone = false;
        
        curPlayerID = 1;
        SetStat(curPlayerID);
    }

    public void SetStat(int playerId)
    {
        PlayerSet(playerId);

        maxHp = initHp;
        curHp = maxHp;

        //각 스텟의 증가율 : 패시브 스킬이나 아이템적용 등으로 가변
        damageIncreaseRate = 1;
        defenceIncreaseRate = 1;
        moveSpeedIncreaseRate = 1;
        attackSpeedIncreaseRate = 1;
        hpRegenRate = 0;

        ApplyStat();
    }

    /// <summary>
    /// 플레이어 스텟 데이터에서 해당 id의 플레이어의  초기 스텟을 설정
    /// </summary>
    public void PlayerSet(int id)
    {
        /* 캐릭터 데이터베이스로 적용할것
        foreach (var player in DataManager.characterData.characterDic.)
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
        */
    }

    /// <summary>
    /// 초기스텟에 증가율을 곱하여 다른 클래스에 공유될 변수 저장.
    /// </summary>
    public void ApplyStat()
    {
        damage = initDamage * damageIncreaseRate;
        defence = initDefence * defenceIncreaseRate;
        moveSpeed = initMoveSpeed * moveSpeedIncreaseRate;
        attackSpeed = initAttackSpeed * attackSpeedIncreaseRate;
    }

    /// <summary>
    /// 플레이어가 데미지를 받을 경우
    /// </summary>
    public void PlayerDamaged(float damage, GameObject attackObj)
    {
        if (!playerController.isInvincibleState)
        {
            playerController.isHitted = true;
            float applyDamage = damage * (1 - (0.01f * defence));

            curHp -= applyDamage;
            Debug.Log(attackObj.name + " 에 의해 " + applyDamage + " 의 데미지를 입음");
            playerController.PlayerDamagedAction(attackObj); //넉백 및 무적 부여
        }
    }
}