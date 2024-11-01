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

    private readonly DataManager _data = new();
    public DataManager Data => Instance._data;

   
}
