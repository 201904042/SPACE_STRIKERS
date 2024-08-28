using UnityEngine;
using System.IO;
using System;

[Serializable]
public class AccountData
{
    public int accountId;
    public string accountName;
    public int accountLevel;
    public int currentExperience;
    public int stageProgress;
}
public class AccountDataList
{
    public AccountData accountData;
}

public class AccountJsonReader 
{
    public TextAsset jsonTextAsset; // Drag and drop the JSON file here in the Inspector

    public void LoadData()
    {
        TextAsset json = Resources.Load<TextAsset>("JSON/AccountData");
        if (json == null)
        {
            Debug.LogError("AccountData: JSON�� �ε���� ����");
            return;
        }

        AccountDataList dataInstance = JsonUtility.FromJson<AccountDataList>(json.text);
        if (dataInstance != null)
        {
            Debug.Log($"Account ID: {dataInstance.accountData.accountId}");
            Debug.Log($"Account Name: {dataInstance.accountData.accountName}");
            Debug.Log($"Account Level: {dataInstance.accountData.accountLevel}");
            Debug.Log($"Current Experience: {dataInstance.accountData.currentExperience}");
            Debug.Log($"Stage Progress: {dataInstance.accountData.stageProgress}");
        }
        else
        {
            Debug.LogError("Failed to parse AccountData.");
        }
    }
}
