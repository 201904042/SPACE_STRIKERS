using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager dataInstance;

    public EnemyJsonReader enemyData;

    public static AccountJsonReader accountData = new AccountJsonReader();
    public static MasterDataReader masterData = new MasterDataReader();
    public static InventoryDataReader inventoryData = new InventoryDataReader();
    public static CharacterDataReader characterData = new CharacterDataReader();
    public static PartsDataReader partsData = new PartsDataReader();
    public static AbilityDataReader abilityData = new AbilityDataReader();
    public static StoreItemReader storeData = new StoreItemReader();
    public static StageDataReader stageData = new StageDataReader();

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

        accountData.LoadData();
        masterData.LoadData();
        characterData.LoadData();
        inventoryData.LoadData();
        partsData.LoadData();
        abilityData.LoadData();
        storeData.LoadData();
        stageData.LoadData();
    }

    public static T LoadJsonData<T>(string path) where T : class
    {
        TextAsset json = Resources.Load<TextAsset>(path);
        if (json == null)
        {
            Debug.LogError($"{path}: JSON 파일이 로드되지 않음");
            return null;
        }

        T dataInstance = JsonUtility.FromJson<T>(json.text);
        if (dataInstance == null)
        {
            Debug.LogError($"{path}: 파싱이 제대로 이루어지지 않음");
            return null;
        }

        Debug.Log($"{path}: 데이터가 성공적으로 로드됨");
        return dataInstance;
    }

    public static Dictionary<int, T> SetDictionary<T, TList>(string path, Func<TList, IEnumerable<T>> itemSelector, Func<T, int> keySelector) where TList : class
    {
        TList dataInstance = LoadJsonData<TList>(path);
        Dictionary<int, T> dictionary = new Dictionary<int, T>();

        foreach (T item in itemSelector(dataInstance))
        {
            dictionary.Add(keySelector(item), item);
        }

        Debug.Log($"{typeof(T).Name} : {dictionary.Count}개의 아이템이 로드됨");
        return dictionary;
    }

    
}
