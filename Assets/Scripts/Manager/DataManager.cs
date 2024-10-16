using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public enum DataFieldType
{
    AbilityData,
    MasterData,
    StoreData,
    StageData,
    UpgradeData,
    AccountData,
    CharData,
    PartsAbilityData,
    InvenData,
    EnemyData,
    GotchaData
}


public class DataManager : MonoBehaviour
{
    public static Dictionary<DataFieldType, string> dataFieldNames = new Dictionary<DataFieldType, string>
{
    { DataFieldType.AbilityData, "AbilityData" },
    { DataFieldType.MasterData, "MasterData" },
    { DataFieldType.StoreData, "StoreData" },
    { DataFieldType.StageData, "StageData" },
    { DataFieldType.UpgradeData, "UpgradeData" },
    { DataFieldType.AccountData, "AccountData" },
    { DataFieldType.CharData, "CharacterData" },
    { DataFieldType.PartsAbilityData, "PartsAbilityData" },
    { DataFieldType.InvenData, "InvenData" },
    { DataFieldType.EnemyData, "EnemyData" },
    { DataFieldType.GotchaData, "GotchaData" }
};

    public static AccountJsonReader account = new AccountJsonReader();
    public static MasterDataReader master = new MasterDataReader();
    public static InventoryDataReader inven = new InventoryDataReader();
    public static EnemyDataReader enemy = new EnemyDataReader();
    public static CharacterDataReader character = new CharacterDataReader();
    public static PartsAbilityDataReader parts = new PartsAbilityDataReader();
    public static AbilityDataReader ability = new AbilityDataReader();
    public static StoreItemReader store = new StoreItemReader();
    public static StageDataReader stage = new StageDataReader();
    public static UpgradeDataReader upgrade = new UpgradeDataReader();
    public static GotchaDataReader gotcha = new GotchaDataReader();
    public void Init()
    {
        LoadAllData();
    }

    private void LoadAllData()
    {
        master.LoadData($"Assets/StreamingAssets/JSON/ReadOnly/MasterData.json");
        ability.LoadData("Assets/StreamingAssets/JSON/ReadOnly/AbilityData.json");
        stage.LoadData("Assets/StreamingAssets/JSON/ReadOnly/StageData.json");
        upgrade.LoadData("Assets/StreamingAssets/JSON/ReadOnly/UpgradeData.json");
        enemy.LoadData("Assets/StreamingAssets/JSON/ReadOnly/EnemyData.json");
        gotcha.LoadData("Assets/StreamingAssets/JSON/ReadOnly/GotchaData.json");

        store.LoadData("Assets/StreamingAssets/JSON/Writable/StoreData.json");
        account.LoadData("Assets/StreamingAssets/JSON/Writable/AccountData.json");
        character.LoadData("Assets/StreamingAssets/JSON/Writable/CharacterData.json");
        inven.LoadData("Assets/StreamingAssets/JSON/Writable/InvenData.json");
        parts.LoadData("Assets/StreamingAssets/JSON/Writable/PartsAbilityData.json");
    }
}
