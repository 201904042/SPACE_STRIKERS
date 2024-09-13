using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class DB_Firebase : MonoBehaviour
{
    public static string DecodeUnicodeEscapes(string unicodeEscaped)
    {
        // ���Խ��� ����Ͽ� \uXXXX ������ �����ڵ� �̽������� �������� ��ȯ�մϴ�.
        return Regex.Replace(unicodeEscaped, @"\\u([0-9A-Fa-f]{4})", match =>
        {
            char c = (char)Convert.ToInt32(match.Groups[1].Value, 16);
            return c.ToString();
        });
    }

    public string url = "https://spacestrikers-f12ac-default-rtdb.firebaseio.com/";
    DatabaseReference databaseReference;
    private void Start()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        Debug.Log(databaseReference);
        //GetFirebaseData();
        FetchData();
    }
    void FetchData()
    {
        databaseReference.Child("ReadOnly").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                // �� ��带 ��ȸ
                foreach (DataSnapshot node in snapshot.Children)
                {
                    string nodeName = node.Key; // ���� ����� �̸�
                    string jsonData = node.GetRawJsonValue(); // ����� �����͸� JSON ���ڿ��� ��ȯ
                    string formattedJsonData = $"{{ \"{nodeName}\": {jsonData} }}";
                    string decodedJsonData = DecodeUnicodeEscapes(formattedJsonData);
                    // ���� ��� ���� (Resources/������ ����)
                    string path = Path.Combine(Application.dataPath, "Resources/Data", nodeName + ".json");

                    // ���丮 ���� ���� Ȯ�� �� ����
                    string directory = Path.GetDirectoryName(path);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    // JSON ���Ϸ� ����
                    File.WriteAllText(path, decodedJsonData);
                    Debug.Log($"Saved {nodeName} data to {path}");
#if UNITY_EDITOR
                    UnityEditor.AssetDatabase.Refresh();
#endif
                }
            }
            else
            {
                Debug.LogError("Failed to fetch data: " + task.Exception);
            }
        });
    }
    /*
    void GetFirebaseData()
    {
        DatabaseReference masterItemsRef = databaseReference.Child("ReadOnly").Child("masterItems");


        masterItemsRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot item in snapshot.Children)
                {
                    string name = item.Child("name").Value.ToString();
                    string description = item.Child("description").Value.ToString();

                    Debug.Log("������ �̸�: " + name);
                    Debug.Log("������ ����: " + description);
                }

                // �����͸� JSON �������� ��ȯ
                string jsonData = snapshot.GetRawJsonValue();
                string decodedJsonData = DecodeUnicodeEscapes(jsonData);

                Debug.Log(decodedJsonData);

                // Unity ���� ������ JSON ������ ����
                SaveDataToFile(decodedJsonData);
            }
            else
            {
                Debug.LogError("Firebase ������ �������� ����: " + task.Exception);
            }
        });
    }

    void SaveDataToFile(string jsonData)
    {
        string path = Path.Combine(Application.dataPath, "Resources/Data", "Example.json");

        // ��ο� ���丮�� ������ ����
        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }

        // JSON �����͸� ���Ϸ� ����
        File.WriteAllText(path, jsonData, Encoding.UTF8);
        Debug.Log("JSON ������ ���� �Ϸ�: " + path);

        // Unity ���� �����ͺ��̽� ��������
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }*/


    
    
}
