using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager spawnInstance;

    [Header("스폰관련")]
    public GameObject sandbag;
    public GameObject earth_cummon;
    public Transform spawnZoneY;
    public Transform spawnZoneXLeft;
    public Transform spawnZoneXRight;
    public Transform bossSpawnZone;
    public int stageEnemyAmount;

    public int ranEnemy;

    public float maxSpawnDelay;
    public float curSpawnDelay;

    public List<GameObject> activeEnemyList;

    private void Awake()
    {
        if (spawnInstance == null)
        {
            spawnInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        foreach (StageEnemy enemy in StageManager.stageInstance.curStageEnemy)
        {
            stageEnemyAmount += enemy.enemyAmount;
        }

        maxSpawnDelay = 4f;
        curSpawnDelay = 4f;
    }
    public void SpawnCheck()
    {
        curSpawnDelay += Time.deltaTime;

        if (curSpawnDelay > maxSpawnDelay)
        {
            SpawnEnemy();
            curSpawnDelay = 0;
        }
    }
    public void SpawnEnemy() //스폰 요소 고치기
    {
        if (StageManager.stageInstance.stage == 0)
        {
            ObjectPool.poolInstance.GetEnemy(0, new Vector3(-2f, 2, 0),Quaternion.identity);
            ObjectPool.poolInstance.GetEnemy(0, new Vector3(0f, 2, 0), Quaternion.identity);
            ObjectPool.poolInstance.GetEnemy(0, new Vector3(2f, 2, 0), Quaternion.identity);
        }
        else if (StageManager.stageInstance.stage >= 1)
        {
            int enemyId = SelectSpawnEnemy();

            Transform selectedSpawnZone = spawnZoneY; //일단은 스폰존 고정. 기준스폰을 랜덤으로하는 메서드 첨부할것
            Vector2 spawnPosition= new Vector2(Random.Range(-2.5f, 2.5f), selectedSpawnZone.position.y);

            ObjectPool.poolInstance.GetEnemy(enemyId, spawnPosition, selectedSpawnZone.rotation);
        }
    }

    private int SelectSpawnEnemy()
    {
        //생성할 적의 코드를 결정하고 리턴. 이후 적을 스폰하도록
        int randRate = Random.Range(1, 3); // 1은 고정형 일반. 2는 직전형 일반
        
        //일단은 커먼 적만 생성
        return randRate;
    }

    //모든 적들을 시스템으로 사망(보상없음)
    public void DeleteEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<EnemyObject>().EnemyEliminate();
        }
    }

}
