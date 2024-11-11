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
    /// �����ڵ� �̽������� �������� �Ϲ� ���ڿ��� ��ȯ
    /// </summary>
    public string DecodeUnicodeEscapes(string unicodeEscaped)
    {
        return Regex.Replace(unicodeEscaped, @"\\u([0-9A-Fa-f]{4})", match =>
        {
            char c = (char)Convert.ToInt32(match.Groups[1].Value, 16);
            return c.ToString();
        });
    }

    //���̾�̽����� �ش� �̸��� ���� ����� �ʵ带 �ҷ��� json���� ����
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
            // Firebase���� �����͸� �񵿱������� ������
            DataSnapshot snapshot = await db_root.Child("ReadOnly").GetValueAsync();

            // �����Ͱ� �����ϴ��� Ȯ��
            if (snapshot.Exists)
            {
                foreach (DataSnapshot node in snapshot.Children)
                {
                    string nodeName = node.Key;
                    string jsonData = node.GetRawJsonValue();

                    // FetchAndSaveDataAsync �޼��忡�� �����͸� ���Ϸ� �����ϰų� ó���ϴ� ���� ����
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
            // Firebase���� AccountBase ������ ��������
            var snapshot = await db_root.Child("AccountBase").GetValueAsync();

            if (snapshot.Exists)
            {
                // AccountBase�� �ڽ� ��� ��ȸ
                foreach (DataSnapshot node in snapshot.Children)
                {
                    string nodeName = node.Key;
                    string jsonData = node.GetRawJsonValue();

                    // FetchAndSaveDataAsync�� ȣ���Ͽ� ������ ����
                    await FetchAndSaveDataAsync(nodeName, jsonData, folderPath);
                }
            }
            else
            {
                Debug.LogError("AccountBase �����Ͱ� �������� �ʽ��ϴ�.");
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
                File.Delete(file);  // ���� ����
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

        // ���� ���� ��� JSON ���� ����� �����ɴϴ�
        string[] jsonFiles = Directory.GetFiles(folderPath, "*.json");
        Debug.Log($"���� �� : {jsonFiles.Length}");

        // ��� ���� ���ε带 �Ϸ��� ������ ��ٸ�
        foreach (string filePath in jsonFiles)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string jsonContent = File.ReadAllText(filePath);

            // Firebase�� ���ε�
            bool uploadSuccess = await UploadJsonToFirebaseAsync(fileName, jsonContent);

            // ������ ���, ���� �α� ���
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
            // Firebase ���: Accounts/userId/���ϸ�
            var task = db_root.Child("Accounts").Child(Auth_Firebase.Instance.UserId).Child(fileName).SetRawJsonValueAsync(jsonContent);

            // Firebase�� �����͸� ���ε� �� �Ϸ� ���
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
