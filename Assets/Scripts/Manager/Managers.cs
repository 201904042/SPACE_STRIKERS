using UnityEngine;

public class Managers : MonoBehaviour
{
    public static Managers Instance;

    public void Awake()
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

        DefaultInit();
    }

    private void DefaultInit()
    {
        Data.Init(); //데이터외에는 게임씬에 들어갈때 초기화 시킬것
    }

    public void GameSceneInit()
    {
        Stage.Init();
        Spawn.Init();
        Pool.Init();
    }

    private readonly DataManager _data = new();
    private readonly PoolManager _pool = new();
    private readonly SpawnManager _spawn = new();
    private readonly StageManager _stage = new();

    public DataManager Data => Instance._data;
    public PoolManager Pool => Instance._pool;
    public SpawnManager Spawn => Instance._spawn;
    public StageManager Stage => Instance._stage;
}
