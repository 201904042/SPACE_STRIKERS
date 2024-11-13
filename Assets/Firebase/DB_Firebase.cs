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
            Debug.Log($"{reference} ��ΰ� ����");
            return true;
        }
        else
        {
            Debug.LogWarning($"{reference} ��δ� �������� ����");
            return false;
        }

    }

    /// <summary>
    /// �����ڵ� �̽������� �������� �Ϲ� ���ڿ��� ��ȯ
    /// </summary>
    private string DecodeUnicodeEscapes(string unicodeEscaped)
    {
        return Regex.Replace(unicodeEscaped, @"\\u([0-9A-Fa-f]{4})", match =>
        {
            char c = (char)Convert.ToInt32(match.Groups[1].Value, 16);
            return c.ToString();
        });
    }

    //���̾�̽����� �ش� �̸��� ���� ����� �ʵ带 �ҷ��� json���� ���� / ���̾�̽� -> Ŭ��
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

    // ���̾�̽� -> Ŭ��
    public async Task GetGameDataAsync()
    {
        string folderPath = Path.Combine(Application.streamingAssetsPath, "JSON/ReadOnly");

        try
        {
            // Firebase���� �����͸� �񵿱������� ������
            DataSnapshot snapshot = await FB_ReadOnlyPath.GetValueAsync();

            // �����Ͱ� �����ϴ��� Ȯ��
            if (snapshot.Exists)
            {
                foreach (DataSnapshot FildName in snapshot.Children)
                {
                    foreach (DataSnapshot node in FildName.Children)
                    {
                        string nodeName = node.Key;
                        string jsonData = node.GetRawJsonValue();

                        // FetchAndSaveDataAsync �޼��忡�� �����͸� ���Ϸ� �����ϰų� ó���ϴ� ���� ����
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

    // ���̾�̽� -> Ŭ��
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
            // Firebase���� AccountBase ������ ��������
            var snapshot = await FB_WritableBasePath.GetValueAsync();

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

    //Ŭ���� writable���� json���ϵ��� ��� ����
    public async Task UploadAllWritableJsonFilesAsync()
    {
        string folderPath = Path.Combine(Application.streamingAssetsPath, "JSON/Writable");

        // ���� ���� ��� JSON ���� ����� �����ɴϴ�
        string[] jsonFiles = Directory.GetFiles(folderPath, "*.json");
        Debug.Log($"���� �� : {jsonFiles.Length}");

        // ��� ���� ���ε带 �Ϸ��� ������ ��ٸ�
        foreach (string filePath in jsonFiles)
        {
            await UploadWritableJsonFilesAsync(filePath);
        }
    }

    //Ŭ���� writable���� json���ϵ��� ��� ����
    public async Task UploadWritableJsonFilesAsync(string filePath)
    {
        string fileName = Path.GetFileNameWithoutExtension(filePath);
        string jsonContent = File.ReadAllText(filePath);

        // Firebase�� ���ε�
        bool uploadSuccess = await UploadJsonToFirebaseAsync(fileName, jsonContent, FB_WritablePath.Child(Managers.Instance.FB_Auth.UserId));
        // ������ ���, ���� �α� ���
        if (!uploadSuccess)
        {
            Debug.LogError($"Failed to upload {fileName} for user {Managers.Instance.FB_Auth.UserId}");
        }
    }

    public async Task UploadAllReadOnlyJsonFilesAsync()
    {
        string folderPath = Path.Combine(Application.streamingAssetsPath, "JSON/ReadOnly");

        // ���� ���� ��� JSON ���� ����� �����ɴϴ�
        string[] jsonFiles = Directory.GetFiles(folderPath, "*.json");
        Debug.Log($"���� �� : {jsonFiles.Length}");

        // ��� ���� ���ε带 �Ϸ��� ������ ��ٸ�
        foreach (string filePath in jsonFiles)
        {
            await UploadReadOnlyJsonFilesAsync(filePath);
        }
    }

    //Ŭ���� writable���� json���ϵ��� ��� ����
    public async Task UploadReadOnlyJsonFilesAsync(string filePath)
    {
        string fileName = Path.GetFileNameWithoutExtension(filePath);
        string jsonContent = File.ReadAllText(filePath);

        // Firebase�� ���ε�
        bool uploadSuccess = await UploadJsonToFirebaseAsync(fileName, jsonContent, FB_ReadOnlyPath);
        // ������ ���, ���� �α� ���
        if (!uploadSuccess)
        {
            Debug.LogError($"Failed to upload {fileName} for user {FB_ReadOnlyPath}");
        }
    }



    //FB_Path�� �ڽ� fileName����� �������� jsonContent�� ��ü ����
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


    //�̻��. Ȱ�뼺�� �����ϸ� �ʵ������ ������Ʈ�� �ϴ°� ������ �̿��̱⿡ json��ü�� ������Ʈ�ϴ� ���� �޼��带 ���
    private async Task<bool> UpdateJsonFieldsInFirebaseAsync(string fileName, Dictionary<string, object> fieldsToUpdate, DatabaseReference FB_Path)
    {
        try
        {
            // Firebase ���: FB_Path/fileName ���
            DatabaseReference targetPath = FB_Path.Child(fileName);

            // �ش� ��ΰ� �����ϴ��� Ȯ��
            bool nodeExists = await CheckIfNodeExistsAsync(targetPath);
            if (!nodeExists)
            {
                Debug.LogError($"Path does not exist: {fileName}");
                return false;
            }

            // UpdateChildrenAsync�� Ư�� �ʵ常 ������Ʈ
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



    #region Ŭ�� ���� ��Ʈ��
    //���Ͽ��� json ����
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
            File.Delete(file);  // ���� ����
        }
    }

    private static bool CheckFolderPath(string folderPath)
    {
        if (Directory.Exists(folderPath))
        {
            Debug.Log($"{folderPath} Ȯ��");
            return true;
        }
        else
        {
            Debug.LogWarning($"{folderPath} �� �������� ����");
            return false;
        }
    }
    #endregion
}
