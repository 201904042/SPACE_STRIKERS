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
    /// �����ڵ� �̽������� �������� �Ϲ� ���ڿ��� ��ȯ
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
    /// Firebase���� �����͸� �����ͼ� JSON ���Ϸ� ����
    /// </summary>
    public static void FetchAndSaveData(string nodeName, string jsonData, string folderPath)
    {
        string formattedJsonData = $"{{ \"{nodeName}\": {jsonData} }}";
        string decodedJsonData = DecodeUnicodeEscapes(formattedJsonData);

        // ��� ���� �� ���丮 Ȯ��
        string path = Path.Combine(folderPath, nodeName + ".json");
        string directory = Path.GetDirectoryName(path);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        // ���� ����
        File.WriteAllText(path, decodedJsonData);
        Debug.Log($"Saved {nodeName} data to {path}");

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    /// <summary>
    /// ���� ������(ReadOnly)�� Firebase���� �о�� JSON ���Ϸ� ����
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
     * ���� ��ȹ
     * root ������ READONLY��忡 ���ӿ�� �ʿ��� �����͵��� ACCOUNTS��忡 �� �÷��̾���� ������
     * �÷��̾� ����� �������� �� �÷��̾ �ڵ带 �̸������ϴ� ������ �̷���� �ְ� �ش� ��� �Ʒ����� �������� writable �����͵��� �ִ�.
     */



    /// <summary>
    /// Ư�� accountCode�� �ش��ϴ� ���� �����͸� Firebase���� �о�� JSON ���Ϸ� ����
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
    /// ���Ͽ��� JSON �����͸� �о�ɴϴ�.
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
    /// Ư�� ����� �����͸� ���� ����� JSON ���Ϸ� ������Ʈ
    /// </summary>
    public static void UpdateFirebaseNodeFromJson(string accountCode,string nodeName, string JsonPath)
    {
        DatabaseReference db_root = FirebaseDatabase.DefaultInstance.RootReference;
        string jsonData = LoadJsonFromFile(JsonPath);
        if (jsonData != null)
        {
            // Firebase�� Ư�� ��带 ������Ʈ
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
