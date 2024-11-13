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
    private static DatabaseReference FB_root;
    private static DatabaseReference FB_WritablePath;
    private static DatabaseReference FB_ReadOnlyPath;
    private static DatabaseReference FB_WritableBasePath;
    private const string url = "https://spacestrikers-f12ac-default-rtdb.firebaseio.com/";

    public async Task Init()
    {
        FB_root = FirebaseDatabase.DefaultInstance.RootReference;
        FB_WritablePath = FB_root.Child("Accounts");
        await CheckIfNodeExistsAsync(FB_WritablePath);
        FB_ReadOnlyPath = FB_root.Child("ReadOnly");
        await CheckIfNodeExistsAsync(FB_ReadOnlyPath);
        FB_WritableBasePath = FB_root.Child("AccountBase");
        await CheckIfNodeExistsAsync(FB_WritableBasePath);
    }

    public async Task<bool> CheckIfNodeExistsAsync(DatabaseReference reference)
    {
        DataSnapshot snapshot = await reference.GetValueAsync();

        if (snapshot.Exists)
        {
            Debug.Log($"{reference} 경로가 존재");
            return true;
        }
        else
        {
            Debug.LogWarning($"{reference} 경로는 존재하지 않음");
            return false;
        }

    }

    /// <summary>
    /// 유니코드 이스케이프 시퀀스를 일반 문자열로 변환
    /// </summary>
    private string DecodeUnicodeEscapes(string unicodeEscaped)
    {
        return Regex.Replace(unicodeEscaped, @"\\u([0-9A-Fa-f]{4})", match =>
        {
            char c = (char)Convert.ToInt32(match.Groups[1].Value, 16);
            return c.ToString();
        });
    }

    //파이어베이스에서 해당 이름을 가진 노드의 필드를 불러와 json으로 저장 / 파이어베이스 -> 클라
    private async Task FetchAndSaveDataAsync(string nodeName, string jsonData, string folderPath)
    {
        if(jsonData == null)
        {
            return;
        }
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

    // 파이어베이스 -> 클라
    public async Task GetGameDataAsync()
    {
        string folderPath = Path.Combine(Application.streamingAssetsPath, "JSON/ReadOnly");

        try
        {
            // Firebase에서 데이터를 비동기적으로 가져옴
            DataSnapshot snapshot = await FB_ReadOnlyPath.GetValueAsync();

            // 데이터가 존재하는지 확인
            if (snapshot.Exists)
            {
                foreach (DataSnapshot FildName in snapshot.Children)
                {
                    foreach (DataSnapshot node in FildName.Children)
                    {
                        string nodeName = node.Key;
                        string jsonData = node.GetRawJsonValue();

                        // FetchAndSaveDataAsync 메서드에서 데이터를 파일로 저장하거나 처리하는 로직 수행
                        await FetchAndSaveDataAsync(nodeName, jsonData, folderPath);
                    }
                }
                    
            }
            else
            {
                Debug.LogError("No data found in ReadOnly JsonNode.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to fetch data: {ex.Message}");
        }
    }

    // 파이어베이스 -> 클라
    public async Task GetAccountDataAsync(string accountCode)
    {
        string folderPath = Path.Combine(Application.streamingAssetsPath, "JSON/Writable");

        try
        {
            DataSnapshot snapshot = await FB_WritablePath
                                                  .OrderByKey()
                                                  .EqualTo(accountCode)
                                                  .GetValueAsync();

            if (snapshot.Exists)
            {
                foreach (DataSnapshot AccountIdNode in snapshot.Children)
                {
                    foreach (DataSnapshot FileNameNode in AccountIdNode.Children)
                    {
                        foreach (DataSnapshot JsonNode in FileNameNode.Children)
                        {
                            string nodeName = JsonNode.Key;
                            string jsonData = JsonNode.GetRawJsonValue();

                            await FetchAndSaveDataAsync(nodeName, jsonData, folderPath);
                        }
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
            var snapshot = await FB_WritableBasePath.GetValueAsync();

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

    //클라의 writable안의 json파일들을 모두 보냄
    public async Task UploadAllWritableJsonFilesAsync()
    {
        string folderPath = Path.Combine(Application.streamingAssetsPath, "JSON/Writable");

        // 폴더 안의 모든 JSON 파일 목록을 가져옵니다
        string[] jsonFiles = Directory.GetFiles(folderPath, "*.json");
        Debug.Log($"파일 수 : {jsonFiles.Length}");

        // 모든 파일 업로드를 완료할 때까지 기다림
        foreach (string filePath in jsonFiles)
        {
            await UploadWritableJsonFilesAsync(filePath);
        }
    }

    //클라의 writable안의 json파일들을 모두 보냄
    public async Task UploadWritableJsonFilesAsync(string filePath)
    {
        string fileName = Path.GetFileNameWithoutExtension(filePath);
        string jsonContent = File.ReadAllText(filePath);

        // Firebase에 업로드
        bool uploadSuccess = await UploadJsonToFirebaseAsync(fileName, jsonContent, FB_WritablePath.Child(Managers.Instance.FB_Auth.UserId));
        // 실패한 경우, 에러 로그 출력
        if (!uploadSuccess)
        {
            Debug.LogError($"Failed to upload {fileName} for user {Managers.Instance.FB_Auth.UserId}");
        }
    }

    public async Task UploadAllReadOnlyJsonFilesAsync()
    {
        string folderPath = Path.Combine(Application.streamingAssetsPath, "JSON/ReadOnly");

        // 폴더 안의 모든 JSON 파일 목록을 가져옵니다
        string[] jsonFiles = Directory.GetFiles(folderPath, "*.json");
        Debug.Log($"파일 수 : {jsonFiles.Length}");

        // 모든 파일 업로드를 완료할 때까지 기다림
        foreach (string filePath in jsonFiles)
        {
            await UploadReadOnlyJsonFilesAsync(filePath);
        }
    }

    //클라의 writable안의 json파일들을 모두 보냄
    public async Task UploadReadOnlyJsonFilesAsync(string filePath)
    {
        string fileName = Path.GetFileNameWithoutExtension(filePath);
        string jsonContent = File.ReadAllText(filePath);

        // Firebase에 업로드
        bool uploadSuccess = await UploadJsonToFirebaseAsync(fileName, jsonContent, FB_ReadOnlyPath);
        // 실패한 경우, 에러 로그 출력
        if (!uploadSuccess)
        {
            Debug.LogError($"Failed to upload {fileName} for user {FB_ReadOnlyPath}");
        }
    }



    //FB_Path의 자식 fileName노드의 컨텐츠를 jsonContent로 전체 수정
    private async Task<bool> UploadJsonToFirebaseAsync(string fileName, string jsonContent, DatabaseReference FB_Path)
    {
        DatabaseReference reference = FB_Path.Child(fileName);
        
        try
        {
            var task = reference.SetRawJsonValueAsync(jsonContent);
            await task;

            Debug.Log($"Successfully uploaded {fileName} for user {reference}.");
            return true;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error uploading {fileName} for user {reference} : {ex.Message}");
            return false;
        }
    }


    //미사용. 활용성을 생각하면 필드단위로 업데이트를 하는게 좋지만 미완이기에 json전체를 업데이트하는 위의 메서드를 사용
    private async Task<bool> UpdateJsonFieldsInFirebaseAsync(string fileName, Dictionary<string, object> fieldsToUpdate, DatabaseReference FB_Path)
    {
        try
        {
            // Firebase 경로: FB_Path/fileName 노드
            DatabaseReference targetPath = FB_Path.Child(fileName);

            // 해당 경로가 존재하는지 확인
            bool nodeExists = await CheckIfNodeExistsAsync(targetPath);
            if (!nodeExists)
            {
                Debug.LogError($"Path does not exist: {fileName}");
                return false;
            }

            // UpdateChildrenAsync로 특정 필드만 업데이트
            await targetPath.UpdateChildrenAsync(fieldsToUpdate);

            Debug.Log($"Successfully updated fields in {fileName} for user {Managers.Instance.FB_Auth.UserId}.");
            return true;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error updating fields in {fileName} for user {Managers.Instance.FB_Auth.UserId}: {ex.Message}");
            return false;
        }
    }



    #region 클라 파일 컨트롤
    //파일에서 json 삭제
    public static void DeleteAccountData()
    {
        string accountFolder = Path.Combine(Application.streamingAssetsPath, "JSON/Writable");
        DeleteJsonByPath(accountFolder);

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    private static void DeleteJsonByPath(string folderPath)
    {
        if (!CheckFolderPath(folderPath))
        {
            return;
        }

        string[] jsonFiles = Directory.GetFiles(folderPath, "*.json");

        foreach (string file in jsonFiles)
        {
            File.Delete(file);  // 파일 삭제
        }
    }

    private static bool CheckFolderPath(string folderPath)
    {
        if (Directory.Exists(folderPath))
        {
            Debug.Log($"{folderPath} 확인");
            return true;
        }
        else
        {
            Debug.LogWarning($"{folderPath} 는 존재하지 않음");
            return false;
        }
    }
    #endregion
}
