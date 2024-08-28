using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager dataInstance;

    public EnemyJsonReader enemyData;
    public StageJsonReader stageData;

    public static AccountJsonReader accountData = new AccountJsonReader();
    public static MasterDataReader masterData = new MasterDataReader();
    public static InventoryDataReader inventoryData = new InventoryDataReader();
    public static CharacterDataReader characterData = new CharacterDataReader();
    public static PartsDataReader partsData = new PartsDataReader();
    public static AbilityDataReader abilityData = new AbilityDataReader();

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
        enemyData = GetComponent<EnemyJsonReader>();
        stageData = GetComponent<StageJsonReader>();

        accountData.LoadData();
        masterData.LoadData();
        inventoryData.LoadData();
        characterData.LoadData();
        partsData.LoadData();
        abilityData.LoadData();
    }
    
}
