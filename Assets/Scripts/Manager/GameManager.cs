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
        set
        {
            isBattleStart = value;
            if (isBattleStart)
            {
                
                if (SpawnCoroutine != null)
                {
                    StopCoroutine(SpawnCoroutine);
                }
                SpawnCoroutine = StartCoroutine(Managers.Instance.Spawn.SpawnEnemyTroops());
                
            }
            else
            {
                if (SpawnCoroutine != null)
                {
                    StopCoroutine(SpawnCoroutine);
                    SpawnCoroutine = null;
                }
            }
        }
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SetGame();
        Managers.Instance.GameSceneInit(); //스테이지, 풀, 스폰 매니저 초기화
    }

    //게임씬의 init. 플레이어를 등록하고 모든 기물을 초기화함
    public void SetGame()
    {
        myPlayer = GameObject.FindWithTag("Player");
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
        while(isBattleStart)
        {
            if (myPlayer.GetComponent<PlayerStat>().curHp <= 0) //플레이어의 hp가 0이 된다면 게임 종료
            {
                Time.timeScale = 0;
                gameEndUI.SetActive(true);
                yield break;
            }

            if (!IsTimerActive())
            {
                //보스가 스폰되지 않을 경우에만 타이머 돌아감
                Timer(); //스테이지 시간 타이머 업데이트
            }

            if (Managers.Instance.Stage.isBossStage)
            {
                //10분이 된 상태에서 보스가 스폰된적 없고 보스가 죽지 않았다면 보스를 스폰한다
                if (minutes == 10 && !Managers.Instance.Spawn.isBossSpawned && !Managers.Instance.Spawn.isBossDown)
                {
                    Managers.Instance.Spawn.SpawnBoss(Managers.Instance.Stage.stageBossId);
                }

                if (!isGameClear && Managers.Instance.Spawn.isBossDown) //보스전에서는 시작 10분후 보스가 등장하고 그 보스를 처치하면 승리. 그 즉시 게임종료
                {
                    isGameClear = true;
                    Time.timeScale = 0;
                    gameEndUI.SetActive(true);
                }
            }
            else
            {
                if (!isGameClear && minutes >= 10) //잡몹전에서는 10분 이상 버틴다면 스테이지 승리. 플레이어가 죽거나 그만둘때까지 무한
                {
                    isGameClear = true;
                }
            }
        }
    }
    

    private bool IsTimerActive()
    {
        bool active = true;

        if (Managers.Instance.Spawn.isBossSpawned)
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
        Managers.Instance.Spawn.SpawnBoss(31);
    }
}
