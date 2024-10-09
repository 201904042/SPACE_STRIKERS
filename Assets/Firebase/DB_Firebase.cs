using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class DB_Firebase
{
    public static DatabaseReference db_root;
    public string url = "https://spacestrikers-f12ac-default-rtdb.firebaseio.com/";

    private void Init()
    {
        db_root = FirebaseDatabase.DefaultInstance.RootReference;
    }

    /// <summary>
    /// 유니코드 이스케이프 시퀀스를 일반 문자열로 변환
    /// </summary>
    public static string DecodeUnicodeEscapes(string unicodeEscaped)
    {
        return Regex.Replace(unicodeEscaped, @"\\u([0-9A-Fa-f]{4})", match =>
        {
            char c = (char)Convert.ToInt32(match.Groups[1].Value, 16);
            return c.ToString();
        });
    }

    /// <summary>
    /// Firebase에서 데이터를 가져와서 JSON 파일로 저장
    /// </summary>
    public static void FetchAndSaveData(string nodeName, string jsonData, string folderPath)
    {
        string formattedJsonData = $"{{ \"{nodeName}\": {jsonData} }}";
        string decodedJsonData = DecodeUnicodeEscapes(formattedJsonData);

        // 경로 설정 및 디렉토리 확인
        string path = Path.Combine(folderPath, nodeName + ".json");
        string directory = Path.GetDirectoryName(path);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        // 파일 저장
        File.WriteAllText(path, decodedJsonData);
        Debug.Log($"Saved {nodeName} data to {path}");

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    /// <summary>
    /// 공통 데이터(ReadOnly)를 Firebase에서 읽어와 JSON 파일로 저장
    /// </summary>
    public static void GetGameData()
    {
        string folderPath = Path.Combine(Application.streamingAssetsPath, "JSON/ReadOnly");

        db_root.Child("ReadOnly").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot node in snapshot.Children)
                {
                    string nodeName = node.Key;
                    string jsonData = node.GetRawJsonValue();
                    FetchAndSaveData(nodeName, jsonData, folderPath);
                }
            }
            else
            {
                Debug.LogError($"Failed to fetch data: {task.Exception}");
            }
        });
    }

    /*
     * 현재 계획
     * root 하위의 READONLY노드에 게임운영에 필요한 데이터들을 ACCOUNTS노드에 각 플레이어들의 노드들을
     * 플레이어 노드의 하위에는 각 플레이어별 코드를 이름으로하는 노드들을 이루어져 있고 해당 노드 아래에는 계정별로 writable 데이터들이 있다.
     */



    /// <summary>
    /// 특정 accountCode에 해당하는 계정 데이터를 Firebase에서 읽어와 JSON 파일로 저장
    /// </summary>
    public void GetAccountData(string accountCode)
    {
        string folderPath = Path.Combine(Application.streamingAssetsPath, "JSON/Writable");

        db_root.Child("Account")
            .OrderByKey()
            .EqualTo(accountCode)
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    foreach (DataSnapshot node in snapshot.Children)
                    {
                        string nodeName = node.Key;
                        string jsonData = node.GetRawJsonValue();
                        FetchAndSaveData(nodeName, jsonData, folderPath);
                    }
                }
                else
                {
                    Debug.LogError($"Failed to fetch data for account {accountCode}: {task.Exception}");
                }
            });
    }

    /// <summary>
    /// 파일에서 JSON 데이터를 읽어옵니다.
    /// </summary>
    public static string LoadJsonFromFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            return File.ReadAllText(filePath);
        }
        else
        {
            Debug.LogError($"File not found: {filePath}");
            return null;
        }
    }

    /// <summary>
    /// 특정 노드의 데이터를 현재 저장된 JSON 파일로 업데이트
    /// </summary>
    public static void UpdateFirebaseNodeFromJson(string accountCode,string nodeName, string JsonPath)
    {
        DatabaseReference db_root = FirebaseDatabase.DefaultInstance.RootReference;
        string jsonData = LoadJsonFromFile(JsonPath);
        if (jsonData != null)
        {
            // Firebase의 특정 노드를 업데이트
            db_root.Child("Account").Child(accountCode).Child(nodeName)
                .SetRawJsonValueAsync(jsonData)
                .ContinueWithOnMainThread(task =>
                {
                    if (task.IsCompleted)
                    {
                        Debug.Log($"Successfully updated {nodeName} node with local JSON data.");
                    }
                    else
                    {
                        Debug.LogError($"Failed to update {nodeName} node: {task.Exception}");
                    }
                });
        }
    }
}
