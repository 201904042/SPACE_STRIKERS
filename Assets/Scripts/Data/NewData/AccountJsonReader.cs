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
    public AccountData account;

    public void LoadData()
    {
        TextAsset json = Resources.Load<TextAsset>("JSON/AccountData");
        if (json == null)
        {
            Debug.LogError("AccountData: JSON이 로드되지 않음");
            return;
        }

        AccountDataList dataInstance = JsonUtility.FromJson<AccountDataList>(json.text);
        account = dataInstance.accountData;
        if (dataInstance != null)
        {
            Debug.Log($"Account ID: {account.accountId}");
            Debug.Log($"Account Name: {account.accountName}");
            Debug.Log($"Account Level: {account.accountLevel}");
            Debug.Log($"Current Experience: {account.currentExperience}");
            Debug.Log($"Stage Progress: {account.stageProgress}");
        }
        else
        {
            Debug.LogError("Failed to parse AccountData.");
        }
    }
}
