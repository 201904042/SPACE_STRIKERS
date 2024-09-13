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
        // 정규식을 사용하여 \uXXXX 형태의 유니코드 이스케이프 시퀀스를 변환합니다.
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

                // 각 노드를 순회
                foreach (DataSnapshot node in snapshot.Children)
                {
                    string nodeName = node.Key; // 현재 노드의 이름
                    string jsonData = node.GetRawJsonValue(); // 노드의 데이터를 JSON 문자열로 변환
                    string formattedJsonData = $"{{ \"{nodeName}\": {jsonData} }}";
                    string decodedJsonData = DecodeUnicodeEscapes(formattedJsonData);
                    // 파일 경로 설정 (Resources/폴더에 저장)
                    string path = Path.Combine(Application.dataPath, "Resources/Data", nodeName + ".json");

                    // 디렉토리 존재 여부 확인 및 생성
                    string directory = Path.GetDirectoryName(path);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    // JSON 파일로 저장
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

                    Debug.Log("아이템 이름: " + name);
                    Debug.Log("아이템 설명: " + description);
                }

                // 데이터를 JSON 형식으로 변환
                string jsonData = snapshot.GetRawJsonValue();
                string decodedJsonData = DecodeUnicodeEscapes(jsonData);

                Debug.Log(decodedJsonData);

                // Unity 에셋 폴더에 JSON 데이터 저장
                SaveDataToFile(decodedJsonData);
            }
            else
            {
                Debug.LogError("Firebase 데이터 가져오기 실패: " + task.Exception);
            }
        });
    }

    void SaveDataToFile(string jsonData)
    {
        string path = Path.Combine(Application.dataPath, "Resources/Data", "Example.json");

        // 경로에 디렉토리가 없으면 생성
        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }

        // JSON 데이터를 파일로 저장
        File.WriteAllText(path, jsonData, Encoding.UTF8);
        Debug.Log("JSON 데이터 저장 완료: " + path);

        // Unity 에셋 데이터베이스 리프레시
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }*/


    
    
}
