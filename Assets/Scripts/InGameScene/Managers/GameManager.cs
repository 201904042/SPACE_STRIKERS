using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public enum GameMode
{
    None = 0,
    Common = 1,
    Boss = 2,
    Infinite = 3
}

public class GameManager : MonoBehaviour
{
    public static GameManager Game { get; private set; }
    public static Transform Canvas { get; private set; }

    private readonly PoolManager _pool = new();
    private readonly SpawnManager _spawn = new();
    private readonly StageManager _stage = new();
    private readonly IG_UIManager _ui = new();

    public PoolManager Pool => _pool;
    public SpawnManager Spawn => _spawn;
    public StageManager Stage => _stage;
    public IG_UIManager UI => _ui;

    public PlayerMain MyPlayer { get; private set; }

    public float StageTime { get; private set; }
    public int minutes => Mathf.FloorToInt(StageTime / 60f);
    public int seconds => Mathf.FloorToInt(StageTime % 60f);
    public int phase { get; private set; }

    private int _score;
    public int Score
    {
        get => _score;
        set
        {
            _score = value;
            UI.SetScoreText(value);
        }
    }
    [SerializeField]
    private bool _isGameStarted;
    public bool IsClear { get; private set; }
    public bool IsPerfectClear { get; private set; }

    public Coroutine GameCoroutine { get; private set; }
    public Coroutine SpawnCoroutine { get; private set; }
    public Transform TriggerCheck { get; private set; }

    public bool BattleSwitch
    {
        get => _isGameStarted;
        set => _isGameStarted = value;
    }

