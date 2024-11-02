
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

    private readonly PoolManager _pool = new();
    private readonly SpawnManager _spawn = new();
    private readonly StageManager _stage = new();
    private readonly IG_UIManager _ui = new();
    //�ΰ��� �Ŵ�����
    public PoolManager Pool => Game._pool;
    public SpawnManager Spawn => Game._spawn;
    public StageManager Stage => Game._stage;
    public IG_UIManager UI => Game._ui;


    public PlayerMain myPlayer;

    public float stageTime;
    public int minutes;
    public int seconds;
    public int score;

    private bool IG_GameStart; //������ ���۵Ǿ��°� -> ������ ������ ����
    public bool IG_Clear; //Ŭ���� ������ �����Ͽ��°�
    public bool IG_PerfectClear; //�÷��̾ �������� ���� �ʾҳ�

    public Coroutine GameCoroutine = null; //���� ���۽� ����, ���� ����� ����
    public Coroutine SpawnCoroutine = null; //���� ���۽� ����, ������ �����ǰų� ���� ����� ����

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
        Pool.Init();
        UI.Init();

        SpawnPlayer();

        StartCoroutine(CountAndStart());
    }

    //���Ӿ��� init. �÷��̾ ����ϰ� ��� �⹰�� �ʱ�ȭ��
    public void GameInit()
    {
        Time.timeScale = 1;
        
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

        // StartCountdown �ڷ�ƾ�� ���� �� ������ �ڵ�
        Debug.Log("ī��Ʈ�ٿ� �Ϸ�. ���� ����");
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
            SpawnCoroutine = null; // �ڷ�ƾ ������ null�� ����
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
        // ���� ���� ��� : ���� ��� �÷��̾� ��� �� ����, ���� Ŭ���� �� ����
        
        if (!IG_Clear)
        {
            yield return StartCoroutine(myPlayer.PlayerDeadAnim());  // ��� �ִϸ��̼��� ���� ������ ���
        }
        else
        {
            yield return StartCoroutine(myPlayer.PlayerClearAnim());  // Ŭ���� �ִϸ��̼��� ���� ������ ���
        }

        yield return new WaitForSeconds(1); //1�� ����� ���� �������̽� ����
        UI.IGameEnd.OpenInterface();
    }

    private IEnumerator CheckInGameBehavior()
    {
        while (true)
        {
            if (!BattleSwitch) //���� ����ġ�� ON����
            {
                yield return new WaitUntil(() => BattleSwitch == true);
            }

            if (IsPlayerDead() || IsGameClear()) //���� ���� : �÷��̾ ����Ұ�� , ������Ŭ������ ���
            {
                GameEnd();
            }

            if (IsTimerActive())
            {
                Timer(); // �������� �ð� Ÿ�̸� ������Ʈ
            }
            yield return null;
        }
    }


    private GameMode curMode => Stage.curMode;
    //�������� Ŭ���� ���� �޼��� true
    private bool IsGameClear()
    {
        //������Ŭ������ ��� => �Ϲ�: ����� ����, ���� : ���� ����, ���� : 5���̸� Ŭ���� ���� �޼� �ִ� 30��
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
                //���� ����
            }
            if (Spawn.isBossSpawned && Spawn.isBossDown) //�Ϲ� ���� ��� �����߸��� ��������., ������ �����Ǿ��� ������ ����� ���� Ŭ����
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

            if (stageTime == 1800) //30�� =1800��
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

    public void SpawnBossBtn()
    {
        Game.Spawn.SpawnBoss(31);
    }


    //�Ŵ��� ���� ���� ���� ��Ʈ
    public static GameObject InstantObject(GameObject target, Transform parent = null)
    {
        if (target == null)
        {
            Debug.LogError("InstantObject: target is null.");
            return null;
        }

        // parent�� null�� ��� ���� ��Ʈ�� ������
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

    
}
