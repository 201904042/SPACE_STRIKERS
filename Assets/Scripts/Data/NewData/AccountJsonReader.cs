using UnityEngine;
using System.IO;
using System;


public class AccountJsonReader
{
    public AccountData account;

    public void LoadData()
    {
        AccountDatas dataInstance = DataManager.LoadJsonData<AccountDatas>("JSON/Writable/AccountData");
        account = dataInstance.AccountData;
        if (dataInstance != null)
        {
            Debug.Log($"Account ID: {account.id}");
            Debug.Log($"Account Name: {account.name}");
            Debug.Log($"Account Level: {account.level}");
            Debug.Log($"Current Experience: {account.exp}");
            Debug.Log($"Stage Progress: {account.stageProgress}");
        }
        else
        {
            Debug.LogError("Failed to parse AccountData.");
        }
    }
}
