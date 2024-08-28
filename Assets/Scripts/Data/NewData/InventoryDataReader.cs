using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct InventoryItem //필드값
{
    public int storageId;
    public int itemType;
    public int masterId;
    public string name;
    public int amount;
}

[System.Serializable]
public class InventoryList //리스트
{
    public List<InventoryItem> storageItems;
}

public class InventoryDataReader
{
    public Dictionary<int, InventoryItem> InvenItemDic; //코드를 통해 검색용

    private void Awake()
    {
        LoadData();
    }
    public void LoadData()
    {
        TextAsset json = Resources.Load<TextAsset>("JSON/InventoryData");
        if (json == null)
        {
            Debug.Log("InvenData :json이 로드되지 않음");
            return;
        }
        InventoryList dataInstance = JsonUtility.FromJson<InventoryList>(json.text);
        if (json == null)
        {
            Debug.Log("InvenData :파씽이 재대로 이루어지지 않음");
            return;
        }
        InvenItemDic = new Dictionary<int, InventoryItem>();
        foreach (InventoryItem item in dataInstance.storageItems)
        {
            InvenItemDic.Add(item.storageId, item);
        }

        Debug.Log($"InvenData : {InvenItemDic.Count}개의 아이템이 로드됨");
    }
}
