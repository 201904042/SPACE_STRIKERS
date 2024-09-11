using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityDataReader
{
    public Dictionary<int, AbilityData> abilityDic;

    public void LoadData()
    {
        abilityDic = DataManager.SetDictionary<AbilityData, AbilityDatas>("JSON/AbilityData",
            data => data.abilityData,
            item => item.abilityCode
            );
    }
}
