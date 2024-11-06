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

        isBossSpawned = false;
        isBossDown = false;
        stopIndex = 1;
        spawnTimer = initialSpawnTimer;
        spawnPhase = 0;

        Debug.Log("스폰 초기화 완료");
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


    //모든 적들을 시스템으로 사망(보상없음)
    public void DeleteEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<EnemyObject>().EliminateEnemy();
        }
    }

}
