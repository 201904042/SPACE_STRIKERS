using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class DB_Firebase
{
    private static DB_Firebase instance = null;
    public static DB_Firebase Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DB_Firebase();
            }
            return instance;
        }
    }

    public static DatabaseReference db_root;
    public const string url = "https://spacestrikers-f12ac-default-rtdb.firebaseio.com/";

    private DB_Firebase()
    {
        Init();
    }

    private void Init()
    {
        db_root = FirebaseDatabase.DefaultInstance.RootReference;
    }

    /// <summary>
    /// 유니코드 이스케이프 시퀀스를 일반 문자열로 변환
    /// </summary>
    public string DecodeUnicodeEscapes(string unicodeEscaped)
    {
        return Regex.Replace(unicodeEscaped, @"\\u([0-9A-Fa-f]{4})", match =>
        {
            char c = (char)Convert.ToInt32(match.Groups[1].Value, 16);
            return c.ToString();
        });
    }

    //파이어베이스에서 해당 이름을 가진 노드의 필드를 불러와 json으로 저장
    public async Task FetchAndSaveDataAsync(string nodeName, string jsonData, string folderPath)
    {
        string formattedJsonData = $"{{ \"{nodeName}\": {jsonData} }}";
        string decodedJsonData = DecodeUnicodeEscapes(formattedJsonData);

        string path = Path.Combine(folderPath, nodeName + ".json");
        string directory = Path.GetDirectoryName(path);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        await File.WriteAllTextAsync(path, decodedJsonData);
        Debug.Log($"Saved {nodeName} data to {path}");

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    public async Task GetGameDataAsync()
    {
        string folderPath = Path.Combine(Application.streamingAssetsPath, "JSON/ReadOnly");

        try
        {
            // Firebase에서 데이터를 비동기적으로 가져옴
            DataSnapshot snapshot = await db_root.Child("ReadOnly").GetValueAsync();

            // 데이터가 존재하는지 확인
            if (snapshot.Exists)
            {
                foreach (DataSnapshot node in snapshot.Children)
                {
                    string nodeName = node.Key;
                    string jsonData = node.GetRawJsonValue();

                    // FetchAndSaveDataAsync 메서드에서 데이터를 파일로 저장하거나 처리하는 로직 수행
                    await FetchAndSaveDataAsync(nodeName, jsonData, folderPath);
                }
            }
            else
            {
                Debug.LogError("No data found in ReadOnly node.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to fetch data: {ex.Message}");
        }
    }


    public async Task GetAccountDataAsync(string accountCode)
    {
        string folderPath = Path.Combine(Application.streamingAssetsPath, "JSON/Writable");

        try
        {
            DataSnapshot snapshot = await db_root.Child("Accounts")
                                                  .OrderByKey()
                                                  .EqualTo(accountCode)
                                                  .GetValueAsync();

            if (snapshot.Exists)
            {
                foreach (DataSnapshot accountSnapshot in snapshot.Children)
                {
                    foreach (DataSnapshot node in accountSnapshot.Children)
                    {
                        string nodeName = node.Key;
                        string jsonData = node.GetRawJsonValue();

                        await FetchAndSaveDataAsync(nodeName, jsonData, folderPath);
                    }
                }
            }
            else
            {
                Debug.LogWarning($"Account with code {accountCode} not found.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error fetching data for account {accountCode}: {ex.Message}");
        }
    }


    public async Task GetBaseAccountDataAsync()
    {
        string folderPath = Path.Combine(Application.streamingAssetsPath, "JSON/Writable");

        try
        {
            // Firebase에서 AccountBase 데이터 가져오기
            var snapshot = await db_root.Child("AccountBase").GetValueAsync();

            if (snapshot.Exists)
            {
                // AccountBase의 자식 노드 순회
                foreach (DataSnapshot node in snapshot.Children)
                {
                    string nodeName = node.Key;
                    string jsonData = node.GetRawJsonValue();

                    // FetchAndSaveDataAsync를 호출하여 파일을 저장
                    await FetchAndSaveDataAsync(nodeName, jsonData, folderPath);
                }
            }
            else
            {
                Debug.LogError("AccountBase 데이터가 존재하지 않습니다.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error fetching data for account: {ex.Message}");
        }
    }


    public static void DeleteJsonData()
    {
        string gameFolderPath = Path.Combine(Application.streamingAssetsPath, "JSON/ReadOnly");
        DeleteAllJsonFiles(gameFolderPath);
        string accountFolder = Path.Combine(Application.streamingAssetsPath, "JSON/Writable");
        DeleteAllJsonFiles(accountFolder);

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    private static void DeleteAllJsonFiles(string folderPath)
    {
        if (Directory.Exists(folderPath))
        {
            string[] jsonFiles = Directory.GetFiles(folderPath, "*.json");

            foreach (string file in jsonFiles)
            {
                File.Delete(file);  // 파일 삭제
            }

            Debug.Log($"{jsonFiles.Length} JSON files deleted from {folderPath}");
        }
        else
        {
            Debug.LogWarning($"Directory not found: {folderPath}");
        }
    }

    public async Task UploadWritableJsonFilesAsync()
    {
        string folderPath = Path.Combine(Application.streamingAssetsPath, "JSON/Writable");

        // 폴더 안의 모든 JSON 파일 목록을 가져옵니다
        string[] jsonFiles = Directory.GetFiles(folderPath, "*.json");
        Debug.Log($"파일 수 : {jsonFiles.Length}");

        // 모든 파일 업로드를 완료할 때까지 기다림
        foreach (string filePath in jsonFiles)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string jsonContent = File.ReadAllText(filePath);

            // Firebase에 업로드
            bool uploadSuccess = await UploadJsonToFirebaseAsync(fileName, jsonContent);

            // 실패한 경우, 에러 로그 출력
            if (!uploadSuccess)
            {
                Debug.LogError($"Failed to upload {fileName} for user {Auth_Firebase.Instance.UserId}");
            }
        }
    }

    private async Task<bool> UploadJsonToFirebaseAsync(string fileName, string jsonContent)
    {
        try
        {
            // Firebase 경로: Accounts/userId/파일명
            var task = db_root.Child("Accounts").Child(Auth_Firebase.Instance.UserId).Child(fileName).SetRawJsonValueAsync(jsonContent);

            // Firebase에 데이터를 업로드 후 완료 대기
            await task;

            Debug.Log($"Successfully uploaded {fileName} for user {Auth_Firebase.Instance.UserId}.");
            return true;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error uploading {fileName} for user {Auth_Firebase.Instance.UserId}: {ex.Message}");
            return false;
        }
    }

}
