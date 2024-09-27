using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class MasterDataReader 
{ 
    public Dictionary<int, MasterData> masterDic;

    public void LoadData()
    {
        masterDic = DataManager.SetDictionary<MasterData, MasterDatas>("JSON/ReadOnly/MasterData",
            data => data.MasterData,
            item => item.id
            );
    }

    public MasterData? GetData(int targetId)
    {
        if (!masterDic.ContainsKey(targetId))
        {
            Debug.Log($"해당 아이디 없음");
            return null;
        }
        return masterDic[targetId];
    }
}
