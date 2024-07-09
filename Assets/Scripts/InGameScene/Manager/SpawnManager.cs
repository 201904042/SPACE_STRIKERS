using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager spawnInstance;

    [Header("��������")]
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
    public void SpawnEnemy() //���� ��� ��ġ��
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

    //��� ������ �ý������� ���(�������)
    public void deleteEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<EnemyObject>().EnemyEliminate();
        }
    }

    //������ ������ �����
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

    //�ӽ÷� �� ���� �׽�Ʈ
    public void SpawnCommonEnemy()
    {
        float ranPoint = Random.Range(0, 11);
        Vector2 spawnPosition = new Vector2(Random.Range(-2.5f, 2.5f), spawnZoneY.position.y);
        
        EnemyObject enemy = Instantiate(earth_cummon, spawnPosition,
            spawnZoneY.rotation).GetComponent<EnemyObject>();
        enemy.enemyStat.enemyId = Random.Range(1, 3); //������ ������ Ÿ��
    }

    public void SpawnFromPool(ObjectPool<GameObject> pool, Vector3 position)
    {
        GameObject enemy = pool.Get();
        enemy.transform.position = position;
        enemy.transform.rotation = Quaternion.identity;
        enemy.GetComponent<EnemyObject>().enemyStat.enemyId = Random.Range(1, 3); //������ ������ Ÿ��
    }



}
