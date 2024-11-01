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
        Data.Init(); //�����Ϳܿ��� ���Ӿ��� ���� �ʱ�ȭ ��ų��
    }

    private readonly DataManager _data = new();
    public DataManager Data => Instance._data;

   
}
