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
    public static StoreItemReader storeData = new StoreItemReader();

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
        stageData = GetComponent<StageJsonReader>();

        accountData.LoadData();
        masterData.LoadData();
        characterData.LoadData();
        inventoryData.LoadData();
        partsData.LoadData();
        abilityData.LoadData();
        storeData.LoadData();
    }

    public static T LoadJsonData<T>(string path) where T : class
    {
        TextAsset json = Resources.Load<TextAsset>(path);
        if (json == null)
        {
            Debug.LogError($"{path}: JSON ������ �ε���� ����");
            return null;
        }

        T dataInstance = JsonUtility.FromJson<T>(json.text);
        if (dataInstance == null)
        {
            Debug.LogError($"{path}: �Ľ��� ����� �̷������ ����");
            return null;
        }

        Debug.Log($"{path}: �����Ͱ� ���������� �ε��");
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

        Debug.Log($"{typeof(T).Name} : {dictionary.Count}���� �������� �ε��");
        return dictionary;
    }

    
}
