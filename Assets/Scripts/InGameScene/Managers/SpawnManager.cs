using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;
using static UnityEditor.PlayerSettings;


[System.Serializable]
public struct SpawnPattern
{
    public EnemyType type;
    public Vector2[] positions; //스폰할 위치들. 개수만큼 스폰
    public Quaternion rotation;
}

public class SpawnManager : MonoBehaviour
{
    private const float initialSpawnTimer = 10f;
    private const float minimumSpawnTimer = 1f;
    private const float maxTime = 20f;
    private const float phaseDuration = 5f;

    public Transform mainSpawnZone;
    public Transform sideSpawnZoneL;
    public Transform sideSpawnZoneR;
    public List<SpawnPattern> AllPatterns = new List<SpawnPattern>();
    public List<SpawnPattern> CommonPattern = new List<SpawnPattern>();
    public List<SpawnPattern> ElitePattern = new List<SpawnPattern>();

    public List<GameObject> activeEnemyList = new List<GameObject>(); //현재 활성 상태인 적들의 리스트

    public Transform bossSpawnZone;
    
    private int stopIndex; // 패턴 간 번갈아 가는 stopCount 값을 저장하는 변수
    private int spawnPhase;
    private float spawnTimer;
    private float currentMinutes => GameManager.Game.minutes;
    


    public bool isBossSpawned; //보스가 생성되었는지
    public bool isBossDown; //생성된 보스가 처치되었는지

    public void Init()
    {
        Transform SpawnZone = GameManager.Game.TriggerCheck.Find("EnemySpawnZone").transform;
        mainSpawnZone = SpawnZone.GetChild(0);
        sideSpawnZoneL = SpawnZone.GetChild(1);
        sideSpawnZoneR = SpawnZone.GetChild(2);

        SpawnPatternSet(); //스폰 패턴 데이터

        isBossSpawned = false;
        isBossDown = false;
        stopIndex = 1;
        spawnTimer = initialSpawnTimer;
        spawnPhase = 0;

        Debug.Log("스폰 초기화 완료");
    }

    private void SpawnPatternSet()
    {
        CommonPattern = new List<SpawnPattern>()
        {
            //메인스폰존
            new SpawnPattern() {
                type = EnemyType.CommonType1,
                positions = new Vector2[]
                {
                    new Vector2(-2f, mainSpawnZone.position.y),
                    new Vector2(-1f, mainSpawnZone.position.y),
                    new Vector2(0f, mainSpawnZone.position.y),
                    new Vector2(1f, mainSpawnZone.position.y),
                    new Vector2(2f, mainSpawnZone.position.y)
                },
                rotation = mainSpawnZone.rotation
            },
            new SpawnPattern() {
                type = EnemyType.CommonType1,
                positions = new Vector2[]
                {
                    new Vector2(-1.5f, mainSpawnZone.position.y),
                    new Vector2(-0.5f, mainSpawnZone.position.y),
                    new Vector2(0.5f, mainSpawnZone.position.y),
                    new Vector2(1.5f, mainSpawnZone.position.y),

                },
                rotation = mainSpawnZone.rotation
            },
            new SpawnPattern() {
                type = EnemyType.CommonType2,
                positions = new Vector2[]
                {
                    new Vector2(-1f, mainSpawnZone.position.y),
                    new Vector2(0f, mainSpawnZone.position.y),
                    new Vector2(1f, mainSpawnZone.position.y)
                },
                rotation = mainSpawnZone.rotation
            },
            //사이드 스폰존
            new SpawnPattern
            {
                type = EnemyType.CommonType2,
                positions = new Vector2[]
                {
                    new Vector2(sideSpawnZoneL.position.x-1, sideSpawnZoneL.position.y),
                    new Vector2(sideSpawnZoneL.position.x, sideSpawnZoneL.position.y),
                    new Vector2(sideSpawnZoneL.position.x+1, sideSpawnZoneL.position.y),
                },
                rotation = sideSpawnZoneL.rotation
            },
             new SpawnPattern
            {
                type = EnemyType.CommonType2,
                positions = new Vector2[]
                {
                    new Vector2(sideSpawnZoneR.position.x-1, sideSpawnZoneR.position.y),
                    new Vector2(sideSpawnZoneR.position.x, sideSpawnZoneR.position.y),
                    new Vector2(sideSpawnZoneR.position.x+1, sideSpawnZoneR.position.y),
                },
                rotation = sideSpawnZoneR.rotation
            }
        };

        ElitePattern = new List<SpawnPattern>()
        {
            new SpawnPattern
            {
                type = EnemyType.EliteType1,
                positions = new Vector2[]
                {
                    new Vector2(-2f, mainSpawnZone.position.y),
                    new Vector2(2f, mainSpawnZone.position.y)
                },
                rotation = mainSpawnZone.rotation
            },
            new SpawnPattern
            {
                type = EnemyType.EliteType2,
                positions = new Vector2[]
                {
                    new Vector2(0, mainSpawnZone.position.y),
                },
                rotation = mainSpawnZone.rotation
            },
            
             new SpawnPattern
            {
                type = EnemyType.EliteType2,
                positions = new Vector2[]
                {
                    new Vector2(sideSpawnZoneL.position.x, sideSpawnZoneL.position.y),
                },
                rotation = sideSpawnZoneL.rotation
            },
             new SpawnPattern
            {
                type = EnemyType.EliteType2,
                positions = new Vector2[]
                {
                    new Vector2(sideSpawnZoneR.position.x, sideSpawnZoneR.position.y),
                },
                rotation = sideSpawnZoneR.rotation
            }
        };

        AllPatterns.AddRange(CommonPattern);
        AllPatterns.AddRange(ElitePattern);
    }


