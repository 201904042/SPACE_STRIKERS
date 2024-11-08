
using System.Collections;
using UnityEngine;

public enum GameMode
{
    None = 0,
    Common = 1,
    Boss = 2 ,
    Infinite = 3
}

public class GameManager : MonoBehaviour
{
    public static GameManager Game;
    public static Transform canvas;

    private readonly PoolManager _pool = new();
    private readonly SpawnManager _spawn = new();
    private readonly StageManager _stage = new();
    private readonly IG_UIManager _ui = new();
    //인게임 매니저들
    public PoolManager Pool => Game._pool;
    public SpawnManager Spawn => Game._spawn;
    public StageManager Stage => Game._stage;
    public IG_UIManager UI => Game._ui;


    public PlayerMain myPlayer;

    public float stageTime;
    public int minutes;
    public int seconds;
    public int phase; // 5분단위로 페이즈 변화. 증폭량 : 1:100, 2:150, 3:200, 4:250, 5:300, 6:500
    private int score;
    public int Score
    {
        get => score;
        set
        {
            score = value;
            UI.SetScoreText(score);
        }
    }

    private bool IG_GameStart; //게임이 시작되었는가 -> 적들의 스폰을 시작
    public bool IG_Clear; //클리어 조건을 만족하였는가
    public bool IG_PerfectClear; //플레이어가 데미지를 입지 않았나

    public Coroutine GameCoroutine = null; //게임 시작시 시작, 게임 종료시 종료
    public Coroutine SpawnCoroutine = null; //게임 시작시 시작, 보스가 생성되거나 게임 종료시 종료

    public Transform TriggerCheck;

    public bool BattleSwitch
    {
        get => IG_GameStart;
        set => IG_GameStart = value;
    }

    private void Awake()
    {
        if (Game == null)
        {
            Game = this;
        }
        else
        {
            Destroy(gameObject);
        }

        GameInit();

        Stage.Init();
        Spawn.Init();
        UI.Init();

        Pool.Init();
        SpawnPlayer();

        //StartCoroutine(CountAndStart());
    }

    //게임씬의 init. 플레이어를 등록하고 모든 기물을 초기화함
    public void GameInit()
    {
        Time.timeScale = 1;
        canvas = GameObject.Find("Canvas").transform; //없으면 로드해서 생성
        score = 0;
        stageTime = 0;
        minutes = 0;
        seconds = 0;

        if (GameCoroutine != null)
        {
            StopCoroutine(GameCoroutine);
            GameCoroutine = null;
        }
        if (SpawnCoroutine != null)
        {
            StopCoroutine(SpawnCoroutine);
            SpawnCoroutine = null;
        }

        BattleSwitch = false;
        IG_Clear = false;

        TriggerCheck = GameObject.Find("TriggerCheck").transform;
    }

    private void SpawnPlayer()
    {
        GameObject Player = Resources.Load<GameObject>("Prefab/Player/Player");
        Transform SpawnTransform = TriggerCheck.Find("SpawnZone").transform.GetChild(0);
        myPlayer = Instantiate(Player).GetComponent<PlayerMain>();
        myPlayer.transform.position = SpawnTransform.position;
        myPlayer.transform.rotation = SpawnTransform.rotation;
    }

    private IEnumerator CountAndStart()
    {
        yield return StartCoroutine(UI.IStartCount.StartCountdown());

        // StartCountdown 코루틴이 끝난 후 실행할 코드
        Debug.Log("카운트다운 완료. 게임 시작");
        GameStart();
    }

    [ContextMenu("GameStart")]
    public void GameStart()
    {
        BattleSwitch = true;
        if (GameCoroutine == null)
        {
            GameCoroutine = StartCoroutine(CheckInGameBehavior());
        }
        StartSpawnTroop();
    }

    public void StartSpawnTroop()
    {
        if (SpawnCoroutine == null)
        {
            SpawnCoroutine = StartCoroutine(Spawn.SpawnEnemyTroops());
        }
    }
    public void StopSpawnTroop()
    {
        if (SpawnCoroutine != null)
        {

            StopCoroutine(SpawnCoroutine);
            SpawnCoroutine = null; // 코루틴 참조를 null로 설정
        }
    }


