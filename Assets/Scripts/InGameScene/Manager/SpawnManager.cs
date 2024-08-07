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
    public void SpawnEnemy() //���� ��� ��ġ��
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

            Transform selectedSpawnZone = spawnZoneY; //�ϴ��� ������ ����. ���ؽ����� ���������ϴ� �޼��� ÷���Ұ�
            Vector2 spawnPosition= new Vector2(Random.Range(-2.5f, 2.5f), selectedSpawnZone.position.y);

            ObjectPool.poolInstance.GetEnemy(enemyId, spawnPosition, selectedSpawnZone.rotation);
        }
    }

    private int SelectSpawnEnemy()
    {
        //������ ���� �ڵ带 �����ϰ� ����. ���� ���� �����ϵ���
        int randRate = Random.Range(1, 3); // 1�� ������ �Ϲ�. 2�� ������ �Ϲ�
        
        //�ϴ��� Ŀ�� ���� ����
        return randRate;
    }

    //��� ������ �ý������� ���(�������)
    public void DeleteEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<EnemyObject>().EnemyEliminate();
        }
    }

}
