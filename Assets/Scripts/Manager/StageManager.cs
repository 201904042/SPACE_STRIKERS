using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.EditorTools;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Header("�������� ����")]
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

        
    }

    /// <summary>
    /// ���������� �ڵ带 �޾� �������� ���� ���� ������ ����Ʈ�� ������Ʈ ��
    /// </summary>
    public void SetStageData()
    {
        planet = PlayerPrefs.GetInt("ChosenPlanet");
        stage = PlayerPrefs.GetInt("ChosenStage");

        int StageCode = (planet + 1) * 10 + stage;

        //foreach (StageData stage in stage.stageList.stage)
        //{
        //    if (stage.stageCode == stageCode)
        //    {
        //        curStageEnemyId = stage.enemyCode;
        //        curStagefirstGain = stage.stageFirstGain;
        //        curStageDefaultGain = stage.stageDefaultGain;
        //        curDefaultFullGain = stage.defaultFullGain;
        //    }
        //}

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


    

    //�ӽ� �������� ����. ���Ŀ��� ���� ����.
    public void StageChange()
    {
        Managers.Instance.Spawn.DeleteEnemy();
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
