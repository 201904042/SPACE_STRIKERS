using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class EnemyObject : MonoBehaviour
{
    private EnemyJsonReader enemyData;
    private GameManager gameManager;

    public int curEnemyId;
    public string enemyName;
    public string enemyGrade;
    public float enemyMaxHP;
    public float enemyCurHP;
    public float enemyDamage;
    public float enemyMoveSpeed;
    public float enemyAttackSpeed;
    public float expAmount;
    public float scoreAmount;
    public bool enemyMoveAttack;
    public bool enemyIsAiming;

    public bool attackType; //1 : 직선 , 0 : 조준
    public bool attackable;
    public bool isSlow;
    
    public GameObject exp;
    public GameObject enemyHpBar;
    
    private GameObject canvas;
    private GameObject hpBarInstance;
    private Slider hpSlider;
    private RectTransform hpBar;

    public GameObject enemyDamageRate;
    private GameObject damageRateInstance;
    private TextMeshProUGUI damageRateText;

    private float enemySaveHP;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        curEnemyId = 0;
        enemyData = GameObject.Find("DataManager").GetComponent<EnemyJsonReader>();
        if (transform.name == "Enemy_common(Clone)")
        {
            curEnemyId = Random.Range(1, 3);
        }
        else if (transform.name == "EnemyElite(Clone)")
        {
            curEnemyId = Random.Range(3, 5);
        }
        else if (transform.name == "Sandbag")
        {
            curEnemyId = 0;
        }
        setStat(curEnemyId);
        hpBarSet();
    }

    public void setStat(int cur_id)
    {
        foreach (var enemy in enemyData.EnemyList.enemy)
        {
            if (enemy.enemyId == cur_id)
            {
                enemyName = enemy.enemyName;
                enemyGrade = enemy.enemyGrade;
                enemyMoveAttack = enemy.enemyMoveAttack;
                enemyIsAiming = enemy.isEnemyAiming;
                enemyMaxHP = enemy.enemyMaxHp;
                enemyDamage = enemy.enemyDamage;
                enemyMoveSpeed = enemy.enemyMoveSpeed;
                enemyAttackSpeed = enemy.enemyAttackSpeed;
                expAmount = enemy.enemyExpAmount;
                scoreAmount = enemy.enemyScoreAmount;
                enemyCurHP = enemyMaxHP;
                enemySaveHP = enemyCurHP;
                attackable = true;
                isSlow = false;
            }
        }
    }

    private void hpBarSet()
    {
        canvas = GameObject.Find("Canvas");
        hpBarInstance = Instantiate(enemyHpBar, canvas.transform);
        hpBar = hpBarInstance.GetComponent<RectTransform>();
        hpSlider = hpBarInstance.GetComponent<Slider>();
        hpBar.sizeDelta = new Vector2(transform.localScale.x * 200, hpBar.sizeDelta.y);
        hpBarInstance.SetActive(false);
        

        if (gameObject.name == "sandBag")
        {
            damageRateInstance = Instantiate(enemyDamageRate, canvas.transform);
            damageRateText = damageRateInstance.GetComponent<TextMeshProUGUI>();
            damageRateInstance.SetActive(false);
            damageRateText.text = enemyCurHP.ToString();
        }
    }


    private void Update()
    {
        if (enemyCurHP != enemySaveHP)
        {
            if (!hpBarInstance.activeSelf)
            {
                hpBarInstance.SetActive(true);
            }
            if (gameObject.name == "sandBag"&&!damageRateInstance.activeSelf)
            {
                damageRateInstance.SetActive(true);
            }
            enemySaveHP = enemyCurHP;
        }
        if (enemyCurHP < enemyMaxHP)
        {
            HpBarUpdate();
        }
        
        

        if (enemyCurHP <= 0)
        {
            EnemyDeath();
        }
    }
    
    private void HpBarUpdate() //hp바의 위치를 업데이트
    {
        Vector3 hpBar_pos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y-(transform.localScale.y), 0));
        hpBar.position = hpBar_pos;

        if(damageRateInstance != null)
        {
            Vector3 DamageRate_pos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y - (transform.localScale.y) * 2, 0));
            damageRateInstance.transform.position = DamageRate_pos;
            damageRateText.text = enemyCurHP.ToString();
        }

        hpSlider.value = enemyCurHP/enemyMaxHP;
    }

    public void EnemyEliminate()
    {
        Destroy(hpBar.gameObject);
        Destroy(gameObject);
        if (damageRateInstance != null)
        {
            Destroy(damageRateInstance.gameObject);
        }
    }

    public void EnemyDeath() //적 사망시
    {
        for(int i=0; i<expAmount; i++)
        {
            Instantiate(exp, transform.position, transform.rotation); //생성되는 위치가 조금씩 차이가 나도록 바꿔보자
        }

        gameManager.stageScore += scoreAmount;
        
        Destroy(hpBar.gameObject);
        Destroy(gameObject);
        if(damageRateInstance != null)
        {
            Destroy(damageRateInstance.gameObject);
        }
        
        //스코어 증가 코드 추가할것
    }

    public void EnemyDamaged(float damage, GameObject attackObj)
    {
        enemyCurHP -= damage;
        Debug.Log(attackObj + " 에 의해 " + damage + " 의 데미지를 입음");
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerStat>().PlayerDamaged(enemyDamage / 2, gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "BulletBorder")
        {
            EnemyEliminate();
        }
    }
}
