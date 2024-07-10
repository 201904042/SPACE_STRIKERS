using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Pool;

public class GameManager : MonoBehaviour
{
    public static GameManager gameInstance;
    public GameObject gameEndUI;

    [Header("오브젝트 풀 관련")]
    public ObjectPool<GameObject> enemyPool;

    public int score;
    public bool isBattleStart;
    public bool isGameClear;
    public bool isPerfectClear;

    private void Awake()
    {
        if (gameInstance == null)
        {
            gameInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Time.timeScale = 1;
    }

    private void Start()
    {
        score = 0;
        isBattleStart = false;
        isGameClear = false;
    }

    private void Update()
    {
        if (isBattleStart)
        {
            StageManager.stageInstance.StageTimer();
            SpawnManager.spawnInstance.SpawnCheck();

            if (SpawnManager.spawnInstance.stageEnemyAmount <= 0 || StageManager.stageInstance.minutes >= 15) //승리조건 만족시 게임 종료
            {
                Time.timeScale = 0;
                isGameClear = true;
                gameEndUI.SetActive(true);
            }
        }
        
    }

}
