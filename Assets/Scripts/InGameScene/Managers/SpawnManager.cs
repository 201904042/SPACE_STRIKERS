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
    private const float initialSpawnTimer = 5f; //�⺻ 10�ʿ� �ѹ� ���̺� �߻�
    private const float minimumSpawnTimer = 1f;  //�ּ� 1�ʿ� �ѹ� ���̺� �߻�
    private const float maxTime = 30f;           //�ִ� ���ӽð� 30��
    private const float phaseDuration = 5f;      //����� �ٲ�½ð� 5��
    private const int DefaultItemDropRate =  10;
    private StageManager G_Stage => GameManager.Game.Stage;

    public Transform mainSpawnZone;
    public Transform sideSpawnZoneL;
    public Transform sideSpawnZoneR;

    public List<SpawnPattern> CommonSPattern = new List<SpawnPattern>();
    public List<SpawnPattern> CommonNPattern = new List<SpawnPattern>();
    public List<SpawnPattern> EliteSPattern = new List<SpawnPattern>();
    public List<SpawnPattern> EliteNPattern = new List<SpawnPattern>();


    public List<GameObject> activeEnemyList = new List<GameObject>(); //���� Ȱ�� ������ ������ ����Ʈ

    public Transform bossSpawnZone;
    
    private int stopIndex; // ���� �� ������ ���� stopCount ���� �����ϴ� ����
    private int spawnPhase;
    private float spawnTimer;
    private float currentMinutes => GameManager.Game.minutes;
   
    public bool isBossSpawned; //������ �����Ǿ�����
    public bool isBossDown; //������ ������ óġ�Ǿ�����
   

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

        SpawnPatternSet();

        isBossSpawned = false;
        isBossDown = false;
        stopIndex = 1;
        spawnTimer = initialSpawnTimer;
        spawnPhase = 0;

        Debug.Log("���� �ʱ�ȭ �Ϸ�");
    }

    public IEnumerator SpawnEnemyTroops()
    {
        GameManager.Game.StartGameCoroutine(RouteTimeSpawn());
        GameManager.Game.StartGameCoroutine(RandomTimeSpawn());
        yield return true;
    }

    
    private IEnumerator RouteTimeSpawn() //������ �ð������� ���Ͻð�( 10 -> 5)�ʿ� �ѹ��� ���� ����
    {
        float spanwTime = 10;
        while (true)
        {
            Debug.Log("���� ����");
            GameManager.Game.StartGameCoroutine(SpawnRandomPattern(true));
            yield return new WaitForSeconds(spanwTime);
        }
    }
    private IEnumerator RandomTimeSpawn() //������ �ð��� ���� ���Ͻð�/2 (5-> 2.5)�ʸ� �������� +- 1���� �����ð��� ����
    {
        float spanwTime = 5;
        float randomDelay = 0;
        while (true)
        {
            randomDelay = Random.Range(-1, 2); //-1���� 1�� ������ ������ �����̸� ��
            spanwTime += randomDelay;
            yield return new WaitForSeconds(spanwTime);
            Debug.Log("���� ����");
            GameManager.Game.StartGameCoroutine(SpawnRandomPattern(false));
        }
    }

    // �־��� ���� ����Ʈ���� �� ���� ������ ó���ϴ�
    private IEnumerator SpawnRandomPattern(bool isStopPattern)
    {
        int randomId = GetRandomEnemyId(isStopPattern); //���������� ���� ������ ���̵� ���ϱ�
        SpawnPattern? selectedPattern = GetRandomSpawnPattern(randomId); //������ ���̵� ���� �ִ� ���� ����

        if (selectedPattern.HasValue)
        {
            bool isItemEnemySpawn = Random.Range(0, 100) < DefaultItemDropRate;

            for (int i = 0; i < selectedPattern.Value.positions.Length; i++) {
                E_TroopBase enemy = GameManager.Game.Pool
                        .GetEnemy(randomId, selectedPattern.Value.positions[i], selectedPattern.Value.rotation)
                        .GetComponent<E_TroopBase>();
                enemy.SetStopLine(stopIndex);

                if (isItemEnemySpawn && i == selectedPattern.Value.positions.Length- 1) //������ ������ �����Ը�
                {
                    enemy.SetItemDrop();
                }

                yield return new WaitForSeconds(selectedPattern.Value.delay);
            }
           
            //��ž��ġ ����
            stopIndex++;
            if (stopIndex > 3)
            {
                stopIndex = 1;
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
            Debug.Log("������ ������ ����");
            return null;
        }

        SpawnPattern selectedPattern = matchingPatterns[Random.Range(0, matchingPatterns.Count)];
        return selectedPattern;
    }

    //�� ���������� ��忡 ���� �� ���̵� ���� ��ȯ
    private int GetRandomEnemyId(bool isStopPattern)
    {
        List<int> validEnemyIds = new List<int>();
        if (G_Stage.curMode == GameMode.Infinite) //���Ѹ�忡�� ���Ѿ���
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
        else
        {
            foreach (var entry in G_Stage.enemyCodeAmountFair) //�Ϲ��̳� ���������� ���� ��ä�� ����
            {
                if (entry.Value <= 0) // ���� 0���� ū ���
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

        //���� �� ����
        return validEnemyIds[Random.Range(0, validEnemyIds.Count)];
    }
    
    //�� ���������� ������ ����
    public void SpawnBoss(int bossId)
    {
        GameManager.Game.StopSpawnTroop();

        isBossSpawned = true;
        isBossDown = false;
        GameManager.Game.Pool.GetEnemy(531, bossSpawnZone.transform.position, bossSpawnZone.transform.rotation).GetComponent<E_TroopBase>();
    }

    //��� ������ �ý������� ���(�������)
    public void DeleteEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<EnemyObject>().EliminateEnemy();
        }
    }
}
