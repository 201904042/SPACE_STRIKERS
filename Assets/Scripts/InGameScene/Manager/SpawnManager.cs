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

    public IEnumerator SpawnEnemyTroops() //커먼이나 엘리트 적의 경우
    {
        float spawnTimer = 8;
        
        while (true)
        {
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

            int itemEnemyRandomRate = Random.Range(0, 100);
            bool isItemEnemySpawn = itemEnemyRandomRate < 20 ? true : false; //20% 확률로 해당 패턴에서 아이템을 생성하는 적 생성

            for(int i =0; i< selectedPattern.amount; i++)
            {
                GameObject enemy = PoolManager.poolInstance.GetEnemy(selectedPattern.enemyId, selectedPattern.positions[i], selectedPattern.spawnZone.rotation);
                if(i == selectedPattern.amount - 1)
                {
                    enemy.GetComponent<EnemyObject>().MakeEnemyDropItem = true;
                }
            }

            
        }
    }

    public void SpawnBoss(int bossId)
    {
        StopCoroutine(GameManager.gameInstance.SpawnCoroutine);

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
