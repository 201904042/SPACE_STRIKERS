using System.Collections;
using UnityEngine;

public class PlayerSpecialSkill : MonoBehaviour
{
    private GameManager gameManager;

    [HideInInspector]
    public int playerId;
    public int specialCount; //�ΰ��� �������� ��ų�� ����� ��ų ������ ����

    private PlayerStat playerStat;
    private float curStatDamage;
    private float curDamageRate;
    private float curPlayerId;

    [Header("��ų�� �����յ�")]
    public GameObject balanceSkillPref;
    public GameObject bomberSkillPref;
    public GameObject tankerSkillPref;
    public GameObject splashSkillPref;

    [Header("�Ŀ�����")]
    public  int powerLevel; //������� �� �ð��� �������� �Ŀ�����(���������� ����� ���� ����)
    public float powerIncreaseRate; //�Ŀ� ������. ������ ���� �ɷ�ġ�� ����
    public float powerIncrease;
    public float powerIncreaseMax;
    [Header("������ ����")]
    public float damageIncreaseRate; //����� ��ų�� ������ ������. ������ �����Ƽ�� ���� ����
    public float specialDamage; //������� ������
    public float specialFireTime; //������� ���ӽð�
    [Header("��ų �ߵ���")]
    public bool specialActive;
    public bool firstSet;
    

    private void Awake()
    {
        playerStat = transform.GetComponent<PlayerStat>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        balanceSkillPref = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player/Player_UniqueSkill/Player1/troop.prefab");
        bomberSkillPref = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player/Player_UniqueSkill/Player2/specialBomb.prefab");
        tankerSkillPref = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player/Player_UniqueSkill/Player3/elecField.prefab");
        splashSkillPref = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player/Player_UniqueSkill/Player4/trackingMissile.prefab");
        playerId = playerStat.curPlayerID;
        specialCount = 3; //��ü������ ����Ͽ� �����ϵ��� ��������

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

            if (specialFireTime <= 0 && specialActive) //Ư�������� ���۵ǰ� �ִ� 10���� Ư������ ����
            {
                specialActive = false;
            }

            if (powerIncrease <= powerIncreaseMax && !specialActive)
            {
                //�Ŀ��� �ƽ�ġ���� ���ų� ������� ��Ȱ��ȭ�� ��쿡�� �Ŀ��� �ö󰣴�.
                powerIncrease += Time.deltaTime * powerIncreaseRate;
            }

            PowerLvSet();

            if (Input.GetKeyDown(KeyCode.Z))
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
        switch (playerId) //�÷��̾��� ���̵� ���� �÷��̾� �� ����Ƚ�ų Ȱ��
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

        //��ų Ȱ��ȭ�� �Ŀ����� �ʱ�ȭ
        powerLevel = 0;
        powerIncrease = 0;
        specialActive = true;
    }

    private void BalanceSpecial()
    {
        int spawnNum = 1+((powerLevel-1)*2);
        specialFireTime = 3;

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
        if (powerLevel == 2)
        {
            specialFireTime += 1; //4�ʰ�
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
            specialFireTime += 2; //5�ʰ�
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
            Instantiate(balanceSkillPref, SpawnPosition, transform.rotation);
        }
    }


    private void BomberSpecial()
    {
        Instantiate(bomberSkillPref, transform.position, transform.rotation);
    }

    private void TankerSpecial()
    {
        specialActive = true;
        Shield shield = GameObject.Find("shield").GetComponent<Shield>();
        shield.shieldCurNum = shield.shieldMaxNum;
        shield.ShieldColorChange();
        shield.shieldIsActive = true;

        GameObject field = Instantiate(tankerSkillPref, transform);
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
        Destroy(field);
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
            Instantiate(splashSkillPref, transform.position, transform.rotation);
            num--;
            yield return new WaitForSeconds(0.1f);
        }
        specialActive = false;
    }

    //UI ��ư
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
