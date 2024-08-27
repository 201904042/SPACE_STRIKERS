using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager dataInstance;

    public AccountJsonReader accountData;
    public MasterDataReader masterData;
    public ItemJsonReader itemData;
    public PlayerJsonReader playerData;
    public EnemyJsonReader enemyData;
    public StageJsonReader stageData;

    private void Awake()
    {
        if (dataInstance == null)
        {
            dataInstance = this;
            DontDestroyOnLoad(dataInstance);
        }
        else
        {
            Destroy(gameObject);
        }

        SetComponent();
    }

    private void SetComponent()
    {
        accountData = GetComponent<AccountJsonReader>();
        masterData = GetComponent<MasterDataReader>();
        itemData = GetComponent<ItemJsonReader>();
        playerData = GetComponent<PlayerJsonReader>();
        enemyData = GetComponent<EnemyJsonReader>();
        stageData = GetComponent<StageJsonReader>();
        
    }
    
}
