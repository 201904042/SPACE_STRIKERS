using UnityEngine;
using UnityEngine.Rendering;

public class Managers : MonoBehaviour
{
    public static Managers Instance;

    private readonly DataManager _data = new();
    private readonly Auth_Firebase _auth = new();
    private readonly DB_Firebase _db = new();
    public DataManager Data => Instance._data;
    public Auth_Firebase FB_Auth => Instance._auth;
    public DB_Firebase FB_Db => Instance._db;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            DefaultInit();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private async void DefaultInit()
    {
        if (FB_Auth.user == null)
        {
            FB_Auth.Init();
        }

        await FB_Db.Init();

        if (!Data.isDone)
        {
            Data.Init();
            Debug.Log("매니저스 데이터 초기화");
        }
    }

    public static T LoadInResources<T>(string path) where T : UnityEngine.Object
    {
        T instance = Resources.Load<T>(path);
        if(instance == null)
        {
            Debug.Log($"리소스 오류 {path}");
            return null;
        }
        return instance;
    }

    [ContextMenu("읽기전용 데이터 업로드")]
    public async void UploadReadOnlyData()
    {
        await FB_Db.UploadAllReadOnlyJsonFilesAsync();
    }

    [ContextMenu("쓰기 데이터 업로드")]
    public async void UploadWritableData()
    {
        await FB_Db.UploadAllWritableJsonFilesAsync();
    }

    [ContextMenu("읽기전용 데이터 다운로드")]
    public async void DownLoadReadOnlyData()
    {
        await FB_Db.GetGameDataAsync();
    }

    [ContextMenu("쓰기 데이터 다운로드")]
    public async void DownLoadWritableData()
    {
        await FB_Db.GetAccountDataAsync(FB_Auth.UserId);
    }

    [ContextMenu("쓰기 파일들 삭제")]
    public void DeleteWritableData()
    {
        DB_Firebase.DeleteAccountData();
    }
}
