using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct InventoryItem //�ʵ尪
{
    public int storageId;
    public int itemType;
    public int masterId;
    public string name;
    public int amount;
}

[System.Serializable]
public class InventoryList //����Ʈ
{
    public List<InventoryItem> storageItems;
}

public class InventoryDataReader
{
    public Dictionary<int, InventoryItem> InvenItemDic; //�ڵ带 ���� �˻���

    private void Awake()
    {
        LoadData();
    }
    public void LoadData()
    {
        TextAsset json = Resources.Load<TextAsset>("JSON/InventoryData");
        if (json == null)
        {
            Debug.Log("InvenData :json�� �ε���� ����");
            return;
        }
        InventoryList dataInstance = JsonUtility.FromJson<InventoryList>(json.text);
        if (json == null)
        {
            Debug.Log("InvenData :�ľ��� ���� �̷������ ����");
            return;
        }
        InvenItemDic = new Dictionary<int, InventoryItem>();
        foreach (InventoryItem item in dataInstance.storageItems)
        {
            InvenItemDic.Add(item.storageId, item);
        }

        Debug.Log($"InvenData : {InvenItemDic.Count}���� �������� �ε��");
    }
}
