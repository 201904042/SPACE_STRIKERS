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
    public Vector2[] positions; //������ ��ġ��. ������ŭ ����
    public Quaternion rotation;
}

public class SpawnManager
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

    public List<GameObject> activeEnemyList = new List<GameObject>(); //���� Ȱ�� ������ ������ ����Ʈ

    public Transform bossSpawnZone;
    
    private int stopIndex; // ���� �� ������ ���� stopCount ���� �����ϴ� ����
    private int spawnPhase;
    private float spawnTimer;
    private float currentMinutes => GameManager.Game.minutes;
    


    public bool isBossSpawned; //������ �����Ǿ�����
    public bool isBossDown; //������ ������ óġ�Ǿ�����

    public void Init()
    {
        Transform SpawnZone = GameManager.Game.TriggerCheck.Find("EnemySpawnZone").transform;
        mainSpawnZone = SpawnZone.GetChild(0);
        sideSpawnZoneL = SpawnZone.GetChild(1);
        sideSpawnZoneR = SpawnZone.GetChild(2);

        SpawnPatternSet(); //���� ���� ������

        isBossSpawned = false;
        isBossDown = false;
        stopIndex = 1;
        spawnTimer = initialSpawnTimer;
        spawnPhase = 0;

        Debug.Log("���� �ʱ�ȭ �Ϸ�");
    }

    private void SpawnPatternSet()
    {
        CommonPattern = new List<SpawnPattern>()
        {
            //���ν�����
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
            //���̵� ������
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

        bool isItemEnemySpawn = Random.Range(0, 100) < 20; // 20% Ȯ���� ������ ���� �� ����

        // ���� ���Ͽ� ���� stopCount ���� ����
        int stopCountToUse = stopIndex;

        for (int i = 0; i < selectedPattern.positions.Length; i++)
        {
            EnemyObject enemy = SpawnEnemyObj(randomId, selectedPattern.type, selectedPattern.positions[i], selectedPattern.rotation);
            // ���� ���� ��� ���鿡�� ������ stopCount �� �Ҵ�
            enemy.stopCount = stopCountToUse;

            if (isItemEnemySpawn && i == selectedPattern.positions.Length - 1) //������ �����Ը�
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
    private SpawnPattern? GetRandomSpawnPattern(int randomEnemyId)
    {

        EnemyData enemyData = DataManager.enemy.GetData(randomEnemyId);
        if (enemyData == null)
        {
            Debug.LogWarning($"�� ������(id: {randomEnemyId})�� ã�� �� �����ϴ�.");
            return null;
        }

        EnemyType enemyType = enemyData.type;

        //�� Ÿ�Կ� �ٸ� ������ ���� ����
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
            Debug.LogWarning($"��ġ�ϴ� ���� ������ ã�� �� �����ϴ�. (Ÿ��: {enemyType})");
            return null;
        }

        SpawnPattern selectedPattern = matchingPatterns[Random.Range(0, matchingPatterns.Count)];
        return selectedPattern;
    }

    public void SpawnBoss(int bossId)
    {

        GameManager.Game.StopSpawnTroop();

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
