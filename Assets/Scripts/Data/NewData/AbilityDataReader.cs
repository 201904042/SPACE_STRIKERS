using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class AbilityData
{
    public int abilityCode;
    public string specialAbility;
    public string minRank;
    public string basicAbilityValue;
    public AbilityRate S;
    public AbilityRate A;
    public AbilityRate B;
    public AbilityRate C;
    public AbilityRate D;
}

[Serializable]
public class AbilityRate
{
    public float? min;
    public float? max;
}

[Serializable]
public class AbilityDataList
{
    public AbilityData[] abilityData;
}

public class AbilityDataReader
{
    public Dictionary<int, AbilityData> abilityDic;

    public void LoadData()
    {
        TextAsset json = Resources.Load<TextAsset>("JSON/AbilityData");
        if (json == null)
        {
            Debug.LogError("AbilityData: JSON이 로드되지 않음");
            return;
        }

        AbilityDataList dataInstance = JsonUtility.FromJson<AbilityDataList>(json.text);
        if (dataInstance == null || dataInstance.abilityData == null)
        {
            Debug.LogError("AbilityData: 파싱이 제대로 이루어지지 않음");
            return;
        }

        abilityDic = new Dictionary<int, AbilityData>();
        foreach (AbilityData ownParts in dataInstance.abilityData)
        {
            abilityDic.Add(ownParts.abilityCode, ownParts);
        }

        Debug.Log($"AbilityData: {abilityDic.Count}개의 아이템이 로드됨");
    }
}
