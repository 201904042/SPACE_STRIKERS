using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager dataInstance;

    public EnemyJsonReader enemyData;

    public static AccountJsonReader account = new AccountJsonReader();
    public static MasterDataReader master = new MasterDataReader();
    public static InventoryDataReader inven = new InventoryDataReader();
    public static CharacterDataReader character = new CharacterDataReader();
    public static PartsDataReader parts = new PartsDataReader();
    public static AbilityDataReader ability = new AbilityDataReader();
    public static StoreItemReader store = new StoreItemReader();
    public static StageDataReader stage = new StageDataReader();
    public static UpgradeDataReader upgrade = new UpgradeDataReader();

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

        LoadAllData();
    }

    private void LoadAllData()
    {
        enemyData = GetComponent<EnemyJsonReader>();

        master.LoadData("Assets/StreamingAssets/JSON/ReadOnly/MasterData.json");
        ability.LoadData("Assets/StreamingAssets/JSON/ReadOnly/AbilityData.json");
        stage.LoadData("Assets/StreamingAssets/JSON/ReadOnly/StageData.json");
        upgrade.LoadData("Assets/StreamingAssets/JSON/ReadOnly/UpgradeData.json");

        store.LoadData("Assets/StreamingAssets/JSON/Writable/StoreData.json");
        account.LoadData("Assets/StreamingAssets/JSON/Writable/AccountData.json");
        character.LoadData("Assets/StreamingAssets/JSON/Writable/CharacterData.json");
        inven.LoadData("Assets/StreamingAssets/JSON/Writable/InvenData.json");
        parts.LoadData("Assets/StreamingAssets/JSON/Writable/PartsData.json");

    }
}
