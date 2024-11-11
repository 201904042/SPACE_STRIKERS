using UnityEngine;
using UnityEngine.Rendering;

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
        if (Auth_Firebase.Instance.user != null)
        {
            Data.Init();
            Debug.Log("�Ŵ����� ������ �ʱ�ȭ");
        }
    }

    public static T LoadInResources<T>(string path) where T : UnityEngine.Object
    {
        T instance = Resources.Load<T>(path);
        if(instance == null)
        {
            Debug.Log($"���ҽ� ���� {path}");
            return null;
        }
        return instance;
    }

    private readonly DataManager _data = new();
    public DataManager Data => Instance._data;

   
}
