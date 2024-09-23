using UnityEngine;
using System.IO;
using System;


public class AccountJsonReader
{
    public AccountData account;

    public void LoadData()
    {
        AccountDatas dataInstance = DataManager.LoadJsonData<AccountDatas>("JSON/Writable/AccountData");
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
