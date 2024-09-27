using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; // ���� ���� �۾��� �ʿ�
using System.Text;
using System.Linq; // �ؽ�Ʈ ���ڵ��� �ʿ�


public class InventoryDataReader
{
    public Dictionary<int, InvenData> InvenItemDic; //�ڵ带 ���� �˻���

    private void Awake()
    {
        LoadData();
    }

    public void LoadData()
    {
        InvenItemDic = DataManager.SetDictionary<InvenData, InvenDatas>("JSON/Writable/InvenData",
            data => data.InvenData,
            item => item.id
            );

    }

    public InvenData? GetData(int targetId)
    {
        if (!InvenItemDic.ContainsKey(targetId))
        {
            Debug.Log($"�ش� ���̵� ����");
            return null;
        }
        return InvenItemDic[targetId];
    }

    // �����͸� �����ϰ� JSON ������ �����ϴ� �޼���
    public void SaveData()
    {
        // InvenDatas ��ü ����
        InvenDatas dataInstance = new InvenDatas
        {
            InvenData = InvenItemDic.Values.ToArray()
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
    public void AddNewItem(ItemType itemType, int masterId, string name, int amount)
    {
        InvenData? findItem = FindByMasterId(masterId);
        if (findItem != null)  //���Ϸ��� �������� �̹� �����Ѵٸ� 
        {
            ModifyItem(findItem.Value.id, findItem.Value.quantity + amount);
            return;
        }

        //Ȯ�ΰ�� �κ��丮�� �������� �ʴ� ������ -> ���ο� �κ��丮 ������ �߰�
        int newStorageId = 0; // �⺻��
        if (InvenItemDic.Count > 0)
        {
            newStorageId = InvenItemDic[InvenItemDic.Count - 1].id + 1;
        }


        InvenData newItem = new InvenData();
        newItem.id = newStorageId;
        newItem.masterId = masterId;
        newItem.name = name;
        newItem.quantity = amount;
        

        InvenItemDic.Add(newStorageId, newItem);
    }

    // ������ ������ �����͸� ����
    public void ModifyItem(int storageId, int newAmount)
    {
        if (InvenItemDic.ContainsKey(storageId))
        {
            InvenData item = InvenItemDic[storageId];
            item.quantity = newAmount; // �������� ������ ����
            InvenItemDic[storageId] = item;
            Debug.Log($"InvenData : ������ {item.name}�� ������ {newAmount}�� ������");
        }
        else
        {
            Debug.Log($"InvenData : ID {storageId}�� ���� �������� �������� ����");
        }
    }

    // todo -> �������� �����ϴ� �ڵ� �߰��Ұ�. storageId�� ������� ����� �� �ڿ� ��ҵ��� ���ܼ� ����� ����

    public InvenData? FindByMasterId(int masterId)
    {
        foreach (KeyValuePair<int, InvenData> pair in InvenItemDic)
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