    [ContextMenu("Pause")]
    public void Pause()
    {
        BattleSwitch = false;
    }

    public void GameEnd()
    {
        BattleSwitch = false;

        if (GameCoroutine != null)
        {
            StopCoroutine(GameCoroutine);
        }
        if (SpawnCoroutine != null)
        {
            StopCoroutine(SpawnCoroutine);
        }

        StartCoroutine(EndGameSequence());
    }

    private IEnumerator EndGameSequence()
    {
        // 게임 종료 모션 : 예를 들어 플레이어 사망 시 폭발, 게임 클리어 시 전진
        
        if (!IG_Clear)
        {
            yield return StartCoroutine(myPlayer.PlayerDeadAnim());  // 사망 애니메이션이 끝날 때까지 대기
        }
        else
        {
            yield return StartCoroutine(myPlayer.PlayerClearAnim());  // 클리어 애니메이션이 끝날 때까지 대기
        }

        yield return new WaitForSeconds(1); //1초 대기후 종료 인터페이스 열기
        UI.IGameEnd.OpenInterface();
    }

    private IEnumerator CheckInGameBehavior()
    {
        while (true)
        {
            if (!BattleSwitch) //게임 스위치가 ON인지
            {
                yield return new WaitUntil(() => BattleSwitch == true);
            }

            if (IsPlayerDead() || IsGameClear()) //게임 종료 : 플레이어가 사망할경우 , 게임을클리어할 경우
            {
                GameEnd();
            }

            if (IsTimerActive())
            {
                Timer(); // 스테이지 시간 타이머 업데이트
            }
            yield return null;
        }
    }


    private GameMode curMode => Stage.curMode;
    //스테이지 클리어 조건 달성시 true
    private bool IsGameClear()
    {
        //게임을클리어할 경우 => 일반: 모든적 섬멸, 보스 : 보스 제거, 무한 : 5분이면 클리어 조건 달성 최대 30분
        if (curMode == GameMode.Common)
        {
            if (Stage.curEnemyAmount == 0)
            {
                IG_Clear = true;
                return true;
            }
        }
        else if (curMode == GameMode.Boss)
        {
            if (Stage.curEnemyAmount == 0)
            {
                //보스 스폰
            }
            if (Spawn.isBossSpawned && Spawn.isBossDown) //일반 적을 모두 쓰러뜨리면 보스생성., 보스가 생성되었고 보스를 섬멸시 게임 클리어
            {
                IG_Clear = true;
                return true;
            }
        }
        else
        {
            if(stageTime == 300)
            {
                IG_Clear = true;
            }

            if (stageTime == 1800) //30분 =1800초
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsPlayerDead()
    {
        return PlayerMain.pStat.CurHp <= 0;
    }

    private bool IsTimerActive()
    {
        bool active = true;

        if (Game.Spawn.isBossSpawned)
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

        UI.SetTimeText(minutes, seconds);
    }

    //매니저 전용 생성 삭제 루트
    public static GameObject InstantObject(GameObject target, Transform parent = null)
    {
        if (target == null)
        {
            Debug.LogError("InstantObject: target is null.");
            return null;
        }

        // parent가 null일 경우 씬의 루트에 생성됨
        return Instantiate(target, parent);
    }

    public static void DestroyObject(GameObject target)
    {
        Destroy(target.gameObject);
    }


    public static T LoadFromResources<T>(string path) where T : Object
    {
        T instance = Resources.Load<T>(path);
        if (instance == null)
        {
            Debug.LogError($"LoadFromResources: No asset found at path '{path}'.");
        }
        return instance;
    }

    public void StartGameCoroutine(IEnumerator target)
    {
        StartCoroutine(target);
    }

    [ContextMenu("보스스폰")]
    public void SpawnBossBtn()
    {
        Game.Spawn.SpawnBoss(531);
    }

}
