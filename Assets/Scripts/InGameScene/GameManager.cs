using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GameManager : StageJsonReader
{
    public GameObject gameEndUI;

    [Header("스테이지 관련")]
    public float stageScore;
    public StageEnemy[] curStageEnemy;
    public Item[] curStagefirstGain;
    public Item[] curStageDefaultGain;
    public Item[] curDefaultFullGain;

    public int planet = 1;
    public int stage=1;
    public int openStage = 5; //현재 열린 스테이지


    public float stageMaxTime = 15;
    public float stageTime;
    public float minutes;
    public float seconds;

    [Header("스폰관련")]
    public GameObject sandbag;
    public GameObject earth_cummon;
    public GameObject CommonSpawnZones;
    public int stageEnemyAmount;
    
    public  int ranEnemy;
    public int cummonpointNum;
    

    public float maxSpawnDelay;
    public float curSpawnDelay;

    public bool isBattleStart;
    public bool isGameClear;
    public bool isPerfectClear;

    private Transform[] CommonSpawnPoints;
    

    protected override void Awake()
    {
        base.Awake ();
        Time.timeScale = 1;
        planet = PlayerPrefs.GetInt("ChosenPlanet");
        stage = PlayerPrefs.GetInt("ChosenStage");
        StageDataSet();
        stageScore = 0;
        stageTime = 0;

        foreach (StageEnemy enemy in curStageEnemy)
        {
            stageEnemyAmount += enemy.enemyAmount;
        }

        maxSpawnDelay = 4f;
        curSpawnDelay = 4f;
        cummonpointNum = 11;
        isBattleStart = false;
        isGameClear = false;
        CommonSpawnPoints = new Transform[cummonpointNum];


        spawnPointSet();
    }
    private void Update()
    {
        if (isBattleStart)
        {
            stageTime += Time.deltaTime;
            minutes = Mathf.FloorToInt(stageTime / 60f);
            seconds = Mathf.FloorToInt(stageTime % 60f);

            curSpawnDelay += Time.deltaTime;

            if (curSpawnDelay > maxSpawnDelay)
            {
                SpawnEnemy();
                curSpawnDelay = 0;
            }

            if (stageEnemyAmount <= 0 || minutes >= 15) //승리조건 만족시 게임 종료
            {
                Time.timeScale = 0;
                isGameClear = true;
                gameEndUI.SetActive(true);
            }
        }
        
    }

    void spawnPointSet()
    {
        for(int i=0;i < cummonpointNum; i++)
        {
            if(CommonSpawnZones.transform.GetChild(i) == null)
            {
                Debug.Log(i + "번째");
            }
            CommonSpawnPoints[i] = CommonSpawnZones.transform.GetChild(i);
            
        }
    }
    public void stageChange()
    {
        deleteEnemy();
        if (stage == 0)
        {
            stage = 1;
        }
        else
        {
            stage = 0;
        }
    }

    void SpawnEnemy() //스폰 요소 고치기
    {
        if(stage == 0)
        {
            SpawnSandbag();
        }
        else if(stage == 1)
        {
            SpawnCommonEnemy();
        }
    }

    void deleteEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<Enemy>().EnemyEliminate();
        }
    }

    void SpawnSandbag()
    {
        deleteEnemy();
        EnemyStat sandbag1 = Instantiate(sandbag, CommonSpawnPoints[3].position - new Vector3(0,2,0),
            CommonSpawnPoints[3].rotation).GetComponent<EnemyStat>();
        EnemyStat sandbag2 = Instantiate(sandbag, CommonSpawnPoints[6].position - new Vector3(0, 2, 0),
            CommonSpawnPoints[6].rotation).GetComponent<EnemyStat>();
        EnemyStat sandbag3 = Instantiate(sandbag, CommonSpawnPoints[9].position - new Vector3(0, 2, 0),
            CommonSpawnPoints[9].rotation).GetComponent<EnemyStat>();

    }
    void SpawnCommonEnemy()
    {
        int ranPoint = Random.Range(0, cummonpointNum);
        EnemyStat enemy = Instantiate(earth_cummon, CommonSpawnPoints[ranPoint].position,
            CommonSpawnPoints[ranPoint].rotation).GetComponent<EnemyStat>();
    }

    void StageDataSet()
    {
        int stageCode = ((planet-1)*10) + stage;
        foreach (StageData stageData in dataContainer.stage)
        {
            if(stageData.stageCode == stageCode)
            {
                curStageEnemy = stageData.stageEnemy;
                curStagefirstGain = stageData.stageFirstGain;
                curStageDefaultGain = stageData.stageDefaultGain;
                curDefaultFullGain = stageData.defaultFullGain;
            }
        }
    }
}
