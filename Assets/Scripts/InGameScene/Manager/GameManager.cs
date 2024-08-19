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

    [Header("������Ʈ Ǯ ����")]
    public ObjectPool<GameObject> enemyPool;

    public int score;

    public bool isBattleStart; //������ ���۵Ǿ��°� -> ������ ������ ����
    public bool isGameClear; //Ŭ���� ������ �����Ͽ��°�
    public bool isPerfectClear; //�÷��̾ �������� ���� �ʾҳ�

    public Coroutine SpawnCoroutine = null;
    public bool isBattleOn
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
                SpawnCoroutine = StartCoroutine(SpawnManager.spawnInstance.SpawnEnemyTroops());
                
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
        isBattleOn = false;
        isGameClear = false;
    }

    [ContextMenu("BattleStart")]
    public void BattleStart()
    {
        isBattleOn = true;
    }

    private void Update()
    {
        if (isBattleStart)
        {
            if (!SpawnManager.spawnInstance.isBossSpawned)
            {
                //������ �������� ���� ��쿡�� Ÿ�̸� ���ư�
                StageManager.stageInstance.StageTimer(); //�������� �ð� Ÿ�̸� ������Ʈ
            }

            if (myPlayer.GetComponent<PlayerStat>().curHp <= 0) //�÷��̾��� hp�� 0�� �ȴٸ� ���� ����
            {
                Time.timeScale = 0;
                gameEndUI.SetActive(true);
            }

            if (StageManager.stageInstance.isBossStage)
            {
                //10���� �� ���¿��� ������ �������� ���� ������ ���� �ʾҴٸ� ������ �����Ѵ�
                if(StageManager.stageInstance.minutes == 10 && !SpawnManager.spawnInstance.isBossSpawned && !SpawnManager.spawnInstance.isBossDown)
                {
                    SpawnManager.spawnInstance.SpawnBoss(StageManager.stageInstance.stageBossId);
                }

                if (!isGameClear && SpawnManager.spawnInstance.isBossDown) //������������ ���� 10���� ������ �����ϰ� �� ������ óġ�ϸ� �¸�. �� ��� ��������
                {
                    isGameClear = true;
                    Time.timeScale = 0;
                    gameEndUI.SetActive(true);
                }
            }
            else
            {
                if (!isGameClear && StageManager.stageInstance.minutes >= 10) //����������� 10�� �̻� ��ƾ�ٸ� �������� �¸�. �÷��̾ �װų� �׸��Ѷ����� ����
                {
                    isGameClear = true;
                }
            }
        }

    }

    public void SpawnBossBtn()
    {
        SpawnManager.spawnInstance.SpawnBoss(31);
    }
}
