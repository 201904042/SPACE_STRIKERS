using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Player_specialSkill : MonoBehaviour
{
    [Header("스킬의 프리팹들")]
    public GameObject balance_skillPref;
    public GameObject bomber_skillPref;
    public GameObject tanker_skillPref;
    public GameObject splash_skillPref;

    private GameObject player;
    private PlayerStat player_stat;
    private float cur_statDamage;
    private float cur_damageRate;
    private float cur_playerId;


    [HideInInspector]
    public int player_id;
    public int special_count; //인게임 레벨업시 스킬중 스페셜 스킬 증가가 있음

    [Header("파워관련")]
    public  int power_level; //스페셜을 쓴 시간을 기준으로 파워증가(무차별적인 스페셜 난사 방지)
    public float power_increaseRate; //파워 증가율. 파츠나 각종 능력치로 증가
    public float power_increase;
    public float power_increaseMax;
    [Header("데미지 관련")]
    public float damage_increaseRate; //스페셜 스킬의 데미지 증가율. 파츠나 어빌리티에 의해 증가
    public float special_Damage; //스페셜의 데미지
    public float special_FireTime; //스페셜의 지속시간
    [Header("스킬 발동됨")]
    public bool special_Active;
    public bool first_set;
    

    private void Awake()
    {
        player = GameObject.Find("Player");
        player_stat = player.GetComponent<PlayerStat>();

        player_id = player_stat.cur_playerID;
        special_count = 3; //기체레벨에 비례하여 증가하도록 수정예정

        power_level = 0;
        power_increaseRate = 1f;
        power_increase = 0;
        power_increaseMax = 30f;

        damage_increaseRate = 1f;
        special_Damage = player_stat.damage * damage_increaseRate;
        special_FireTime = 10f;

        special_Active = false;

        cur_statDamage = player_stat.damage;
        cur_damageRate = damage_increaseRate;
    }

    private void Update()
    {
        if(!first_set|| (cur_statDamage!= player_stat.damage) 
            || (cur_damageRate!= damage_increaseRate) || cur_playerId != player_id)
        {
            player_id = player_stat.cur_playerID;
            special_Damage = player_stat.damage * damage_increaseRate;
            first_set=  true;
        }
       

        if (special_Active)
        {
            special_FireTime -= Time.deltaTime;
        }
        
        if(special_FireTime <= 0 && special_Active) //특수공격이 시작되고 최대 10초후 특수공격 종료
        {
            special_Active = false; 
        }

        if (power_increase <= power_increaseMax && !special_Active)
        {
            //파워가 맥스치보다 적거나 스페셜이 비활성화일 경우에만 파워가 올라간다.
            power_increase += Time.deltaTime * power_increaseRate;
        }

        PowerLvSet();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (special_count > 0 && !special_Active && power_level != 0)
            {
                special_count--;
                specialFire();
            }
            else
            {
                Debug.Log("cant do specialattack");
            }
            
        }
    }

    private void PowerLvSet()
    {
        if (power_increase > 5 && power_level == 0)
        {
            power_level = 1;
        }
        else if (power_increase > 15 && power_level == 1)
        {
            power_level = 2;
        }
        else if (power_increase >= power_increaseMax && power_level == 2)
        {
            power_level = 3;
        }
    }

    private void specialFire()
    {
        switch (player_id) //플레이어의 아이디에 따라 플레이어 별 스페셜스킬 활성
        {
            case 1:
                balanceSpecial(); break;
            case 2:
                bomberSpecial(); break;
            case 3:
                tankerSpecial(); break;
            case 4:
                splashSpecial(); break;
            default:
                Debug.Log("can't find id"); break;
        }

        //스킬 활성화시 파워레벨 초기화
        power_level = 0;
        power_increase = 0;
        special_Active = true;
    }

    private void balanceSpecial()
    {
        int spawnNum = 1+((power_level-1)*2);
        special_FireTime = 3;

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
        if (power_level == 2)
        {
            special_FireTime += 1; //4초간
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
        else if (power_level == 3)
        {
            special_FireTime += 2; //5초간
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
            Instantiate(balance_skillPref, SpawnPosition, transform.rotation);
        }
    }


    private void bomberSpecial()
    {
        Instantiate(bomber_skillPref, transform.position, transform.rotation);
    }

    private void tankerSpecial()
    {
        special_Active = true;
        Shield shield = GameObject.Find("shield").GetComponent<Shield>();
        shield.shield_curNum = shield.shield_maxNum;
        shield.shield_color_change();
        shield.shield_is_active = true;

        GameObject field = Instantiate(tanker_skillPref, transform);
        if (power_level == 1)
        {
            field.transform.localScale = new Vector3(7f, 7f, 7f);
            special_FireTime = 5;
        }
        else if (power_level == 2)
        {
            field.transform.localScale = new Vector3(10, 10f, 10f);
            special_FireTime = 7;
        }
        else if (power_level == 3)
        {
            field.transform.localScale = new Vector3(15, 15f, 15f);
            special_FireTime = 10;
        }

        StartCoroutine(player3_special_End(special_FireTime, field));

    }

    private IEnumerator player3_special_End(float timer, GameObject field)
    {
        yield return new WaitForSeconds(timer);
        Destroy(field);
    }

    private void splashSpecial()
    {
        special_Active = true;
        int fire_Num = 0;
        if (power_level == 1)
        {
            fire_Num = 10;
        }
        else if (power_level == 2)
        {
            fire_Num = 25;
        }
        else if (power_level == 3)
        {
            fire_Num = 30;
        }

        StartCoroutine(Player4_SkillActivate(fire_Num));
    }

    private IEnumerator Player4_SkillActivate(int num)
    {
        while (num!=0)
        {
            Instantiate(splash_skillPref, transform.position, transform.rotation);
            num--;
            yield return new WaitForSeconds(0.1f);
        }
        special_Active = false;
    }

    //UI 버튼
    public void btn1()
    {
        power_level = 1;
        specialFire();
    }

    public void btn2()
    {
        power_level = 2;
        specialFire();
    }

    public void btn3()
    {
        power_level = 3;
        specialFire();
    }

}
