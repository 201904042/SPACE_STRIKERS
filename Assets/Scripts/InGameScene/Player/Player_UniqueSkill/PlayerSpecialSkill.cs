using System.Collections;
using UnityEngine;

public class PlayerSpecialSkill : MonoBehaviour
{
    private GameManager gameManager;

    [HideInInspector]
    public int playerId;
    public int specialCount; //인게임 레벨업시 스킬중 스페셜 스킬 증가가 있음

    private PlayerStat playerStat;
    private float curStatDamage;
    private float curDamageRate;
    private float curPlayerId;

    [Header("파워관련")]
    public  int powerLevel; //스페셜을 쓴 시간을 기준으로 파워증가(무차별적인 스페셜 난사 방지)
    public float powerIncreaseRate; //파워 증가율. 파츠나 각종 능력치로 증가
    public float powerIncrease;
    public float powerIncreaseMax;
    [Header("데미지 관련")]
    public float damageIncreaseRate; //스페셜 스킬의 데미지 증가율. 파츠나 어빌리티에 의해 증가
    public float specialDamage; //스페셜의 데미지
    public float specialFireTime; //스페셜의 지속시간
    [Header("스킬 발동됨")]
    public bool specialActive;
    public bool firstSet;
    

    private void Awake()
    {
        playerStat = transform.GetComponent<PlayerStat>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerId = playerStat.curPlayerID;
        specialCount = 3; //기체레벨에 비례하여 증가하도록 수정예정

        powerLevel = 0;
        powerIncreaseRate = 1f;
        powerIncrease = 0;
        powerIncreaseMax = 30f;

        damageIncreaseRate = 1f;
        specialDamage = playerStat.damage * damageIncreaseRate;
        specialFireTime = 10f;

        specialActive = false;

        curStatDamage = playerStat.damage;
        curDamageRate = damageIncreaseRate;
    }

    private void Update()
    {
        if (gameManager.isBattleStart)
        {
            if (!firstSet || (curStatDamage != playerStat.damage)
            || (curDamageRate != damageIncreaseRate) || curPlayerId != playerId)
            {
                playerId = playerStat.curPlayerID;
                specialDamage = playerStat.damage * damageIncreaseRate;
                firstSet = true;
            }


            if (specialActive)
            {
                specialFireTime -= Time.deltaTime;
            }

            if (specialFireTime <= 0 && specialActive) //특수공격이 시작되고 최대 10초후 특수공격 종료
            {
                specialActive = false;
            }

            if (powerIncrease <= powerIncreaseMax && !specialActive)
            {
                //파워가 맥스치보다 적거나 스페셜이 비활성화일 경우에만 파워가 올라간다.
                powerIncrease += Time.deltaTime * powerIncreaseRate;
            }

            PowerLvSet();

            
        }
    }

    public void PlayerSkillOn()
    {
        if (specialCount > 0 && !specialActive && powerLevel != 0)
        {
            specialCount--;
            SpecialFire();
        }
        else
        {
            Debug.Log("cant do specialattack");
        }
    }

    private void PowerLvSet()
    {
        if (powerIncrease > 5 && powerLevel == 0)
        {
            powerLevel = 1;
        }
        else if (powerIncrease > 15 && powerLevel == 1)
        {
            powerLevel = 2;
        }
        else if (powerIncrease >= powerIncreaseMax && powerLevel == 2)
        {
            powerLevel = 3;
        }
    }

    private void SpecialFire()
    {
        switch (playerId) //플레이어의 아이디에 따라 플레이어 별 스페셜스킬 활성
        {
            case 1:
                BalanceSpecial(); break;
            case 2:
                BomberSpecial(); break;
            case 3:
                TankerSpecial(); break;
            case 4:
                SplashSpecial(); break;
            default:
                Debug.Log("can't find id"); break;
        }

        //스킬 활성화시 파워레벨 초기화
        powerLevel = 0;
        powerIncrease = 0;
        specialActive = true;
    }

