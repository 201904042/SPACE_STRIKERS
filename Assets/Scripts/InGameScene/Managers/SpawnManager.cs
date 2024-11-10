using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;
using static UnityEngine.EventSystems.EventTrigger;

[System.Serializable]
public struct SpawnPattern
{
    public EnemyType type;
    public Vector2[] positions;
    public Quaternion rotation;
    public float delay;
}

public class SpawnManager
{
    private const float initialSpawnTimer = 5f; //기본 10초에 한번 웨이브 발생
    private const float minimumSpawnTimer = 1f;  //최소 1초에 한번 웨이브 발생
    private const float maxTime = 30f;           //최대 게임시간 30분
    private const float phaseDuration = 5f;      //페이즈가 바뀌는시간 5분
    private const int DefaultItemDropRate =  10;
    private StageManager G_Stage => GameManager.Game.Stage;

    public Transform mainSpawnZone;
    public Transform sideSpawnZoneL;
    public Transform sideSpawnZoneR;
    public Transform bossSpawnZone;

    public List<SpawnPattern> CommonSPattern = new List<SpawnPattern>();
    public List<SpawnPattern> CommonNPattern = new List<SpawnPattern>();
    public List<SpawnPattern> EliteSPattern = new List<SpawnPattern>();
    public List<SpawnPattern> EliteNPattern = new List<SpawnPattern>();

    public List<GameObject> activeEnemyList = new List<GameObject>(); //현재 활성 상태인 적들의 리스트

    private int stopIndex;
    private int spawnPhase => GameManager.Game.phase;
    private float spawnTimer;
    private float currentMinutes => GameManager.Game.minutes;
   
    public bool isBossSpawned; //보스가 생성되었는지
    public bool isBossDown; //생성된 보스가 처치되었는지

    private Coroutine RouteSpawn;
    private Coroutine RandomSpawn;

    private void SpawnPatternSet()
    {
        CommonSPattern = new List<SpawnPattern>()
        {
            new SpawnPattern() {
                type = EnemyType.CommonS,
                positions = new Vector2[]
                {
                    new Vector2(mainSpawnZone.position.x-2f, mainSpawnZone.position.y),
                    new Vector2(mainSpawnZone.position.x-1f, mainSpawnZone.position.y),
                    new Vector2(mainSpawnZone.position.x, mainSpawnZone.position.y),
                    new Vector2(mainSpawnZone.position.x+1f, mainSpawnZone.position.y),
                    new Vector2(mainSpawnZone.position.x+2f, mainSpawnZone.position.y)
                },
                rotation = mainSpawnZone.rotation,
                delay = 0f,
            },
            new SpawnPattern() {
                type = EnemyType.CommonS,
                positions = new Vector2[]
                {
                    new Vector2(mainSpawnZone.position.x-2.5f, mainSpawnZone.position.y),
                    new Vector2(mainSpawnZone.position.x-1.5f, mainSpawnZone.position.y),
                    new Vector2(mainSpawnZone.position.x+0.5f, mainSpawnZone.position.y),
                    new Vector2(mainSpawnZone.position.x+1.5f, mainSpawnZone.position.y)
                },
                rotation = mainSpawnZone.rotation,
                delay = 0f,
            },
            new SpawnPattern() {
                type = EnemyType.CommonS,
                positions = new Vector2[]
                {
                    new Vector2(mainSpawnZone.position.x-1.5f, mainSpawnZone.position.y),
                    new Vector2(mainSpawnZone.position.x-0.5f, mainSpawnZone.position.y),
                    new Vector2(mainSpawnZone.position.x+1.5f, mainSpawnZone.position.y),
                    new Vector2(mainSpawnZone.position.x+2.5f, mainSpawnZone.position.y)
                },
                rotation = mainSpawnZone.rotation,
                delay = 0f,
            }
        };

        EliteSPattern = new List<SpawnPattern>()
        {
             new SpawnPattern
             {
                type = EnemyType.EliteS,
                positions = new Vector2[]
                {
                    new Vector2(mainSpawnZone.position.x-2f, mainSpawnZone.position.y),
                    new Vector2(mainSpawnZone.position.x+2f, mainSpawnZone.position.y)
                },
                rotation = mainSpawnZone.rotation,
                delay = 0f,
             },
             new SpawnPattern
             {
                type = EnemyType.EliteS,
                positions = new Vector2[]
                {
                    new Vector2(mainSpawnZone.position.x-2f, mainSpawnZone.position.y),
                    new Vector2(mainSpawnZone.position.x+2f, mainSpawnZone.position.y)
                },
                rotation = mainSpawnZone.rotation,
                delay = 0f,
             }
        };

        CommonNPattern = new List<SpawnPattern>()
        {
            new SpawnPattern() {
                type = EnemyType.CommonN,
                positions = new Vector2[]
                {
                    new Vector2(mainSpawnZone.position.x-1.5f, mainSpawnZone.position.y),
                    new Vector2(mainSpawnZone.position.x-0.5f, mainSpawnZone.position.y),
                    new Vector2(mainSpawnZone.position.x+0.5f, mainSpawnZone.position.y),
                    new Vector2(mainSpawnZone.position.x+1.5f, mainSpawnZone.position.y),
                },
                rotation = mainSpawnZone.rotation,
                 delay = 0.3f
            },
            new SpawnPattern
            {
                type = EnemyType.CommonN,
                positions = new Vector2[]
                {
                    new Vector2(sideSpawnZoneL.position.x-1, sideSpawnZoneL.position.y),
                    new Vector2(sideSpawnZoneL.position.x, sideSpawnZoneL.position.y),
                    new Vector2(sideSpawnZoneL.position.x+1, sideSpawnZoneL.position.y),
                },
                rotation = sideSpawnZoneL.rotation,
                delay = 0.3f
            },
             new SpawnPattern
            {
                type = EnemyType.CommonN,
                positions = new Vector2[]
                {
                    new Vector2(sideSpawnZoneR.position.x-1, sideSpawnZoneR.position.y),
                    new Vector2(sideSpawnZoneR.position.x, sideSpawnZoneR.position.y),
                    new Vector2(sideSpawnZoneR.position.x+1, sideSpawnZoneR.position.y),
                },
                rotation = sideSpawnZoneR.rotation,
                delay = 0.3f
            }
        };

        EliteNPattern = new List<SpawnPattern>
        {
             new SpawnPattern
             {
                type = EnemyType.EliteN,
                positions = new Vector2[]
                {
                    new Vector2(mainSpawnZone.position.x, mainSpawnZone.position.y),
                },
                rotation = mainSpawnZone.rotation,
                delay = 0f
             },

             new SpawnPattern
             {
                type = EnemyType.EliteN,
                positions = new Vector2[]
                {
                    new Vector2(sideSpawnZoneL.position.x, sideSpawnZoneL.position.y),
                },
                rotation = sideSpawnZoneL.rotation,
                 delay = 0f
             },
             new SpawnPattern
             {
                type = EnemyType.EliteN,
                positions = new Vector2[]
                {
                    new Vector2(sideSpawnZoneR.position.x, sideSpawnZoneR.position.y),
                },
                rotation = sideSpawnZoneR.rotation,
                delay = 0f
             }
        };
    }

