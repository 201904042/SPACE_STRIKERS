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
            SpawnSandbag();
        }
        else if (StageManager.stageInstance.stage == 1)
        {
            SpawnCommonEnemy();
        }
    }

    //모든 적들을 시스템으로 사망(보상없음)
    public void deleteEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<EnemyObject>().EnemyEliminate();
        }
    }

    //전투력 측정용 샌드백
    public void SpawnSandbag()
    {
        deleteEnemy();
        EnemyObject sandbag1 = Instantiate(sandbag, new Vector3(-1.5f, 2, 0),
            Quaternion.identity).GetComponent<EnemyObject>();
        EnemyObject sandbag2 = Instantiate(sandbag, new Vector3(0, 2, 0),
            Quaternion.identity).GetComponent<EnemyObject>();
        EnemyObject sandbag3 = Instantiate(sandbag, new Vector3(1.5f, 2, 0),
            Quaternion.identity).GetComponent<EnemyObject>();
    }

    //임시로 적 생성 테스트
    public void SpawnCommonEnemy()
    {
        float ranPoint = Random.Range(0, 11);
        Vector2 spawnPosition = new Vector2(Random.Range(-2.5f, 2.5f), spawnZoneY.position.y);
        
        EnemyObject enemy = Instantiate(earth_cummon, spawnPosition,
            spawnZoneY.rotation).GetComponent<EnemyObject>();
        enemy.enemyStat.enemyId = Random.Range(1, 3); //직전형 고정형 타입
    }

    public void SpawnFromPool(ObjectPool<GameObject> pool, Vector3 position)
    {
        GameObject enemy = pool.Get();
        enemy.transform.position = position;
        enemy.transform.rotation = Quaternion.identity;
        enemy.GetComponent<EnemyObject>().enemyStat.enemyId = Random.Range(1, 3); //직전형 고정형 타입
    }



}
