using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

[System.Serializable]
public class Ability
{
    public int key;
    public float value;
}

[System.Serializable]
public class OwnPartsData
{
    public int inventoryCode;
    public int masterCode;
    public string grade;
    public int mainAbility;
    public Ability ability1;
    public Ability ability2;
    public Ability ability3;
    public Ability ability4;
    public Ability ability5;
}

[System.Serializable]
public class OwnPartsList
{
    public OwnPartsData[] ownParts;
}

public class PartsDataReader
{
    public Dictionary<int, OwnPartsData> ownPartsDic;

    public void LoadData()
    {
        TextAsset json = Resources.Load<TextAsset>("JSON/OwnParts");
        if (json == null)
        {
            Debug.LogError("OwnPartsData: JSON이 로드되지 않음");
            return;
        }

        OwnPartsList dataInstance = JsonUtility.FromJson<OwnPartsList>(json.text);
        if (dataInstance == null || dataInstance.ownParts == null)
        {
            Debug.LogError("OwnPartsData: 파싱이 제대로 이루어지지 않음");
            return;
        }
        
        ownPartsDic = new Dictionary<int, OwnPartsData>();
        foreach (OwnPartsData ownParts in dataInstance.ownParts)
        {
            ownPartsDic.Add(ownParts.masterCode, ownParts);
        }

        Debug.Log($"OwnPartsData: {ownPartsDic.Count}개의 아이템이 로드됨");
    }
}
