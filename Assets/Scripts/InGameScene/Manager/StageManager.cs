using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager stageInstance;

    [Header("�������� ����")]
    StageData stageData;
    public List<StageEnemyData> stageEnemyList;
    public List<int> stageEnemyIdList;
    public int planet = 1;
    public int stage = 1;


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
        int StageCode = (planet + 1) * 10 + stage;
        stageEnemyList  = new List<StageEnemyData>();
        stageEnemyIdList = new List<int>();   
        isBossStage = false;
        StageDataSet();
        stageData = DataManager.stageData.GetData(StageCode);

        foreach(StageEnemyData enemyInfo in stageData.stageEnemy)
        {
            if(enemyInfo.enemyId > 20)
            {
                stageBossId = enemyInfo.enemyId;
                isBossStage = true;
            }
            stageEnemyIdList.Add(enemyInfo.enemyId);
            stageEnemyList.Add(enemyInfo);
        }
    }

    private void Start()
    {
        stageTime = 0;
    }

    //���� ���������� ������ �����Ϳ��� ������ ���� ������ ����
    private void StageDataSet()
    {

        //foreach (StageData stageData in stageData.stageList.stage)
        //{
        //    if (stageData.stageCode == stageCode)
        //    {
        //        curStageEnemyId = stageData.enemyCode;
        //        curStagefirstGain = stageData.stageFirstGain;
        //        curStageDefaultGain = stageData.stageDefaultGain;
        //        curDefaultFullGain = stageData.defaultFullGain;
        //    }
        //}
    }

    public void StageTimer()
    {
        stageTime += Time.deltaTime;
        minutes = Mathf.FloorToInt(stageTime / 60f);
        seconds = Mathf.FloorToInt(stageTime % 60f);
    }

    //�ӽ� �������� ����. ���Ŀ��� ���� ����.
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
