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

    private bool isBattleStart; //������ ���۵Ǿ��°� -> ������ ������ ����
    public bool isGameClear; //Ŭ���� ������ �����Ͽ��°�
    public bool isPerfectClear; //�÷��̾ �������� ���� �ʾҳ�

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
        Managers.Instance.GameSceneInit(); //��������, Ǯ, ���� �Ŵ��� �ʱ�ȭ
    }

    //���Ӿ��� init. �÷��̾ ����ϰ� ��� �⹰�� �ʱ�ȭ��
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
            if (myPlayer.GetComponent<PlayerStat>().curHp <= 0) //�÷��̾��� hp�� 0�� �ȴٸ� ���� ����
            {
                Time.timeScale = 0;
                gameEndUI.SetActive(true);
                yield break;
            }

            if (!IsTimerActive())
            {
                //������ �������� ���� ��쿡�� Ÿ�̸� ���ư�
                Timer(); //�������� �ð� Ÿ�̸� ������Ʈ
            }

            if (Managers.Instance.Stage.isBossStage)
            {
                //10���� �� ���¿��� ������ �������� ���� ������ ���� �ʾҴٸ� ������ �����Ѵ�
                if (minutes == 10 && !Managers.Instance.Spawn.isBossSpawned && !Managers.Instance.Spawn.isBossDown)
                {
                    Managers.Instance.Spawn.SpawnBoss(Managers.Instance.Stage.stageBossId);
                }

                if (!isGameClear && Managers.Instance.Spawn.isBossDown) //������������ ���� 10���� ������ �����ϰ� �� ������ óġ�ϸ� �¸�. �� ��� ��������
                {
                    isGameClear = true;
                    Time.timeScale = 0;
                    gameEndUI.SetActive(true);
                }
            }
            else
            {
                if (!isGameClear && minutes >= 10) //����������� 10�� �̻� ��ƾ�ٸ� �������� �¸�. �÷��̾ �װų� �׸��Ѷ����� ����
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
