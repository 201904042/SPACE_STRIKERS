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
    SkillData,
    AccountData,
    CharData,
    PartsAbilityData,
    InvenData,
    EnemyData,
    GotchaData
}


public class DataManager
{
    public static Dictionary<DataFieldType, string> dataFieldNames = new Dictionary<DataFieldType, string>
{
    { DataFieldType.AbilityData, "AbilityData" },
    { DataFieldType.MasterData, "MasterData" },
    { DataFieldType.StoreData, "StoreData" },
    { DataFieldType.StageData, "StageData" },
    { DataFieldType.UpgradeData, "UpgradeData" },
    { DataFieldType.SkillData, "SkillData" },
    { DataFieldType.AccountData, "AccountData" },
    { DataFieldType.CharData, "CharacterData" },
    { DataFieldType.PartsAbilityData, "PartsAbilityData" },
    { DataFieldType.InvenData, "InvenData" },
    { DataFieldType.EnemyData, "EnemyData" },
    { DataFieldType.GotchaData, "GotchaData" }
};

    public static AccountJsonReader account = new();
    public static MasterDataReader master = new();
    public static InventoryDataReader inven = new();
    public static EnemyDataReader enemy = new ();
    public static SkillDataReader skill = new ();
    public static CharacterDataReader character = new ();
    public static PartsAbilityDataReader parts = new ();
    public static AbilityDataReader ability = new ();
    public static StoreItemReader store = new ();
    public static StageDataReader stage = new ();
    public static UpgradeDataReader upgrade = new ();
    public static GotchaDataReader gotcha = new ();

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
        skill.LoadData("Assets/StreamingAssets/JSON/ReadOnly/SkillData.json");

        store.LoadData("Assets/StreamingAssets/JSON/Writable/StoreData.json");
        account.LoadData("Assets/StreamingAssets/JSON/Writable/AccountData.json");
        character.LoadData("Assets/StreamingAssets/JSON/Writable/CharacterData.json");
        inven.LoadData("Assets/StreamingAssets/JSON/Writable/InvenData.json");
        parts.LoadData("Assets/StreamingAssets/JSON/Writable/PartsAbilityData.json");

    }


}