    private void Awake()
    {
        if (Game == null)
        {
            Game = this;
            StartCoroutine(InitializeGame());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region Init
    private IEnumerator InitializeGame()
    {
        Canvas = GameObject.Find("Canvas")?.transform;
        TriggerCheck = GameObject.Find("TriggerCheck")?.transform;
        Restart();

        yield return new WaitUntil(() => Managers.Instance.Data.isDone == true);

        _stage.Init();
        _spawn.Init();
        _ui.Init();
        _pool.Init();

        yield return StartCoroutine(SpawnPlayer());
        yield return new WaitUntil(() => MyPlayer.GetComponent<PlayerMain>().isPlayerSetDone);
        GameStart();
    }

    private IEnumerator SpawnPlayer()
    {
        GameObject playerPrefab = Resources.Load<GameObject>("Prefab/Player/Player");
        Transform spawnTransform = TriggerCheck?.Find("SpawnZone")?.GetChild(0);
        if (playerPrefab == null || spawnTransform == null)
        {
            Debug.LogError("플레이어 생성에러");
            yield break;
        }

        MyPlayer = Instantiate(playerPrefab, spawnTransform.position, spawnTransform.rotation).GetComponent<PlayerMain>();
    }

    public void GameStart()
    {
        BattleSwitch = true;
        GameCoroutine ??= StartCoroutine(InGameRoutine());
    }

    #endregion

    private IEnumerator InGameRoutine()
    {
        //인게임 데이터 초기화
        Score = 0;
        StageTime = 0;
        phase = 1;
        BattleSwitch = false;
        IsClear = false;
        
        //카운트 다운 후 시작
        yield return StartCoroutine(UI.IStartCount.StartCountdown());
        BattleSwitch = true;

        StartSpawningEnemies();

        while(true)
        {
            yield return new WaitUntil(() => BattleSwitch);

            if (IsPlayerDead()) //플레이어가 죽으면 종료 //게임이 클리어되면 종료
            {
                break;
            }

            if (IsGameClear()) //플레이어가 죽으면 종료 //게임이 클리어되면 종료
            {
                break;
            }


            if (IsTimerActive())
                UpdateStageTimer();
            yield return null;
        }

        BattleSwitch = false;
        yield return StartCoroutine(EndGameSequence());
    }

    private void StartSpawningEnemies()
    {
        Spawn.StartSpawnEnemy();
    }

    private void StopSpawningEnemies()
    {
        Spawn.StopSpawnEnemy();
    }

    public IEnumerator EndGameSequence()
    {
        StopSpawningEnemies();
        Spawn.DeleteAllEnemy();

        yield return IsClear ? StartCoroutine(MyPlayer.PlayerClearAnim()) : StartCoroutine(MyPlayer.PlayerDeadAnim());
        yield return new WaitForSeconds(1);

        UI.IGameEnd.OpenInterface();
        Pause();
    }

    private GameMode CurrentGameMode => _stage.curMode;

    private bool IsGameClear()
    {
        switch (CurrentGameMode)
        {
            case GameMode.Common:
                return CheckCommonClearCondition();
            case GameMode.Boss:
                return CheckBossClearCondition();
            case GameMode.Infinite:
                return CheckInfiniteClearCondition();
            default:
                Debug.LogError("Game mode not set.");
                return true;
        }
    }

    private bool CheckCommonClearCondition()
    {
        if (_stage.curEnemyAmount == 0)
        {
            IsClear = true;
            return true;
        }
        return false;
    }

    private bool CheckBossClearCondition()
    {
        if (_stage.curEnemyAmount == 0 && !_spawn.isBossSpawned && !_spawn.isBossDown)
        {
            StopSpawningEnemies();
            _spawn.SpawnStageBoss();
        }

        if (_spawn.isBossSpawned && _spawn.isBossDown)
        {
            IsClear = true;
            return true;
        }
        return false;
    }

    private bool CheckInfiniteClearCondition()
    {
        if (StageTime >= (phase) * 300)
        {
            IsClear = true;
            Spawn.SpawnStageBoss();
        }

        return StageTime >= 1800f; // Ends game at 30 minutes
    }

    private bool IsPlayerDead() 
    {
        return PlayerMain.pStat.IG_Life <= 0;
    }

    private bool IsTimerActive() => !Spawn.isBossSpawned || !BattleSwitch;

    private void UpdateStageTimer()
    {
        StageTime += Time.deltaTime;

        CheckGamePhase();
        UI.SetTimeText(minutes, seconds);
    }

    private void CheckGamePhase()
    {
        if (StageTime >= (phase) * 300)
        {
            phase++;
            //OnPhaseChange(); 페이즈 증가시의 행동
        }
    }

    #region Utility Methods
    public static GameObject InstantiateObject(GameObject prefab, Transform parent = null)
    {
        if (prefab == null)
        {
            Debug.LogError("InstantiateObject: prefab is null.");
            return null;
        }
        return Instantiate(prefab, parent);
    }

    public static void DestroyObject(GameObject target)
    {
        if (target != null)
            Destroy(target);
    }

    public static T LoadFromResources<T>(string path) where T : UnityEngine.Object
    {
        T asset = Resources.Load<T>(path);
        if (asset == null)
            Debug.LogError($"LoadFromResources: No asset found at path '{path}'.");
        return asset;
    }

    public Coroutine StartOtherManagerCoroutine(IEnumerator coroutine)
    {
        Coroutine co = StartCoroutine(coroutine);
        return co;
    }

    public void StopCoroutineSafely(Coroutine coroutine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }
    #endregion

    #region 임시 벝튼
    [ContextMenu("Pause")]
    public void Pause()
    {
        Time.timeScale = 0;
    }

    [ContextMenu("Restart")]
    public void Restart()
    {
        Time.timeScale = 1;
    }

    [ContextMenu("Spawn Boss")]
    public void SpawnBoss()
    {
        StopSpawningEnemies();
        Spawn.SpawnBossById(531);
    }

    [ContextMenu("Victory")]
    public void Victory()
    {
        IsClear = true;
        BattleSwitch = false;
        StartCoroutine(EndGameSequence());
    }

    #endregion
}
