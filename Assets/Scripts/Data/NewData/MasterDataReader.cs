using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class MasterDataReader
{
    public Dictionary<int, MasterItemData> masterItemDic;

    public void LoadData()
    {
        masterItemDic = DataManager.SetDictionary<MasterItemData, MasterItemDatas>("JSON/MasterData",
            data => data.masterItems,
            item => item.masterId
            );
    }
}
