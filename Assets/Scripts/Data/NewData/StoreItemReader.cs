using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StoreItemReader : MonoBehaviour
{
    public Dictionary<int, StoreItemData> storeItemDic;

    public void LoadData()
    {
        storeItemDic = DataManager.SetDictionary<StoreItemData, StoreItemDatas>("JSON/ReadOnly/DailyShopList",
            data => data.DailyShopList,
            item => item.storeItemId
            );
    }
}
