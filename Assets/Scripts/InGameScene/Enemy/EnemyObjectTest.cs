using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyObjectTest: MonoBehaviour
{
    public Enemy enemyStat;
    private EnemyJsonReader enemyList;
    private GameManager gameManager;

    private int curEnemyId;
    [HideInInspector]
    public float curHp;
    [HideInInspector]
    public float curHpSave;

    [Header("적 체력바")]
    private GameObject canvas;
    private EnemyObjectTest enemyObject;
    private RectTransform hpBar;
    private GameObject hpBarInstance;
    private Slider hpSlider;
    public GameObject enemyHpBar;

    public GameObject exp;

    private void Awake()
    {
        enemyList = GameObject.Find("DataManager").GetComponent<EnemyJsonReader>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        enemyObject = transform.GetComponent<EnemyObjectTest>();
        canvas = GameObject.Find("Canvas");

        hpBarSet();
    }

    private void Update()
    {
        if(enemyStat.enemyId != curEnemyId)
        {
            SetStat();
            setEnemySprite();
        }
        if (enemyObject.curHp != enemyObject.curHpSave)
        {
            if (!hpBarInstance.activeSelf)
            {
                hpBarInstance.SetActive(true);
            }
            enemyObject.curHpSave = enemyObject.curHp;
        }
        if (enemyObject.curHp < enemyObject.enemyStat.enemyMaxHp)
        {
            HpBarUpdate();
        }
    }

    private void SetStat()
    {
        foreach(Enemy enemy in enemyList.EnemyList.enemy)
        {
            if(enemyStat.enemyId == enemy.enemyId)
            {
                enemyStat.enemyGrade = enemy.enemyGrade;
                enemyStat.enemyName = enemy.enemyName;
                enemyStat.enemyMaxHp = enemy.enemyMaxHp;
                enemyStat.enemyDamage = enemy.enemyDamage;
                enemyStat.enemyMoveSpeed = enemy.enemyMoveSpeed;
                enemyStat.enemyAttackSpeed = enemy.enemyAttackSpeed;
                enemyStat.enemyExpAmount = enemy.enemyExpAmount;
                enemyStat.enemyScoreAmount = enemy.enemyScoreAmount;
                enemyStat.enemyMoveAttack = enemy.enemyMoveAttack;
                enemyStat.isEnemyAiming = enemy.isEnemyAiming;

                curEnemyId = enemyStat.enemyId;
                curHp = enemyStat.enemyMaxHp;
                curHpSave = curHp;
            }
            else
            {
                Debug.Log("enemy setStat error");
            }
        }
    }

    private void setEnemySprite()
    {
        //적의 스프라이트와 크기 지정
    }

    private void hpBarSet()
    {
        hpBarInstance = Instantiate(enemyHpBar, canvas.transform);
        hpBar = hpBarInstance.GetComponent<RectTransform>();
        hpSlider = hpBarInstance.GetComponent<Slider>();
        hpBar.sizeDelta = new Vector2(transform.localScale.x * 200, hpBar.sizeDelta.y);
        hpBarInstance.SetActive(false);

    }
    private void HpBarUpdate() //hp바의 위치를 업데이트
    {
        Vector3 hpBar_pos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y - (transform.localScale.y), 0));
        hpBar.position = hpBar_pos;
        hpSlider.value = enemyObject.curHp / enemyObject.enemyStat.enemyMaxHp;
    }

    private void EnemyExpInstatiate()
    {
        for (int i = 0; i < enemyStat.enemyExpAmount; i++)
        {
            Instantiate(exp, transform.position, transform.rotation); //생성되는 위치가 조금씩 차이가 나도록 바꿔보자
        }
    }
    public void EnemyEliminate() //시스템 적 제거. 적처치 보상 없음
    {

        Destroy(hpBar.gameObject);
        Destroy(gameObject);
    }

    public void EnemyDeath() //적 사망시. 적 처치 보상 있음
    {
        EnemyExpInstatiate();
        AddEnemyScoreToStageScore();
        Destroy(hpBar.gameObject);
        Destroy(gameObject);
    }

    private void AddEnemyScoreToStageScore()
    {
        GameManager.gameInstance.score += enemyStat.enemyScoreAmount;
    }

    public void EnemyDamaged(float damage, GameObject attackObj)
    {
        curHp -= damage;
        Debug.Log(attackObj + " 에 의해 " + damage + " 의 데미지를 입음");
    }
}