    public void Init()
    {
        Transform SpawnZone = GameManager.Game.TriggerCheck.Find("EnemySpawnZone").transform;
        mainSpawnZone = SpawnZone.GetChild(0);
        sideSpawnZoneL = SpawnZone.GetChild(1);
        sideSpawnZoneR = SpawnZone.GetChild(2);
        bossSpawnZone = SpawnZone.GetChild(3);
        SpawnPatternSet();

        isBossSpawned = false;
        isBossDown = false;
        stopIndex = 1;
        spawnTimer = initialSpawnTimer;

        RouteSpawn = null;
        RandomSpawn = null;

        Debug.Log("스폰 초기화 완료");
    }

    public void StartSpawnEnemy()
    {
        RouteSpawn = StartCoroutine(RouteTimeSpawn());
        RandomSpawn = StartCoroutine(RandomTimeSpawn());
    }

    public void StopSpawnEnemy()
    {
        StopCoroutine(RouteSpawn);
        StopCoroutine(RandomSpawn);
    }

    private IEnumerator RouteTimeSpawn() //정해진 시간에의한 패턴시간( 10 -> 5)초에 한번씩 패턴 실행
    {
        float spanwTime = 10;
        while (true)
        {
            Debug.Log("정기 스폰");
            StartCoroutine(SpawnRandomPattern(true));
            yield return new WaitForSeconds(spanwTime);
        }
    }
    private IEnumerator RandomTimeSpawn() //랜덤한 시간에 의한 패턴시간/2 (5-> 2.5)초를 기준으로 +- 1초의 스폰시간을 가짐
    {
        float spanwTime = 5;
        float randomDelay = 0;
        while (true)
        {
            randomDelay = Random.Range(-1, 2); //-1에서 1초 사이의 랜덤한 딜레이를 줌
            spanwTime += randomDelay;
            yield return new WaitForSeconds(spanwTime);
            Debug.Log("랜덤 스폰");
            StartCoroutine(SpawnRandomPattern(false));
        }
    }

