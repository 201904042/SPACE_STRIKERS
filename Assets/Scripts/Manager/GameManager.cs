using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Pool;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private readonly PoolManager _pool = new();
    private readonly SpawnManager _spawn = new();
    private readonly StageManager _stage = new();

    public PoolManager Pool => Instance._pool;
    public SpawnManager Spawn => Instance._spawn;
    public StageManager Stage => Instance._stage;


    public GameObject myPlayer;

    public GameObject gameEndUI;

    public float stageTime;
    public float minutes;
    public float seconds;

    public int score;

    private bool isBattleStart; //게임이 시작되었는가 -> 적들의 스폰을 시작
    public bool isGameClear; //클리어 조건을 만족하였는가
    public bool isPerfectClear; //플레이어가 데미지를 입지 않았나

    public Coroutine GameCoroutine = null;
    public Coroutine SpawnCoroutine = null;
    public bool BattleSwitch
    {
        get => isBattleStart;
        set => isBattleStart = value;
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        GameInit();

        //매니저 스크립트 초기화
        Stage.Init();
        Spawn.Init();
        Pool.Init();

        SpawnPlayer();
        Debug.Log("게임 초기화 완료");
    }

    private void SpawnPlayer()
    {
        GameObject Player = Resources.Load<GameObject>("Prefab/Player/Player");
        Transform SpawnTransform = GameObject.Find("SpawnZone").transform.GetChild(4);
        myPlayer = Instantiate(Player);
        myPlayer.transform.position = SpawnTransform.position;
        myPlayer.transform.rotation = SpawnTransform.rotation;
    }

    //게임씬의 init. 플레이어를 등록하고 모든 기물을 초기화함
    public void GameInit()
    {
        

        Time.timeScale = 1;
        score = 0;
        stageTime = 0;
        minutes = 0;
        seconds = 0;

        BattleSwitch = false;
        isGameClear = false;
    }

    [ContextMenu("BattleStart")]
    public void BattleStart()
    {
        BattleSwitch = true;
        if (GameCoroutine == null)
        {
            GameCoroutine = StartCoroutine(PlayGame());
        }
    }


    [ContextMenu("BattleStop")]
    public void BattleStop()
    {
        BattleSwitch = false;
    }

    private IEnumerator PlayGame()
    {
        while (true)
        {
            if(!BattleSwitch) //종료 조건
            {
                if (SpawnCoroutine != null)
                {
                    StopCoroutine(SpawnCoroutine);
                    SpawnCoroutine = null;
                }

                yield break;
            }

            if (SpawnCoroutine == null)
            {
                SpawnCoroutine = StartCoroutine(GameManager.Instance.Spawn.SpawnEnemyTroops());
            }

            CheckPlayerHp(); //플레이어의 체력 확인. 0이면 게임 종료

            if (GameManager.Instance.Stage.isBossStage)
            {
                HandleBossStage();
            }
            else
            {
                HandleNormalStage();
            }


            if (!IsTimerActive())
            {
                Timer(); // 스테이지 시간 타이머 업데이트
            }

            yield return null;
        }
    }

    private void CheckPlayerHp()
    {
        if (myPlayer.GetComponent<PlayerStat>().curHp <= 0)
        {
            Time.timeScale = 0;
            gameEndUI.SetActive(true);
            StopCoroutine(PlayGame()); // 코루틴 종료
        }
    }

    private void HandleBossStage()
    {
        if (minutes == 10 && !GameManager.Instance.Spawn.isBossSpawned && !GameManager.Instance.Spawn.isBossDown)
        {
            GameManager.Instance.Spawn.SpawnBoss(GameManager.Instance.Stage.stageBossId);
        }

        if (!isGameClear && GameManager.Instance.Spawn.isBossDown)
        {
            isGameClear = true;
            Time.timeScale = 0;
            gameEndUI.SetActive(true);
        }
    }

    private void HandleNormalStage()
    {
        if (!isGameClear && minutes >= 10)
        {
            isGameClear = true;
        }
    }



    private bool IsTimerActive()
    {
        bool active = true;

        if (GameManager.Instance.Spawn.isBossSpawned)
        {
            active = false;
        }

        return active;
    }

    public void Timer()
    {
        stageTime += Time.deltaTime;
        minutes = Mathf.FloorToInt(stageTime / 60f);
        seconds = Mathf.FloorToInt(stageTime % 60f);
    }

    public void SpawnBossBtn()
    {
        GameManager.Instance.Spawn.SpawnBoss(31);
    }
}
