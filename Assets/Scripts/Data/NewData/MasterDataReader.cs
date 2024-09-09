using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[System.Serializable]
public struct MasterItem //필드값
{
    public int masterId;
    public string name;
    public int type;
    public string description;
    public string spritePath;
    public int buyPrice;
    public int sellPrice;
}

[System.Serializable]
public class MasterList //리스트
{
    public List<MasterItem> masterItems; // JSON에서의 루트 필드와 매칭
}

public class MasterDataReader
{
    public Dictionary<int, MasterItem> masterItemDic;

    public void LoadData()
    {
        TextAsset json = Resources.Load<TextAsset>("JSON/MasterData");
        if(json == null)
        {
            Debug.Log("MasterData :json이 로드되지 않음");
        }
        MasterList dataInstance = JsonUtility.FromJson<MasterList>(json.text);
        if (dataInstance == null)
        {
            Debug.Log("MasterData : 파씽이 재대로 이루어지지 않음");
        }

        masterItemDic = new Dictionary<int, MasterItem>();
        foreach (MasterItem item in dataInstance.masterItems)
        {
            masterItemDic.Add(item.masterId, item);
        }

        Debug.Log($"MasterData : {masterItemDic.Count}개의 아이템이 로드됨");
    }
}
