using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;


[System.Serializable]
public struct SpawnPattern
{
    public int enemyId; // 적의 종류를 나타내는 ID
    public int amount; // 스폰할 적의 수
    public Transform spawnZone;
    public Vector2[] positions; // 적이 스폰될 위치
}

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager spawnInstance;

    [Header("커먼, 엘리트 등 스폰 관련")]
    //스폰 위치
    public Transform mainSpawnZone;
    public Transform sideSpawnZoneL;
    public Transform sideSpawnZoneR;
   
    //스폰할 패턴들
    public List<SpawnPattern> spawnPatterns;
    //이 스테이지에서 생성 가능한 스폰
    public List<SpawnPattern> canSpawnList;

    private int ranEnemy; //랜덤한 적

    public List<GameObject> activeEnemyList; //스폰상태인 적의 리스트

    public Transform bossSpawnZone;
    public bool isBossSpawned; //보스가 생성되었는지
    public bool isBossDown; //생성된 보스가 처치되었는지


    private int stopIndex = 1; // 패턴 간 번갈아 가는 stopCount 값을 저장하는 변수

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
        CheckPossiblePattern();

        isBossSpawned = false;
        isBossDown = false;
        stopIndex = 1;
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
            // 기본 스폰 로직
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
            // 패턴에 기반한 스폰 로직
            int patternIndex = Random.Range(0, canSpawnList.Count);
            SpawnPattern selectedPattern = canSpawnList[patternIndex];

            bool isItemEnemySpawn = Random.Range(0, 100) < 20; // 20% 확률로 아이템 생성 적 생성

            // 현재 패턴에 대해 stopCount 값을 설정
            int stopCountToUse = stopIndex;

            for (int i = 0; i < selectedPattern.amount; i++)
            {
                GameObject enemy = PoolManager.poolInstance.GetEnemy(selectedPattern.enemyId, selectedPattern.positions[i], selectedPattern.spawnZone.rotation);
                EnemyObject enemyObj = enemy.GetComponent<EnemyObject>();

                // 패턴 내의 모든 적들에게 동일한 stopCount 값 할당
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
            GameManager.gameInstance.SpawnCoroutine = null; // 코루틴 참조를 null로 설정
        }

        isBossSpawned = true;
        isBossDown = false;
        PoolManager.poolInstance.GetEnemy(bossId, bossSpawnZone.transform.position, bossSpawnZone.transform.rotation);
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
