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
        if(dataDict.Count == 0)
        {
            Debug.LogWarning($"{typeof(T).Name}에 데이터 {dataDict.Count}개가 들어옴");
        }
        else
        {
            Debug.Log($"{typeof(T).Name}에 데이터 {dataDict.Count}개가 들어옴");
        }
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
        else if (typeof(T) == typeof(SkillData))
        {
            var wrapper = JsonUtility.FromJson<SkillDataWrapper>(json);
            dataList = wrapper.SkillData as List<T>;
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
        else if (typeof(T) == typeof(PartsAbilityData))
        {
            var wrapper = JsonUtility.FromJson<PartsAbilityDataWrapper>(json);
            dataList = wrapper.PartsAbilityData as List<T>;
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
        else if (typeof(T) == typeof(GotchaData))
        {
            var wrapper = JsonUtility.FromJson<GotchaDataWrapper>(json);
            dataList = wrapper.GotchaData as List<T>;
        }


        // 필요한 만큼 else if 블록을 추가
        return dataList;
    }

    public string GetFilePath()
    {
        return filePath;
    }

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

    public bool IsDataExist(int id)
    {
        return dataDict.ContainsKey(id) ? true : false;
    }

}

// 읽기 및 쓰기 가능 데이터 관리 클래스
public abstract class EditableData<T> : ReadOnlyData<T>
{
    public int GetLastKey()
    {
        keysList = keysList.OrderBy(x => x).ToList(); // 키 리스트 정렬
        return keysList[keysList.Count - 1];
    }
    // 추가
    public bool AddData(T data)
    {
        int id = GetId(data); // 데이터를 추가하기 전에 ID를 가져옵니다.
        if (!dataDict.ContainsKey(id)) //해당 아이디가 없음
        {
            dataDict.Add(id,data);
            keysList.Add(id); // 키 리스트에 추가
            keysList = keysList.OrderBy(x => x).ToList(); // 키 리스트 정렬
            return true;
        }
        
        return false; // 중복 ID의 경우 false 반환
    }

    // 삭제
    public bool DeleteData(int id)
    {
        if (!dataDict.ContainsKey(id))
        {
            Debug.Log($"ID {id}가 존재하지 않음");
            return false;
        }

        dataDict.Remove(id);
        keysList.Remove(id); // 키 리스트에서 삭제
        return true;
    }

    // 업데이트
    public bool UpdateData(int id, T data)
    {
        if (!dataDict.ContainsKey(id))
        {
            Debug.Log($"ID {id}가 존재하지 않음");
            return false;
        }

        dataDict[id] = data;
        return true;
    }

    // JSON으로 저장하는 함수 (필요 시 구현)

    public void SaveData()
    {
        List<int> sortedKeys = dataDict.Keys.OrderBy(key => key).ToList();

        // 정렬된 키에 따라 데이터 리스트를 생성
        List<T> sortedDataList = new List<T>();
        foreach (int key in sortedKeys)
        {
            sortedDataList.Add(dataDict[key]);
        }

        // Wrapper를 사용하여 JSON으로 변환
        Wrapper<T> wrapper = new Wrapper<T>(sortedDataList);
        string json = JsonUtility.ToJson(wrapper, true);

        // JSON 필드명 변경
        json = json.Replace("\"dataList\"", $"\"{DataManager.dataFieldNames[fieldType]}\"");

        // 파일에 저장
        File.WriteAllText(filePath, json);
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

}

// 제네릭 리스트를 JSON으로 감싸는 Wrapper 클래스
[System.Serializable]
public class Wrapper<T>
{
    // json의 동적 필드명을 저장하기 위해 추가
    [SerializeField]
    private List<T> dataList;

    // Wrapper 클래스의 생성자
    public Wrapper(List<T> dataList)
    {
        this.dataList = dataList;
    }

    // 데이터 리스트를 프로퍼티로 제공
    public List<T> Datas => dataList;
}