    StageManager G_Stage => GameManager.Game.Stage;


    public IEnumerator SpawnEnemyTroops()
    {
        while (true)
        {
            // Update the phase every 5 minutes
            if (currentMinutes % phaseDuration == 0 && currentMinutes > 0)
            {
                spawnPhase++;
            }

            // Calculate the spawn timer based on the current phase
            spawnTimer = Mathf.Max(minimumSpawnTimer, initialSpawnTimer - spawnPhase);

            SpawnEnemy();

            yield return new WaitForSeconds(spawnTimer);
        }
    }

    private void SpawnEnemy()
    {
        int randomId = GetRandomEnemyId();
        SpawnPattern? sp = null;
        while (sp == null)
        {
            sp = GetRandomSpawnPattern(randomId);
        }

        SpawnPattern selectedPattern = (SpawnPattern)sp;

        bool isItemEnemySpawn = Random.Range(0, 100) < 20; // 20% 확률로 아이템 생성 적 생성

        // 현재 패턴에 대해 stopCount 값을 설정
        int stopCountToUse = stopIndex;

        for (int i = 0; i < selectedPattern.positions.Length; i++)
        {
            EnemyObject enemy = SpawnEnemyObj(randomId, selectedPattern.type, selectedPattern.positions[i], selectedPattern.rotation);
            // 패턴 내의 모든 적들에게 동일한 stopCount 값 할당
            enemy.stopCount = stopCountToUse;

            if (isItemEnemySpawn && i == selectedPattern.positions.Length - 1) //마지막 적에게만
            {
                enemy.MakeEnemyDropItem = true;
            }
        }
        stopIndex++;
        if (stopIndex > 3)
        {
            stopIndex = 1;
        }
    }

    private int GetRandomEnemyId()
    {
        List<int> validEnemyIds = new List<int>();
        if (G_Stage.curMode == GameMode.Infinite) //무한모드에는 제한없음
        {
            foreach (var entry in G_Stage.enemyCodeAmountFair)
            {
                validEnemyIds.Add(entry.Key);
            }
        }
        else
        {
            foreach (var entry in G_Stage.enemyCodeAmountFair) //일반이나 보스에서는 적의 개채수 따짐
            {
                if (entry.Value > 0) // 양이 0보다 큰 경우
                {
                    validEnemyIds.Add(entry.Key);
                }
            }
        }

        //랜덤 적 선택
        return validEnemyIds[Random.Range(0, validEnemyIds.Count)];
    }
    private SpawnPattern? GetRandomSpawnPattern(int randomEnemyId)
    {

        EnemyData enemyData = DataManager.enemy.GetData(randomEnemyId);
        if (enemyData == null)
        {
            Debug.LogWarning($"적 데이터(id: {randomEnemyId})를 찾을 수 없습니다.");
            return null;
        }

        EnemyType enemyType = enemyData.type;

        //적 타입에 다른 가능한 패턴 선택
        List<SpawnPattern> matchingPatterns = new List<SpawnPattern>();
        if(enemyType == EnemyType.CommonType1 || enemyType == EnemyType.CommonType2)
        {
            foreach (var pattern in CommonPattern)
            {
                if (pattern.type == enemyType)
                {
                    matchingPatterns.Add(pattern);
                }
            }
        }
        else if(enemyType == EnemyType.EliteType1 || enemyType == EnemyType.EliteType2)
        {
            foreach (var pattern in ElitePattern)
            {
                if (pattern.type == enemyType)
                {
                    matchingPatterns.Add(pattern);
                }
            }
        }

        if (matchingPatterns.Count == 0)
        {
            Debug.LogWarning($"일치하는 스폰 패턴을 찾을 수 없습니다. (타입: {enemyType})");
            return null;
        }

        SpawnPattern selectedPattern = matchingPatterns[Random.Range(0, matchingPatterns.Count)];
        return selectedPattern;
    }

    public void SpawnBoss(int bossId)
    {
        
        if (GameManager.Game.SpawnCoroutine != null)
        {
            
            StopCoroutine(GameManager.Game.SpawnCoroutine);
            GameManager.Game.SpawnCoroutine = null; // 코루틴 참조를 null로 설정
        }

        isBossSpawned = true;
        isBossDown = false;
        SpawnEnemyObj(bossId, EnemyType.Boss, bossSpawnZone.transform.position, bossSpawnZone.transform.rotation);
    }

    public EnemyObject SpawnEnemyObj(int enemyId, EnemyType type, Vector3 pos, Quaternion rot)
    {
        EnemyObject enemy = GameManager.Game.Pool.GetEnemy(type, pos, rot).GetComponent<EnemyObject>();
        enemy.SetId(enemyId);
        return enemy;
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
