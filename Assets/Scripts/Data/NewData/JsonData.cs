using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.U2D.Animation;
using UnityEngine;

public abstract class ReadOnlyData<T>
{
    protected Dictionary<int, T> dataDict = new Dictionary<int, T>();
    protected List<int> keysList = new List<int>();
    protected string filePath;
    public DataFieldType fieldType;
    public void LoadData(string _filePath)
    {
        filePath = _filePath;
        string json = File.ReadAllText(filePath);
        List<T> dataList = null;
        dataList = SetJsonList(json, dataList);

        foreach (T data in dataList)
        {
            int id = GetId(data);
            dataDict[id] = data;
            keysList.Add(id);
        }
        
        Debug.Log($"{typeof(T).Name}�� ������ {dataDict.Count}���� ����");
    }

    private static List<T> SetJsonList(string json, List<T> dataList)
    {
        if (typeof(T) == typeof(MasterData))
        {
            var wrapper = JsonUtility.FromJson<MasterDataWrapper>(json);
            dataList = wrapper.MasterData as List<T>;
            
        }
        else if (typeof(T) == typeof(AbilityData))
        {
            var wrapper = JsonUtility.FromJson<AbilityDataWrapper>(json);
            dataList = wrapper.AbilityData as List<T>;
        }
        else if (typeof(T) == typeof(StoreItemData))
        {
            var wrapper = JsonUtility.FromJson<StoreDataWrapper>(json);
            dataList = wrapper.StoreData as List<T>;
        }
        else if (typeof(T) == typeof(StageData))
        {
            var wrapper = JsonUtility.FromJson<StageDataWrapper>(json);
            dataList = wrapper.StageData as List<T>;
        }
        else if (typeof(T) == typeof(UpgradeData))
        {
            var wrapper = JsonUtility.FromJson<UpgradeDataWrapper>(json);
            dataList = wrapper.UpgradeData as List<T>;
        }
        else if (typeof(T) == typeof(AccountData))
        {
            var wrapper = JsonUtility.FromJson<AccountDataWrapper>(json);
            dataList = wrapper.AccountData as List<T>;
        }
        else if (typeof(T) == typeof(CharData))
        {
            var wrapper = JsonUtility.FromJson<CharacterDataWrapper>(json);
            dataList = wrapper.CharacterData as List<T>;
        }
        else if (typeof(T) == typeof(PartsData))
        {
            var wrapper = JsonUtility.FromJson<PartsDataWrapper>(json);
            dataList = wrapper.PartsData as List<T>;
        }
        else if (typeof(T) == typeof(InvenData))
        {
            var wrapper = JsonUtility.FromJson<InvenDataWrapper>(json);
            dataList = wrapper.InvenData as List<T>;
        }

        else if (typeof(T) == typeof(EnemyData))
        {
            var wrapper = JsonUtility.FromJson<EnemyDataWrapper>(json);
            dataList = wrapper.EnemyData as List<T>;
        }


        // �ʿ��� ��ŭ else if ����� �߰�
        return dataList;
    }

    public string GetFilePath()
    {
        return filePath;
    }

    protected abstract int GetId(T data);

    // �˻�
    public T GetData(int id)
    {
        return dataDict.ContainsKey(id) ? dataDict[id] : default;
    }

    public Dictionary<int, T> GetDictionary()
    {
        return dataDict;
    }

    public bool IsDataExist(int id)
    {
        return dataDict.ContainsKey(id) ? true : false;
    }

}

// �б� �� ���� ���� ������ ���� Ŭ����
public abstract class EditableData<T> : ReadOnlyData<T>
{
    public int GetLastKey()
    {
        keysList = keysList.OrderBy(x => x).ToList(); // Ű ����Ʈ ����
        return keysList[keysList.Count - 1];
    }
    // �߰�
    public bool AddData(T data)
    {
        int id = GetId(data); // �����͸� �߰��ϱ� ���� ID�� �����ɴϴ�.
        if (!dataDict.ContainsKey(id)) //�ش� ���̵� ����
        {
            dataDict.Add(id,data);
            keysList.Add(id); // Ű ����Ʈ�� �߰�
            keysList = keysList.OrderBy(x => x).ToList(); // Ű ����Ʈ ����
            return true;
        }
        
        return false; // �ߺ� ID�� ��� false ��ȯ
    }

    // ����
    public bool DeleteData(int id)
    {
        if (!dataDict.ContainsKey(id))
        {
            Debug.Log($"ID {id}�� �������� ����");
            return false;
        }

        dataDict.Remove(id);
        keysList.Remove(id); // Ű ����Ʈ���� ����
        return true;
    }

    // ������Ʈ
    public bool UpdateData(int id, T data)
    {
        if (!dataDict.ContainsKey(id))
        {
            Debug.Log($"ID {id}�� �������� ����");
            return false;
        }

        dataDict[id] = data;
        return true;
    }

    // JSON���� �����ϴ� �Լ� (�ʿ� �� ����)

    public void SaveData()
    {
        Wrapper<T> wrapper = new Wrapper<T>(new List<T>(dataDict.Values));
        string json = JsonUtility.ToJson(wrapper, true);
       
        // JSON �ʵ�� ����
        json = json.Replace("\"dataList\"", $"\"{DataManager.dataFieldNames[fieldType]}\"");

        File.WriteAllText(filePath, json);
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

}

// ���׸� ����Ʈ�� JSON���� ���δ� Wrapper Ŭ����
[System.Serializable]
public class Wrapper<T>
{
    // json�� ���� �ʵ���� �����ϱ� ���� �߰�
    [SerializeField]
    private List<T> dataList;

    // Wrapper Ŭ������ ������
    public Wrapper(List<T> dataList)
    {
        this.dataList = dataList;
    }

    // ������ ����Ʈ�� ������Ƽ�� ����
    public List<T> Datas => dataList;
}



