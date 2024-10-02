using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor.U2D.Animation;
using UnityEngine;

public abstract class ReadOnlyData<T>
{
    protected Dictionary<int, T> dataDict = new Dictionary<int, T>();

    // JSON 파일에서 데이터를 로드하여 딕셔너리에 저장하는 함수
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

        Debug.Log($"{typeof(T).Name}에 데이터 {dataDict.Count}개가 들어옴");
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
        
        // 필요한 만큼 else if 블록을 추가
        return dataList;
    }

    // 데이터에서 ID를 추출하는 추상 메서드
    protected abstract int GetId(T data);

    // 검색
    public T GetData(int id)
    {
        return dataDict.ContainsKey(id) ? dataDict[id] : default;
    }

    public Dictionary<int, T> GetDictionary()
    {
        return dataDict;
    }

}

// 읽기 및 쓰기 가능 데이터 관리 클래스
public abstract class EditableData<T> : ReadOnlyData<T>
{
    // 추가
    public void AddData(int id, T data)
    {
        dataDict[id] = data;
    }

    // 삭제
    public void DeleteData(int id)
    {
        if (dataDict.ContainsKey(id))
        {
            dataDict.Remove(id);
        }
    }

    // 업데이트
    public void UpdateData(int id, T data)
    {
        if (dataDict.ContainsKey(id))
        {
            dataDict[id] = data;
        }
    }

    // JSON으로 저장하는 함수 (필요 시 구현)
    public void SaveData(string filePath)
    {
        Wrapper<T> wrapper = new Wrapper<T> { datas = new List<T>(dataDict.Values) };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(filePath, json);
    }
}

// 제네릭 리스트를 JSON으로 감싸는 Wrapper 클래스
[System.Serializable]
public class Wrapper<T>
{
    public List<T> datas;
}


