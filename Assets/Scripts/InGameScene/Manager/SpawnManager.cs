using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Pool;


[System.Serializable]
public class SpawnPattern
{
    public int enemyId; // ���� ������ ��Ÿ���� ID
    public int amount; // ������ ���� ��
    public Transform spawnZone;
    public Vector2[] positions; // ���� ������ ��ġ
}

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager spawnInstance;

    [Header("��������")]
    public Transform mainSpawnZone;
    public Transform sideSpawnZoneL;
    public Transform sideSpawnZoneR;
    public Transform bossSpawnZone;
    public int stageEnemyAmount;
    public List<SpawnPattern> spawnPatterns;

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
        SpawnPatternSet();
    }

    private void SpawnPatternSet()
    {
        spawnPatterns = new List<SpawnPattern>()
        {
            //���ν�����
            new SpawnPattern() { 
                enemyId = 1,
                amount = 5,
                spawnZone = mainSpawnZone,
                positions = new Vector2[]
                {
                    new Vector2(-2f, mainSpawnZone.position.y),
                    new Vector2(-1f, mainSpawnZone.position.y),
                    new Vector2(0f, mainSpawnZone.position.y),
                    new Vector2(1f, mainSpawnZone.position.y),
                    new Vector2(2f, mainSpawnZone.position.y)
                }
            },
            new SpawnPattern() {
                enemyId = 2,
                amount = 3,
                spawnZone = mainSpawnZone,
                positions = new Vector2[]
                {
                    new Vector2(-1f, mainSpawnZone.position.y),
                    new Vector2(0f, mainSpawnZone.position.y),
                    new Vector2(1f, mainSpawnZone.position.y)
                }
            },
            new SpawnPattern
            {
                enemyId = 11, 
                amount = 2,
                spawnZone = mainSpawnZone,
                positions = new Vector2[]
                {
                    new Vector2(-2f, mainSpawnZone.position.y),
                    new Vector2(2f, mainSpawnZone.position.y)
                }
            },
            new SpawnPattern
            {
                enemyId = 12, 
                amount = 1,
                spawnZone = mainSpawnZone,
                positions = new Vector2[]
                {
                    new Vector2(0, mainSpawnZone.position.y),
                }
            }

        };

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

    public IEnumerator SpawnCheckCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(maxSpawnDelay);
            SetSpawnType();
        }
    }

    private void SetSpawnType()
    {
        SpawnEnemy();
    }

    private void SpawnEnemy() //���� ��� ��ġ��
    {
        if (StageManager.stageInstance.stage == 0)
        {
            ObjectPool.poolInstance.GetEnemy(0, new Vector3(-2f, 2, 0),Quaternion.identity);
            ObjectPool.poolInstance.GetEnemy(0, new Vector3(0f, 2, 0), Quaternion.identity);
            ObjectPool.poolInstance.GetEnemy(0, new Vector3(2f, 2, 0), Quaternion.identity);
        }

        else if (StageManager.stageInstance.stage >= 1)
        {
            int patternIndex = Random.Range(0, spawnPatterns.Count); // �������� ���� ����
            SpawnPattern selectedPattern = spawnPatterns[patternIndex];

            foreach (Vector2 pos in selectedPattern.positions)
            {
                ObjectPool.poolInstance.GetEnemy(selectedPattern.enemyId, pos,selectedPattern.spawnZone.rotation);
            }
        }
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
