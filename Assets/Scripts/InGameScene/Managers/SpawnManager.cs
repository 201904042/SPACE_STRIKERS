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
    private StageManager G_Stage => GameManager.Game.Stage;

    public Transform mainSpawnZone;
    public Transform sideSpawnZoneL;
    public Transform sideSpawnZoneR;

    public List<SpawnPattern> AllPatterns = new List<SpawnPattern>();

    public List<SpawnPattern> stopEnemyPatterns = new List<SpawnPattern>();
    public List<SpawnPattern> nonStopEnemyPatterns = new List<SpawnPattern>();


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
        stopEnemyPatterns = new List<SpawnPattern>()
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

        nonStopEnemyPatterns = new List<SpawnPattern>()
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
                 delay = 0.5f
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
                delay = 0.5f
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
                delay = 0.5f
            },
            new SpawnPattern
            {
                type = EnemyType.EliteN,
                positions = new Vector2[]
                {
                    new Vector2(mainSpawnZone.position.x, mainSpawnZone.position.y),
                },
                rotation = mainSpawnZone.rotation,
                delay = 0.5f
            },

             new SpawnPattern
            {
                type = EnemyType.EliteN,
                positions = new Vector2[]
                {
                    new Vector2(sideSpawnZoneL.position.x, sideSpawnZoneL.position.y),
                },
                rotation = sideSpawnZoneL.rotation,
                 delay = 0.5f
            },
             new SpawnPattern
            {
                type = EnemyType.EliteN,
                positions = new Vector2[]
                {
                    new Vector2(sideSpawnZoneR.position.x, sideSpawnZoneR.position.y),
                },
                rotation = sideSpawnZoneR.rotation,
                 delay = 0.5f
            }
        };

        AllPatterns.AddRange(stopEnemyPatterns);
        AllPatterns.AddRange(nonStopEnemyPatterns);
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
        GameManager.Game.StartGameCoroutine(SpawnStopEnemies());
        GameManager.Game.StartGameCoroutine(SpawnNonStopEnemies());

        yield return null;
    }

    private IEnumerator SpawnStopEnemies()
    {
        while (true)
        {
            SpawnEnemiesFromPattern(stopEnemyPatterns);
            yield return new WaitForSeconds(10f); 
        }
    }

    private IEnumerator SpawnNonStopEnemies()
    {
        while (true)
        {
            SpawnEnemiesFromPattern(nonStopEnemyPatterns);
            yield return new WaitForSeconds(3f);
        }
    }

    // �־��� ���� ����Ʈ���� �� ���� ������ ó���ϴ�
    private void SpawnEnemiesFromPattern(List<SpawnPattern> patterns)
    {
        int randomId = GetRandomEnemyId();
        SpawnPattern? selectedPattern = GetRandomSpawnPattern(randomId);

        if (selectedPattern.HasValue)
        {
            foreach (Vector2 position in selectedPattern.Value.positions)
            {
                E_TroopBase enemy = GameManager.Game.Pool
                    .GetEnemy(randomId, position, selectedPattern.Value.rotation)
                    .GetComponent<E_TroopBase>();
                enemy.SetStopLine(stopIndex);
                bool isItemEnemySpawn = Random.Range(0, 100) < 20;
                if (isItemEnemySpawn)
                {
                    enemy.SetItemDrop();
                }
            }

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
        if (enemyType == EnemyType.CommonN || enemyType == EnemyType.CommonS)
        {
            foreach (var pattern in stopEnemyPatterns)
            {
                if (pattern.type == enemyType)
                {
                    matchingPatterns.Add(pattern);
                }
            }
        }
        else if (enemyType == EnemyType.EliteN || enemyType == EnemyType.EliteS)
        {
            foreach (var pattern in nonStopEnemyPatterns)
            {
                if (pattern.type == enemyType)
                {
                    matchingPatterns.Add(pattern);
                }
            }
        }

        if (matchingPatterns.Count == 0)
        {

            return null;
        }

        SpawnPattern selectedPattern = matchingPatterns[Random.Range(0, matchingPatterns.Count)];
        return selectedPattern;
    }


    private int GetRandomEnemyId()
    {
        List<int> validEnemyIds = new List<int>();
        if (G_Stage.curMode == GameMode.Infinite) //���Ѹ�忡�� ���Ѿ���
        {
            foreach (var entry in G_Stage.enemyCodeAmountFair)
            {
                validEnemyIds.Add(entry.Key);
            }
        }
        else
        {
            foreach (var entry in G_Stage.enemyCodeAmountFair) //�Ϲ��̳� ���������� ���� ��ä�� ����
            {
                if (entry.Value > 0) // ���� 0���� ū ���
                {
                    validEnemyIds.Add(entry.Key);
                }
            }
        }

        //���� �� ����
        return validEnemyIds[Random.Range(0, validEnemyIds.Count)];
    }
    
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
