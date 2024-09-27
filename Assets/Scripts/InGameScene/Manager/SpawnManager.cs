using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;


[System.Serializable]
public struct SpawnPattern
{
    public int enemyId; // ���� ������ ��Ÿ���� ID
    public int amount; // ������ ���� ��
    public Transform spawnZone;
    public Vector2[] positions; // ���� ������ ��ġ
}

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager spawnInstance;

    [Header("Ŀ��, ����Ʈ �� ���� ����")]
    //���� ��ġ
    public Transform mainSpawnZone;
    public Transform sideSpawnZoneL;
    public Transform sideSpawnZoneR;
   
    //������ ���ϵ�
    public List<SpawnPattern> spawnPatterns;
    //�� ������������ ���� ������ ����
    public List<SpawnPattern> canSpawnList;

    private int ranEnemy; //������ ��

    public List<GameObject> activeEnemyList; //���������� ���� ����Ʈ

    public Transform bossSpawnZone;
    public bool isBossSpawned; //������ �����Ǿ�����
    public bool isBossDown; //������ ������ óġ�Ǿ�����


    private int stopIndex = 1; // ���� �� ������ ���� stopCount ���� �����ϴ� ����

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
        canSpawnList = new List<SpawnPattern>();
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
            },
            //���̵� ������
            new SpawnPattern
            {
                enemyId = 2,
                amount = 3,
                spawnZone = sideSpawnZoneL,
                positions = new Vector2[]
                {
                    new Vector2(sideSpawnZoneL.position.x-1, sideSpawnZoneL.position.y),
                    new Vector2(sideSpawnZoneL.position.x, sideSpawnZoneL.position.y),
                    new Vector2(sideSpawnZoneL.position.x+1, sideSpawnZoneL.position.y),
                }
            },
             new SpawnPattern
            {
                enemyId = 2,
                amount = 3,
                spawnZone = sideSpawnZoneR,
                positions = new Vector2[]
                {
                    new Vector2(sideSpawnZoneR.position.x-1, sideSpawnZoneR.position.y),
                    new Vector2(sideSpawnZoneR.position.x, sideSpawnZoneR.position.y),
                    new Vector2(sideSpawnZoneR.position.x+1, sideSpawnZoneR.position.y),
                }
            },
             new SpawnPattern
            {
                enemyId = 12,
                amount = 1,
                spawnZone = sideSpawnZoneL,
                positions = new Vector2[]
                {
                    new Vector2(sideSpawnZoneL.position.x, sideSpawnZoneL.position.y),
                }
            },
             new SpawnPattern
            {
                enemyId = 12,
                amount = 1,
                spawnZone = sideSpawnZoneR,
                positions = new Vector2[]
                {
                    new Vector2(sideSpawnZoneR.position.x, sideSpawnZoneR.position.y),
                }
            }
        };

    }

    private void Start()
    {
        CheckPossiblePattern();

        isBossSpawned = false;
        isBossDown = false;
        stopIndex = 1;
    }

    private void CheckPossiblePattern()
    {
        foreach(SpawnPattern pattern in spawnPatterns)
        {
            if (StageManager.stageInstance.stageEnemyIdList.Contains(pattern.enemyId))
            {
                canSpawnList.Add(pattern);
            }
        }
    }
    public IEnumerator SpawnEnemyTroops()
    {
        float spawnTimer = 8;
        while (true)
        {
            if (isBossSpawned)
            {
                break;
            }
            Debug.Log("Current spawnTimer: " + spawnTimer);
            if (spawnTimer == 8 && StageManager.stageInstance.minutes >= 5)
            {
                spawnTimer = 6;
            }
            else if (spawnTimer == 6 && StageManager.stageInstance.minutes >= 10)
            {
                spawnTimer = 4;
            }
            else if (spawnTimer == 4 && StageManager.stageInstance.minutes >= 15)
            {
                spawnTimer = 2;
            }
            else if (spawnTimer == 2 && StageManager.stageInstance.minutes >= 20)
            {
                spawnTimer = 1;
            }

            SpawnEnemy();
            yield return new WaitForSeconds(spawnTimer);
        }
    }


    private void SpawnEnemy()
    {
        if (StageManager.stageInstance.stage == 0)
        {
            // �⺻ ���� ����
            Vector3[] positions = new Vector3[]
            {
            new Vector3(-2f, 2, 0),
            new Vector3(0f, 2, 0),
            new Vector3(2f, 2, 0)
            };

            foreach (var position in positions)
            {
                PoolManager.poolInstance.GetEnemy(0, position, Quaternion.identity);
            }
        }
        else if (StageManager.stageInstance.stage >= 1)
        {
            // ���Ͽ� ����� ���� ����
            int patternIndex = Random.Range(0, canSpawnList.Count);
            SpawnPattern selectedPattern = canSpawnList[patternIndex];

            bool isItemEnemySpawn = Random.Range(0, 100) < 20; // 20% Ȯ���� ������ ���� �� ����

            // ���� ���Ͽ� ���� stopCount ���� ����
            int stopCountToUse = stopIndex;

            for (int i = 0; i < selectedPattern.amount; i++)
            {
                GameObject enemy = PoolManager.poolInstance.GetEnemy(selectedPattern.enemyId, selectedPattern.positions[i], selectedPattern.spawnZone.rotation);
                EnemyObject enemyObj = enemy.GetComponent<EnemyObject>();

                // ���� ���� ��� ���鿡�� ������ stopCount �� �Ҵ�
                enemyObj.stopCount = stopCountToUse;
                
                if (isItemEnemySpawn && i == selectedPattern.amount - 1)
                {
                    enemyObj.MakeEnemyDropItem = true;
                }
               
            }

            stopIndex++;
            if (stopIndex > 3)
            {
                stopIndex = 1;
            }
        }
    }



    public void SpawnBoss(int bossId)
    {
        
        if (GameManager.gameInstance.SpawnCoroutine != null)
        {
            StopCoroutine(GameManager.gameInstance.SpawnCoroutine);
            GameManager.gameInstance.SpawnCoroutine = null; // �ڷ�ƾ ������ null�� ����
        }

        isBossSpawned = true;
        isBossDown = false;
        PoolManager.poolInstance.GetEnemy(bossId, bossSpawnZone.transform.position, bossSpawnZone.transform.rotation);
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
