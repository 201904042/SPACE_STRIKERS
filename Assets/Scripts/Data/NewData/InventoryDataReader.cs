using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct InventoryItem //�ʵ尪
{
    public int storageId;
    public int itemType;
    public int itemCode;
    public string name;
    public int amount;
}

[System.Serializable]
public class InventoryList //����Ʈ
{
    public List<InventoryItem> storageItems;
}

public class InventoryDataReader : MonoBehaviour
{
    public List<InventoryItem> InvenItemList; // �ӽ� Ȯ�ο�
    public Dictionary<int, InventoryItem> InvenItemDic; //�ڵ带 ���� �˻���

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
