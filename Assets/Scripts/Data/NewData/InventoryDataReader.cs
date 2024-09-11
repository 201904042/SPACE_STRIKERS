using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; // ���� ���� �۾��� �ʿ�
using System.Text;
using System.Linq; // �ؽ�Ʈ ���ڵ��� �ʿ�


public class InventoryDataReader : MonoBehaviour
{
    public Dictionary<int, InvenItemData> InvenItemDic; //�ڵ带 ���� �˻���

    private void Awake()
    {
        LoadData();
    }

    public void LoadData()
    {
        InvenItemDic = DataManager.SetDictionary<InvenItemData, InvenItemDatas>("JSON/InventoryData",
            data => data.storageItems,
            item => item.storageId
            );

    }

    // �����͸� �����ϰ� JSON ������ �����ϴ� �޼���
    public void SaveData()
    {
        // InvenItemDatas ��ü ����
        InvenItemDatas dataInstance = new InvenItemDatas
        {
            storageItems = InvenItemDic.Values.ToArray()
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
        InvenItemData? findItem = FindByMasterId(masterId);
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


        InvenItemData newItem = new InvenItemData();
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
            InvenItemData item = InvenItemDic[storageId];
            item.amount = newAmount; // �������� ������ ����
            InvenItemDic[storageId] = item;
            Debug.Log($"InvenData : ������ {item.name}�� ������ {newAmount}�� ������");
        }
        else
        {
            Debug.Log($"InvenData : ID {storageId}�� ���� �������� �������� ����");
        }
    }

    // todo -> �������� �����ϴ� �ڵ� �߰��Ұ�. storageId�� ������� ����� �� �ڿ� ��ҵ��� ���ܼ� ����� ����

    public InvenItemData? FindByMasterId(int masterId)
    {
        foreach (KeyValuePair<int, InvenItemData> pair in InvenItemDic)
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
