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
    public Transform mainSpawnZone;
    public Transform sideSpawnZoneL;
    public Transform sideSpawnZoneR;
   
    public List<SpawnPattern> spawnPatterns;
    public List<SpawnPattern> canSpawnList;
    public List<GameObject> activeEnemyList; //현재 활성 상태인 적들의 리스트

    public Transform bossSpawnZone;
    public bool isBossSpawned; //보스가 생성되었는지
    public bool isBossDown; //생성된 보스가 처치되었는지
    private int stopIndex; // 패턴 간 번갈아 가는 stopCount 값을 저장하는 변수

    public void Init()
    {
        Transform SpawnZone = GameObject.Find("SpawnZone").transform;
        mainSpawnZone = SpawnZone.GetChild(0);
        sideSpawnZoneL = SpawnZone.GetChild(1);
        sideSpawnZoneR = SpawnZone.GetChild(2);

        SpawnPatternSet(); //스폰 패턴 데이터
        CheckPossiblePattern(); //이번 스테이지에서 사용 가능한 패턴

        activeEnemyList = new List<GameObject>();
        isBossSpawned = false;
        isBossDown = false;
        stopIndex = 1;
    }

    private void SpawnPatternSet()
    {
        spawnPatterns = new List<SpawnPattern>()
        {
            //메인스폰존
            new SpawnPattern() { 
                enemyId = 501,
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
                enemyId = 502,
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
                enemyId = 511, 
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
                enemyId = 512, 
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
                enemyId = 502,
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
                enemyId = 502,
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
                enemyId = 512,
                amount = 1,
                spawnZone = sideSpawnZoneL,
                positions = new Vector2[]
                {
                    new Vector2(sideSpawnZoneL.position.x, sideSpawnZoneL.position.y),
                }
            },
             new SpawnPattern
            {
                enemyId = 512,
                amount = 1,
                spawnZone = sideSpawnZoneR,
                positions = new Vector2[]
                {
                    new Vector2(sideSpawnZoneR.position.x, sideSpawnZoneR.position.y),
                }
            }
        };

    }
    private void CheckPossiblePattern()
    {
        canSpawnList = new List<SpawnPattern>();
        foreach (SpawnPattern pattern in spawnPatterns)
        {
            if (Managers.Instance.Stage.stageEnemyIdList.Contains(pattern.enemyId))
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

            float initialSpawnTimer = 8f;
            float minimumSpawnTimer = 1f;
            float maxTime = 20f;
            float timeToDecrease = 10f;

            float currentMinutes = GameManager.Instance.minutes;

            if (currentMinutes < timeToDecrease)
            {
                spawnTimer = Mathf.Lerp(initialSpawnTimer, minimumSpawnTimer, currentMinutes / timeToDecrease);
            }
            else if (currentMinutes >= timeToDecrease && currentMinutes <= maxTime)
            {
                // 10~20분 사이에서는 최소값인 1을 유지 -> 변경가능성 있음
                spawnTimer = minimumSpawnTimer;
            }
            else if (currentMinutes > maxTime)
            {
                // 20분 이후의 경우에도 spawnTimer는 1로 유지
                spawnTimer = minimumSpawnTimer;
            }

            SpawnEnemy();
            yield return new WaitForSeconds(spawnTimer);
        }
    }


    private void SpawnEnemy()
    {
        if (Managers.Instance.Stage.stage == 0) 
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
                Managers.Instance.Pool.GetEnemy(0, position, Quaternion.identity);
            }
        }
        else if (Managers.Instance.Stage.stage >= 1)
        {
            // 패턴에 기반한 스폰 로직
            int patternIndex = Random.Range(0, canSpawnList.Count);
            SpawnPattern selectedPattern = canSpawnList[patternIndex];

            bool isItemEnemySpawn = Random.Range(0, 100) < 20; // 20% 확률로 아이템 생성 적 생성

            // 현재 패턴에 대해 stopCount 값을 설정
            int stopCountToUse = stopIndex;

            for (int i = 0; i < selectedPattern.amount; i++)
            {
                GameObject enemy = Managers.Instance.Pool.GetEnemy(selectedPattern.enemyId, selectedPattern.positions[i], selectedPattern.spawnZone.rotation);
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
        
        if (GameManager.Instance.SpawnCoroutine != null)
        {
            
            StopCoroutine(GameManager.Instance.SpawnCoroutine);
            GameManager.Instance.SpawnCoroutine = null; // 코루틴 참조를 null로 설정
        }

        isBossSpawned = true;
        isBossDown = false;
        Managers.Instance.Pool.GetEnemy(bossId, bossSpawnZone.transform.position, bossSpawnZone.transform.rotation);
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
