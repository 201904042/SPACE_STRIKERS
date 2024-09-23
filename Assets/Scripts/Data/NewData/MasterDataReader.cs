using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class MasterDataReader 
{ 
    public Dictionary<int, MasterItemData> masterItemDic;

    public void LoadData()
    {
        masterItemDic = DataManager.SetDictionary<MasterItemData, MasterItemDatas>("JSON/ReadOnly/masterItems",
            data => data.masterItems,
            item => item.masterId
            );
    }

    public MasterItemData? GetData(int targetId)
    {
        if (!masterItemDic.ContainsKey(targetId))
        {
            Debug.Log($"해당 아이디 없음");
            return null;
        }
        return masterItemDic[targetId];
    }
}