    //등장할 적 설정 -> 해당 적의 등장패턴 설정 -> 아이템 스폰률 설정 -> 스폰
    private IEnumerator SpawnRandomPattern(bool isStopPattern)
    {
        int randomId = GetRandomEnemyId(isStopPattern); //스테이지의 적중 랜덤한 아이디 정하기
        SpawnPattern? selectedPattern = GetRandomSpawnPattern(randomId); //정해진 아이디가 갈수 있는 패턴 정함
        
        if (selectedPattern.HasValue)
        {
            bool isStop = selectedPattern.Value.type == EnemyType.EliteS || selectedPattern.Value.type == EnemyType.CommonS;
            bool isItemEnemySpawn = Random.Range(0, 100) < DefaultItemDropRate;

            for (int i = 0; i < selectedPattern.Value.positions.Length; i++) {
                E_TroopBase enemy = GameManager.Game.Pool
                        .GetEnemy(randomId, selectedPattern.Value.positions[i], selectedPattern.Value.rotation)
                        .GetComponent<E_TroopBase>();
                enemy.SetStopLine(stopIndex);

                if (isItemEnemySpawn && i == selectedPattern.Value.positions.Length- 1) //패턴의 마지막 적에게만
                {
                    enemy.SetItemDrop();
                }

                yield return new WaitForSeconds(selectedPattern.Value.delay);
            }

            //스탑위치 조정
            if (isStop)
            {
                stopIndex++;
                if (stopIndex > 3)
                {
                    stopIndex = 1;
                }
            }
        }
    }

    private SpawnPattern? GetRandomSpawnPattern(int randomEnemyId)
    {
        EnemyData enemyData = DataManager.enemy.GetData(randomEnemyId);
        if (enemyData == null)
        {
            return null;
        }

        EnemyType enemyType = enemyData.type;
        List<SpawnPattern> matchingPatterns = new List<SpawnPattern>();
        if (enemyType == EnemyType.CommonS)
        {
            matchingPatterns = CommonSPattern;
        }
        else if ( enemyType == EnemyType.EliteS)
        {
            matchingPatterns = EliteSPattern;
        }
        else if(enemyType == EnemyType.CommonN)
        {
            matchingPatterns = CommonNPattern;
        }
        else if(enemyType == EnemyType.EliteN)
        {
            matchingPatterns = EliteNPattern;
        }

        if (matchingPatterns.Count == 0)
        {
            Debug.Log("가능한 패턴이 없음");
            return null;
        }

        SpawnPattern selectedPattern = matchingPatterns[Random.Range(0, matchingPatterns.Count)];
        return selectedPattern;
    }

    //현 스테이지의 모드에 따른 적 아이디 랜덤 반환
    private int GetRandomEnemyId(bool isStopPattern)
    {
        List<int> validEnemyIds = new List<int>();
        if (G_Stage.curMode == GameMode.Infinite) //무한모드에는 제한없음
        {
            foreach (var entry in G_Stage.enemyCodeAmountFair)
            {
                EnemyData enemy = DataManager.enemy.GetData(entry.Key);
                if (isStopPattern)
                {
                    if (enemy.type == EnemyType.EliteS || enemy.type == EnemyType.CommonS)
                    {
                        validEnemyIds.Add(entry.Key);
                    }
                }
                else
                {
                    if (enemy.type == EnemyType.EliteN || enemy.type == EnemyType.CommonN)
                    {
                        validEnemyIds.Add(entry.Key);
                    }
                }
            }
        }
        else //일반이나 보스에서는 적의 개채수 따짐
        {
            foreach (var entry in G_Stage.enemyCodeAmountFair)
            {
                if (entry.Value <= 0) // 양이 0보다 큰 경우
                {
                    continue;
                }

                EnemyData enemy = DataManager.enemy.GetData(entry.Key);
                if (isStopPattern)
                {
                    if (enemy.type == EnemyType.EliteS || enemy.type == EnemyType.CommonS)
                    {
                        validEnemyIds.Add(entry.Key);
                    }
                }
                else
                {
                    if (enemy.type == EnemyType.EliteN || enemy.type == EnemyType.CommonN)
                    {
                        validEnemyIds.Add(entry.Key);
                    }
                }
            }
        }

        //랜덤 적 선택
        return validEnemyIds[Random.Range(0, validEnemyIds.Count)];
    }
    
    //현 스테이지의 보스를 생성
    public void SpawnStageBoss()
    {
        SpawnBossById(G_Stage.stageBossId);
    }

    //아이디를 통해 보스를 생성
    public void SpawnBossById(int id)
    {
        GameManager.Game.Pool.GetEnemy(id, bossSpawnZone.transform.position, bossSpawnZone.transform.rotation).GetComponent<E_TroopBase>();
        isBossSpawned = true;
        isBossDown = false;
    }

    //모든 적들을 시스템으로 사망(보상없음)
    public void DeleteAllEnemy()
    {
        for (int i = 0; i < activeEnemyList.Count; i++)
        {
            activeEnemyList[i].GetComponent<EnemyObject>().EliminateEnemy();
        }
    }

    private Coroutine StartCoroutine(IEnumerator coroutine)
    {
        return GameManager.Game.StartOtherManagerCoroutine(coroutine);
    }

    private void StopCoroutine(Coroutine coroutine)
    {
        GameManager.Game.StopCoroutineSafely(coroutine);
    }
}
