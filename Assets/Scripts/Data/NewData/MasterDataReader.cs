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

    
}
