using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class SpawnManager
{
    private const float initialSpawnTimer = 10f;
    private const float minimumSpawnTimer = 1f;
    private const float maxTime = 20f;
    private const float phaseDuration = 5f;
    private StageManager G_Stage => GameManager.Game.Stage;

    public Transform mainSpawnZone;
    public Transform sideSpawnZoneL;
    public Transform sideSpawnZoneR;


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

        isBossSpawned = false;
        isBossDown = false;
        stopIndex = 1;
        spawnTimer = initialSpawnTimer;
        spawnPhase = 0;

        Debug.Log("���� �ʱ�ȭ �Ϸ�");
    }

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

    public E_TroopBase SpawnTroops(int id, Vector3 pos, Quaternion rot)
    {
        E_TroopBase enemy = GameManager.Game.Pool.GetEnemy(id, pos, rot).GetComponent<E_TroopBase>();
        return enemy;
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
