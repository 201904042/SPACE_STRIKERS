using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject sandbag;
    public GameObject earth_cummon;
    public GameObject CommonSpawnZones;
    
    
    public  int ranEnemy;
    public int cummonpointNum;
    public float maxSpawnDelay;
    public float curSpawnDelay;
    public bool battleStart;

    public string StageName;
    public string StageGain;

    private Transform[] CommonSpawnPoints;

    private int stage;

    

    private void Awake()
    {
        stage = 0;
        maxSpawnDelay = 4f;
        curSpawnDelay = 4f;
        cummonpointNum = 11;
        battleStart = false;
        CommonSpawnPoints = new Transform[cummonpointNum];
        spawnPointSet();
    }
    private void Update()
    {
        if (battleStart)
        {
            curSpawnDelay += Time.deltaTime;

            if (curSpawnDelay > maxSpawnDelay)
            {
                SpawnEnemy();
                curSpawnDelay = 0;
            }
        }
        
    }

    void spawnPointSet()
    {
        for(int i=0;i < cummonpointNum; i++)
        {
            if(CommonSpawnZones.transform.GetChild(i) == null)
            {
                Debug.Log(i + "¹øÂ°");
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

    void SpawnEnemy()
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
}
