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
    public Transform mainSpawnZone;
    public Transform sideSpawnZoneL;
    public Transform sideSpawnZoneR;
   
    public List<SpawnPattern> spawnPatterns;
    public List<SpawnPattern> canSpawnList;
    public List<GameObject> activeEnemyList; //���� Ȱ�� ������ ������ ����Ʈ

    public Transform bossSpawnZone;
    public bool isBossSpawned; //������ �����Ǿ�����
    public bool isBossDown; //������ ������ óġ�Ǿ�����
    private int stopIndex; // ���� �� ������ ���� stopCount ���� �����ϴ� ����

    public void Init()
    {
        Transform SpawnZone = GameObject.Find("SpawnZone").transform;
        mainSpawnZone = SpawnZone.GetChild(0);
        sideSpawnZoneL = SpawnZone.GetChild(1);
        sideSpawnZoneR = SpawnZone.GetChild(2);

        SpawnPatternSet(); //���� ���� ������
        CheckPossiblePattern(); //�̹� ������������ ��� ������ ����

        activeEnemyList = new List<GameObject>();
        isBossSpawned = false;
        isBossDown = false;
        stopIndex = 1;
    }

    private void SpawnPatternSet()
    {
        spawnPatterns = new List<SpawnPattern>()
        {
            //���ν�����
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
            //���̵� ������
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

            float currentMinutes = GameManager.game.minutes;

            if (currentMinutes < timeToDecrease)
            {
                spawnTimer = Mathf.Lerp(initialSpawnTimer, minimumSpawnTimer, currentMinutes / timeToDecrease);
            }
            else if (currentMinutes >= timeToDecrease && currentMinutes <= maxTime)
            {
                // 10~20�� ���̿����� �ּҰ��� 1�� ���� -> ���氡�ɼ� ����
                spawnTimer = minimumSpawnTimer;
            }
            else if (currentMinutes > maxTime)
            {
                // 20�� ������ ��쿡�� spawnTimer�� 1�� ����
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
            // �⺻ ���� ����
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
            // ���Ͽ� ����� ���� ����
            int patternIndex = Random.Range(0, canSpawnList.Count);
            SpawnPattern selectedPattern = canSpawnList[patternIndex];

            bool isItemEnemySpawn = Random.Range(0, 100) < 20; // 20% Ȯ���� ������ ���� �� ����

            // ���� ���Ͽ� ���� stopCount ���� ����
            int stopCountToUse = stopIndex;

            for (int i = 0; i < selectedPattern.amount; i++)
            {
                GameObject enemy = Managers.Instance.Pool.GetEnemy(selectedPattern.enemyId, selectedPattern.positions[i], selectedPattern.spawnZone.rotation);
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
        
        if (GameManager.game.SpawnCoroutine != null)
        {
            
            StopCoroutine(GameManager.game.SpawnCoroutine);
            GameManager.game.SpawnCoroutine = null; // �ڷ�ƾ ������ null�� ����
        }

        isBossSpawned = true;
        isBossDown = false;
        Managers.Instance.Pool.GetEnemy(bossId, bossSpawnZone.transform.position, bossSpawnZone.transform.rotation);
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