    private void BalanceSpecial()
    {
        int spawnNum = 1+((powerLevel-1)*2);
        specialFireTime = 3;

        //스폰 거리 보정
        float spawnXpos = transform.position.x;
        float spawnYpos = -4.5f;
        float space = 0.75f;
        
        //스폰된 부대가 화면 밖으로 나가는것 방지
        if (spawnXpos < -2.5)
        {
            spawnXpos = -2.5f;
        }
        else if (spawnXpos > 2.5)
        {
            spawnXpos = 2.5f;
        }

        //파워레벨에 따른 변화(개수, 활성시간, 스폰위치)
        if (powerLevel == 2)
        {
            specialFireTime += 1; //4초간
            spawnXpos = transform.position.x - space;
            if (spawnXpos < -2.5)
            {
                spawnXpos = -2.5f;
            }
            else if (transform.position.x + space > 2.5)
            {
                spawnXpos = 3.25f - space * spawnNum;
            }
        }
        else if (powerLevel == 3)
        {
            specialFireTime += 2; //5초간
            spawnXpos = transform.position.x - 2 * space;
            if (spawnXpos < -2.5)
            {
                spawnXpos = -2.5f;
            }
            else if (transform.position.x + 2*space > 2.5)
            {
                spawnXpos = 3.25f - space * spawnNum;
            }
        }

        for (int i = 0; i < spawnNum; i++)
        {
            Vector3 SpawnPosition = new Vector3(spawnXpos + (i * space), spawnYpos, 0f);
            ObjectPool.poolInstance.GetSkill(SkillProjType.Spcial_Player1, SpawnPosition,
                transform.rotation);
        }
    }


    private void BomberSpecial()
    {
        ObjectPool.poolInstance.GetSkill(SkillProjType.Spcial_Player2, transform.position, transform.rotation);
    }

    private void TankerSpecial()
    {
        specialActive = true;
        Shield shield = GameObject.Find("shield").GetComponent<Shield>();
        shield.shieldCurNum = shield.shieldMaxNum;
        shield.ShieldColorChange();
        shield.shieldIsActive = true;

        GameObject field = ObjectPool.poolInstance.GetSkill(SkillProjType.Spcial_Player3, transform.position,transform.rotation);
        field.transform.SetParent(transform);

        if (powerLevel == 1)
        {
            field.transform.localScale = new Vector3(7f, 7f, 7f);
            specialFireTime = 5;
        }
        else if (powerLevel == 2)
        {
            field.transform.localScale = new Vector3(10, 10f, 10f);
            specialFireTime = 7;
        }
        else if (powerLevel == 3)
        {
            field.transform.localScale = new Vector3(15, 15f, 15f);
            specialFireTime = 10;
        }

        StartCoroutine(Bomber_End(specialFireTime, field));

    }

    private IEnumerator Bomber_End(float timer, GameObject field)
    {
        yield return new WaitForSeconds(timer);
        ObjectPool.poolInstance.ReleasePool(field);
    }

    private void SplashSpecial()
    {
        specialActive = true;
        int fire_Num = 0;
        if (powerLevel == 1)
        {
            fire_Num = 10;
        }
        else if (powerLevel == 2)
        {
            fire_Num = 25;
        }
        else if (powerLevel == 3)
        {
            fire_Num = 30;
        }

        StartCoroutine(SplashActivate(fire_Num));
    }

    private IEnumerator SplashActivate(int num)
    {
        while (num!=0)
        {
            ObjectPool.poolInstance.GetSkill(SkillProjType.Spcial_Player4, transform.position, transform.rotation);
            num--;
            yield return new WaitForSeconds(0.1f);
        }
        specialActive = false;
    }

    //UI 버튼
    public void btn1()
    {
        powerLevel = 1;
        SpecialFire();
    }

    public void btn2()
    {
        powerLevel = 2;
        SpecialFire();
    }

    public void btn3()
    {
        powerLevel = 3;
        SpecialFire();
    }

}
