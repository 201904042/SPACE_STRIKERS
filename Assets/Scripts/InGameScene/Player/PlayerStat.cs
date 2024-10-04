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
        int savedPlayerId = PlayerPrefs.GetInt("curCharacterCode");
        Debug.Log(savedPlayerId);
        curPlayerID = savedPlayerId + 100;
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
        CharData curPlayerChar = DataManager.character.GetData(id);

        ////아웃게임에서 받아온 캐릭터의 스텟 todo -> 이부분은 특정 데이터베이스를 통해 받아올예정
        //level = curPlayerChar.level;
        //initDamage = curPlayerChar.damage;
        //initDefence = curPlayerChar.defense;
        //initMoveSpeed = curPlayerChar.moveSpeed;
        //initAttackSpeed = curPlayerChar.attackSpeed;
        //initHp = curPlayerChar.hp;

        level = 1;
        initDamage = 10;
        initDefence = 10;
        initMoveSpeed = 10;
        initAttackSpeed = 10;
        initHp = 100;
    }

    /// <summary>
    /// 패시브를 통해 강화된 스텟
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