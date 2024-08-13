using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;


[System.Serializable]
public class SpawnPattern
{
    public int enemyId; // 적의 종류를 나타내는 ID
    public int amount; // 스폰할 적의 수
    public Transform spawnZone;
    public Vector2[] positions; // 적이 스폰될 위치
}

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager spawnInstance;

    [Header("스폰관련")]
    public Transform mainSpawnZone;
    public Transform sideSpawnZoneL;
    public Transform sideSpawnZoneR;
    public Transform bossSpawnZone;
    public int stageEnemyAmount;
    public List<SpawnPattern> spawnPatterns;
    public List<SpawnPattern> canSpawnList;

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
        canSpawnList = new List<SpawnPattern>();
        spawnPatterns = new List<SpawnPattern>()
        {
            //메인스폰존
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
            //사이드 스폰존
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
        foreach (StageEnemy enemy in StageManager.stageInstance.curStageEnemy)
        {
            stageEnemyAmount += enemy.enemyAmount;
        }

        CheckPossiblePattern();

        maxSpawnDelay = 8f;
        curSpawnDelay = 4f;
    }

    private void CheckPossiblePattern()
    {
        foreach(SpawnPattern pattern in spawnPatterns)
        {
            if (StageManager.stageInstance.useingEnemyId.Contains(pattern.enemyId))
            {
                canSpawnList.Add(pattern);
            }
        }
    }

    public IEnumerator SpawnCheckCoroutine()
    {
        PoolManager.poolInstance.GetEnemy(31, bossSpawnZone.transform.position, bossSpawnZone.transform.rotation);
        while (true)
        {
            yield return new WaitForSeconds(maxSpawnDelay);
            SpawnEnemy();
        }
    }

    private void SpawnEnemy() //스폰 요소 고치기
    {
        
        if (StageManager.stageInstance.stage == 0)
        {
            PoolManager.poolInstance.GetEnemy(0, new Vector3(-2f, 2, 0),Quaternion.identity);
            PoolManager.poolInstance.GetEnemy(0, new Vector3(0f, 2, 0), Quaternion.identity);
            PoolManager.poolInstance.GetEnemy(0, new Vector3(2f, 2, 0), Quaternion.identity);
        }

        else if (StageManager.stageInstance.stage >= 1)
        {
            int patternIndex = Random.Range(0, canSpawnList.Count); // 랜덤으로 패턴 선택
            SpawnPattern selectedPattern = canSpawnList[patternIndex];

            foreach (Vector2 pos in selectedPattern.positions)
            {
                PoolManager.poolInstance.GetEnemy(selectedPattern.enemyId, pos,selectedPattern.spawnZone.rotation);
            }
        }
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
