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
    public GameObject myPlayer;
    public GameObject gameEndUI;

    [Header("오브젝트 풀 관련")]
    public ObjectPool<GameObject> enemyPool;

    public int score;

    public bool isBattleStart; //게임이 시작되었는가 -> 적들의 스폰을 시작
    public bool isGameClear; //클리어 조건을 만족하였는가
    public bool isPerfectClear; //플레이어가 데미지를 입지 않았나

    public Coroutine SpawnCoroutine;
    public bool isBattleOn
    {
        get => isBattleStart;
        set
        {
            isBattleStart = value;
            if (isBattleStart)
            {
                SpawnCoroutine = StartCoroutine(SpawnManager.spawnInstance.SpawnEnemyTroops());
                SpawnManager.spawnInstance.SpawnBoss(31); //임시로 시작하자마자 보스 스폰
            }
            else
            {
                if(SpawnCoroutine == null)
                {
                    return;
                }
                StopCoroutine(SpawnCoroutine);
            }
        }
    }

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
        myPlayer = GameObject.FindWithTag("Player");
        Time.timeScale = 1;
    }

    private void Start()
    {
        score = 0;
        isBattleOn = true;
        isGameClear = false;
    }

    private void Update()
    {
        if (isBattleStart)
        {
            if (!SpawnManager.spawnInstance.isBossSpawned)
            {
                //보스가 스폰되지 않을 경우에만 타이머 돌아감
                StageManager.stageInstance.StageTimer(); //스테이지 시간 타이머 업데이트
            }

            if (myPlayer.GetComponent<PlayerStat>().curHp <= 0) //플레이어의 hp가 0이 된다면 게임 종료
            {
                Time.timeScale = 0;
                gameEndUI.SetActive(true);
            }

            if (StageManager.stageInstance.isBossStage)
            {
                //10분이 된 상태에서 보스가 스폰된적 없고 보스가 죽지 않았다면 보스를 스폰한다
                if(StageManager.stageInstance.minutes == 10 && !SpawnManager.spawnInstance.isBossSpawned && !SpawnManager.spawnInstance.isBossDown)
                {
                    SpawnManager.spawnInstance.SpawnBoss(StageManager.stageInstance.stageBossId);
                }

                if (!isGameClear && SpawnManager.spawnInstance.isBossDown) //보스전에서는 시작 10분후 보스가 등장하고 그 보스를 처치하면 승리. 그 즉시 게임종료
                {
                    isGameClear = true;
                    Time.timeScale = 0;
                    gameEndUI.SetActive(true);
                }
            }
            else
            {
                if (!isGameClear && StageManager.stageInstance.minutes >= 10) //잡몹전에서는 10분 이상 버틴다면 스테이지 승리. 플레이어가 죽거나 그만둘때까지 무한
                {
                    isGameClear = true;
                }
            }
        }
    }
}
