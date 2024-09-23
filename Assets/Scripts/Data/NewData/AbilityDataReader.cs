using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityDataReader 
{
    public Dictionary<int, AbilityData> abilityDic;

    public void LoadData()
    {
        abilityDic = DataManager.SetDictionary<AbilityData, AbilityDatas>("JSON/ReadOnly/AbilityData",
            data => data.abilityData,
            item => item.abilityCode
            );
    }

    public AbilityData? GetData(int targetId)
    {
        if (!abilityDic.ContainsKey(targetId))
        {
            Debug.Log($"해당 아이디 없음");
            return null;
        }
        return abilityDic[targetId];
    }
}
