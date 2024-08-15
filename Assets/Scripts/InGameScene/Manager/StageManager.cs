using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager stageInstance;

    [Header("스테이지 관련")]
    private StageJsonReader stageData;

    public int[] curStageEnemyId;
    public Item[] curStagefirstGain;
    public Item[] curStageDefaultGain;
    public Item[] curDefaultFullGain;
    public List<int> useingEnemyId;

    public int planet = 1;
    public int stage = 1;
    public int openStage = 5; //현재 열린 스테이지

    public float stageTime;
    public float minutes;
    public float seconds;

    public float timeForReduceSpawnDelay;

    public bool isBossStage;
    public int stageBossId;

    private void Awake()
    {
        if (stageInstance == null)
        {
            stageInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Init();
    }

    public void Init()
    {
        planet = PlayerPrefs.GetInt("ChosenPlanet");
        stage = PlayerPrefs.GetInt("ChosenStage");
        stageData = DataManager.dataInstance.GetComponent<StageJsonReader>();
        useingEnemyId  = new List<int>();
        isBossStage = false;
        StageDataSet();

        foreach(int enemyId in curStageEnemyId)
        {
            if(enemyId > 20)
            {
                stageBossId = enemyId;
                isBossStage = true;
            }
            useingEnemyId.Add(enemyId);
        }
    }

    private void Start()
    {
        stageTime = 0;
    }

    //현재 스테이지의 정보를 데이터에서 가져와 현재 변수에 세팅
    private void StageDataSet()
    {
        int stageCode = ((planet - 1) * 10) + stage;
        foreach (StageData stageData in stageData.stageList.stage)
        {
            if (stageData.stageCode == stageCode)
            {
                curStageEnemyId = stageData.enemyCode;
                curStagefirstGain = stageData.stageFirstGain;
                curStageDefaultGain = stageData.stageDefaultGain;
                curDefaultFullGain = stageData.defaultFullGain;
            }
        }
    }

    public void StageTimer()
    {
        stageTime += Time.deltaTime;
        minutes = Mathf.FloorToInt(stageTime / 60f);
        seconds = Mathf.FloorToInt(stageTime % 60f);
    }

    //임시 스테이지 변경. 이후에는 쓸일 없음.
    public void StageChange()
    {
        SpawnManager.spawnInstance.DeleteEnemy();
        
        if (stage == 0)
        {
            stage = 1;
        }
        else
        {
            stage = 0;
        }
    }
}
