using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; // ���� ���� �۾��� �ʿ�
using System.Text; // �ؽ�Ʈ ���ڵ��� �ʿ�

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

public class InventoryDataReader : MonoBehaviour
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
            Debug.Log("InvenData : json�� �ε���� ����");
            return;
        }

        InventoryList dataInstance = JsonUtility.FromJson<InventoryList>(json.text);
        if (dataInstance == null)
        {
            Debug.Log("InvenData : �Ľ��� ����� �̷������ ����");
            return;
        }

        InvenItemDic = new Dictionary<int, InventoryItem>();
        foreach (InventoryItem item in dataInstance.storageItems)
        {
            InvenItemDic.Add(item.storageId, item);
        }

        Debug.Log($"InvenData : {InvenItemDic.Count}���� �������� �ε��");
    }

    // �����͸� �����ϰ� JSON ������ �����ϴ� �޼���
    public void SaveData()
    {
        // InventoryList ��ü ����
        InventoryList dataInstance = new InventoryList
        {
            storageItems = new List<InventoryItem>(InvenItemDic.Values)
        };

        // �����͸� JSON ���ڿ��� ��ȯ
        string jsonData = JsonUtility.ToJson(dataInstance, true);

        // JSON �����͸� ������ ���� ��� ���� (���� ������ ���)
        string path = Path.Combine(Application.persistentDataPath, "InventoryData.json");

        // JSON ���Ϸ� ����
        File.WriteAllText(path, jsonData, Encoding.UTF8);

        Debug.Log($"InvenData : JSON ������ {path}�� �����");
    }

    /// <summary>
    /// �ش� �������� �κ��丮�� �����ϸ� ���� ������Ʈ, ������ �κ� ������ �߰�
    /// </summary>
    public void AddNewItem(int itemType, int masterId, string name, int amount)
    {
        InventoryItem? findItem = FindByMasterId(masterId);
        if (findItem != null)  //���Ϸ��� �������� �̹� �����Ѵٸ� 
        {
            ModifyItem(findItem.Value.storageId, findItem.Value.amount + amount);
            return;
        }

        //Ȯ�ΰ�� �κ��丮�� �������� �ʴ� ������ -> ���ο� �κ��丮 ������ �߰�
        int newStorageId = 0; // �⺻��
        if (InvenItemDic.Count > 0)
        {
            newStorageId = InvenItemDic[InvenItemDic.Count - 1].storageId + 1;
        }


        InventoryItem newItem = new InventoryItem();
        newItem.storageId = newStorageId;
        newItem.itemType = itemType;
        newItem.masterId = masterId;
        newItem.name = name;
        newItem.amount = amount;
        

        InvenItemDic.Add(newStorageId, newItem);
    }

    // ������ ������ �����͸� ����
    public void ModifyItem(int storageId, int newAmount)
    {
        if (InvenItemDic.ContainsKey(storageId))
        {
            InventoryItem item = InvenItemDic[storageId];
            item.amount = newAmount; // �������� ������ ����
            InvenItemDic[storageId] = item;
            Debug.Log($"InvenData : ������ {item.name}�� ������ {newAmount}�� ������");
        }
        else
        {
            Debug.Log($"InvenData : ID {storageId}�� ���� �������� �������� ����");
        }
    }

    public InventoryItem? FindByMasterId(int masterId)
    {
        foreach (KeyValuePair<int, InventoryItem> pair in InvenItemDic)
        {
            if (pair.Value.masterId == masterId)
            {
                return pair.Value; // �ش� masterId�� ���� �������� ã��
            }
        }
        Debug.Log("InvenData : �ش� masterId�� ���� �������� �����ϴ�.");
        return null; // �ش� �������� ���� ��� null ��ȯ
    }
}
