using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor.U2D.Animation;
using UnityEngine;

public abstract class ReadOnlyData<T>
{
    protected Dictionary<int, T> dataDict = new Dictionary<int, T>();

    // JSON ���Ͽ��� �����͸� �ε��Ͽ� ��ųʸ��� �����ϴ� �Լ�
    public void LoadData(string filePath)
    {
        string json = File.ReadAllText(filePath);
        List<T> dataList = null;
        dataList = SetJsonList(json, dataList);

        foreach (T data in dataList)
        {
            int id = GetId(data);
            dataDict[id] = data;
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
        
        // �ʿ��� ��ŭ else if ����� �߰�
        return dataList;
    }

    // �����Ϳ��� ID�� �����ϴ� �߻� �޼���
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
}

// �б� �� ���� ���� ������ ���� Ŭ����
public abstract class EditableData<T> : ReadOnlyData<T>
{
    // �߰�
    public void AddData(int id, T data)
    {
        dataDict[id] = data;
    }

    // ����
    public void DeleteData(int id)
    {
        if (dataDict.ContainsKey(id))
        {
            dataDict.Remove(id);
        }
    }

    // ������Ʈ
    public void UpdateData(int id, T data)
    {
        if (dataDict.ContainsKey(id))
        {
            dataDict[id] = data;
        }
    }

    // JSON���� �����ϴ� �Լ� (�ʿ� �� ����)
    public void SaveData(string filePath)
    {
        Wrapper<T> wrapper = new Wrapper<T> { datas = new List<T>(dataDict.Values) };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(filePath, json);
    }
}

// ���׸� ����Ʈ�� JSON���� ���δ� Wrapper Ŭ����
[System.Serializable]
public class Wrapper<T>
{
    public List<T> datas;
}


