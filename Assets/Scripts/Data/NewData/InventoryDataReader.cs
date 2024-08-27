using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct InventoryItem //필드값
{
    public int storageId;
    public int itemType;
    public int itemCode;
    public string name;
    public int amount;
}

[System.Serializable]
public class InventoryList //리스트
{
    public List<InventoryItem> storageItems;
}

public class InventoryDataReader : MonoBehaviour
{
    public List<InventoryItem> InvenItemList; // 임시 확인용
    public Dictionary<int, InventoryItem> InvenItemDic; //코드를 통해 검색용

    private void Awake()
    {
        LoadData();
    }
    public void LoadData()
    {
        TextAsset json = Resources.Load<TextAsset>("JSON/InventoryData");
        InventoryList dataInstance = JsonUtility.FromJson<InventoryList>(json.text);
        InvenItemList = new List<InventoryItem>(dataInstance.storageItems);
        InvenItemDic = new Dictionary<int, InventoryItem>();
        foreach (InventoryItem item in InvenItemList)
        {
            InvenItemDic.Add(item.storageId, item);
        }
    }
}
