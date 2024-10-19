using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.EditorTools;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Header("스테이지 관련")]
    public List<StageEnemyData> stageEnemyList;
    public List<int> stageEnemyIdList;
    public int planet;
    public int stage;

    public float timeForReduceSpawnDelay;

    public bool isBossStage;
    public int stageBossId;


    public void Init()
    {
        stageEnemyList = new List<StageEnemyData>();
        stageEnemyIdList = new List<int>();
        isBossStage = false;

        planet = 0; 
        stage = 1;

        SetStageData();

        Debug.Log("스테이지 초기화 완료");
    }

    /// <summary>
    /// 스테이지의 코드를 받아 스테이지 내의 적의 데이터 리스트를 업데이트 함
    /// </summary>
    public void SetStageData()
    {
        AccountData account = DataManager.account.GetData(0);
        planet = account.planetIndex;
        stage = account.stageIndex;

        int StageCode = (planet-1) * 10 + stage;

        StageData stageData = DataManager.stage.GetData(StageCode);

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


    

    //임시 스테이지 변경. 이후에는 쓸일 없음.
    public void StageChange()
    {
        GameManager.Instance.Spawn.DeleteEnemy();
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
