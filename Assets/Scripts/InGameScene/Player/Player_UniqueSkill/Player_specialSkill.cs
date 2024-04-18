using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Player_specialSkill : MonoBehaviour
{
    [Header("��ų�� �����յ�")]
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
    public int special_count; //�ΰ��� �������� ��ų�� ����� ��ų ������ ����

    [Header("�Ŀ�����")]
    public  int power_level; //������� �� �ð��� �������� �Ŀ�����(���������� ����� ���� ����)
    public float power_increaseRate; //�Ŀ� ������. ������ ���� �ɷ�ġ�� ����
    public float power_increase;
    public float power_increaseMax;
    [Header("������ ����")]
    public float damage_increaseRate; //����� ��ų�� ������ ������. ������ �����Ƽ�� ���� ����
    public float special_Damage; //������� ������
    public float special_FireTime; //������� ���ӽð�
    [Header("��ų �ߵ���")]
    public bool special_Active;
    public bool first_set;
    

    private void Awake()
    {
        player = GameObject.Find("Player");
        player_stat = player.GetComponent<PlayerStat>();

        player_id = player_stat.cur_playerID;
        special_count = 3; //��ü������ ����Ͽ� �����ϵ��� ��������

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
        
        if(special_FireTime <= 0 && special_Active) //Ư�������� ���۵ǰ� �ִ� 10���� Ư������ ����
        {
            special_Active = false; 
        }

        if (power_increase <= power_increaseMax && !special_Active)
        {
            //�Ŀ��� �ƽ�ġ���� ���ų� ������� ��Ȱ��ȭ�� ��쿡�� �Ŀ��� �ö󰣴�.
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
        switch (player_id) //�÷��̾��� ���̵� ���� �÷��̾� �� ����Ƚ�ų Ȱ��
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

        //��ų Ȱ��ȭ�� �Ŀ����� �ʱ�ȭ
        power_level = 0;
        power_increase = 0;
        special_Active = true;
    }

    private void balanceSpecial()
    {
        int spawnNum = 1+((power_level-1)*2);
        special_FireTime = 3;

        //���� �Ÿ� ����
        float spawnXpos = transform.position.x;
        float spawnYpos = -4.5f;
        float space = 0.75f;
        
        //������ �δ밡 ȭ�� ������ �����°� ����
        if (spawnXpos < -2.5)
        {
            spawnXpos = -2.5f;
        }
        else if (spawnXpos > 2.5)
        {
            spawnXpos = 2.5f;
        }

        //�Ŀ������� ���� ��ȭ(����, Ȱ���ð�, ������ġ)
        if (power_level == 2)
        {
            special_FireTime += 1; //4�ʰ�
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
            special_FireTime += 2; //5�ʰ�
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

    //UI ��ư
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
