
using System.Collections.Generic;
using System.IO;

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
    PartsData,
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
    { DataFieldType.AccountData, "Accountdata" },
    { DataFieldType.CharData, "CharacterData" },
    { DataFieldType.PartsData, "PartsData" },
    { DataFieldType.InvenData, "InvenData" },
    { DataFieldType.EnemyData, "EnemyData" },
    { DataFieldType.GotchaData, "GotchaData" }
};

    public static AccountDataReader account = new();
    public static MasterDataReader master = new();
    public static InventoryDataReader inven = new();
    public static EnemyDataReader enemy = new ();
    public static SkillDataReader skill = new ();
    public static CharacterDataReader character = new ();
    public static PartsDataReader parts = new ();
    public static AbilityDataReader ability = new ();
    public static StoreDataReader store = new ();
    public static StageDataReader stage = new ();
    public static UpgradeDataReader upgrade = new ();
    public static GotchaDataReader gotcha = new ();

    public void Init()
    {
        LoadAllData();
    }

    public static class JsonFilePaths
    {
        public static readonly string ReadOnlyFolder = "Assets/StreamingAssets/JSON/ReadOnly/";
        public static readonly string WritableFolder = "Assets/StreamingAssets/JSON/Writable/";
    }

    public void LoadAllData()
    {
        master.LoadData(Path.Combine(JsonFilePaths.ReadOnlyFolder, "MasterData.json"));
        ability.LoadData(Path.Combine(JsonFilePaths.ReadOnlyFolder, "AbilityData.json"));
        stage.LoadData(Path.Combine(JsonFilePaths.ReadOnlyFolder, "StageData.json"));
        upgrade.LoadData(Path.Combine(JsonFilePaths.ReadOnlyFolder, "UpgradeData.json"));
        enemy.LoadData(Path.Combine(JsonFilePaths.ReadOnlyFolder, "EnemyData.json"));
        gotcha.LoadData(Path.Combine(JsonFilePaths.ReadOnlyFolder, "GotchaData.json"));
        skill.LoadData(Path.Combine(JsonFilePaths.ReadOnlyFolder, "SkillData.json"));
        store.LoadData(Path.Combine(JsonFilePaths.ReadOnlyFolder, "StoreData.json"));

        account.LoadData(Path.Combine(JsonFilePaths.WritableFolder, "AccountData.json"));
        character.LoadData(Path.Combine(JsonFilePaths.WritableFolder, "CharacterData.json"));
        inven.LoadData(Path.Combine(JsonFilePaths.WritableFolder, "InvenData.json"));
        parts.LoadData(Path.Combine(JsonFilePaths.WritableFolder, "PartsData.json"));
    }



}
