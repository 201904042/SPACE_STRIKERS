using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class MasterDataReader : ReadOnlyData<MasterData>
{
    protected override int GetId(MasterData data)
    {
        fieldType = DataFieldType.MasterData;
        return data.id;
    }

    public List<MasterData> GetItemsByType(MasterType type)
    {
        // Ư�� type�� �����͸� ���͸��Ͽ� ��ȯ
        return dataDict.Values.Where(item => item.type == type).ToList();
    }
}

public class StoreItemReader : ReadOnlyData<StoreItemData>
{
    protected override int GetId(StoreItemData data)
    {
        fieldType = DataFieldType.StoreData;
        return data.storeItemId;
    }
}

public class AbilityDataReader : ReadOnlyData<AbilityData>
{
    protected override int GetId(AbilityData data)
    {
        fieldType = DataFieldType.AbilityData;
        return data.id;
    }
}

public class StageDataReader : ReadOnlyData<StageData>
{
    protected override int GetId(StageData data)
    {
        fieldType = DataFieldType.StageData;
        return data.stageCode;
    }

}

public class UpgradeDataReader : ReadOnlyData<UpgradeData>
{
    protected override int GetId(UpgradeData data)
    {
        fieldType = DataFieldType.UpgradeData;
        return data.masterId;
    }
}

public class EnemyDataReader : ReadOnlyData<EnemyData>
{
    protected override int GetId(EnemyData data)
    {
        fieldType = DataFieldType.EnemyData;
        return data.id;
    }
}

public class GotchaDataReader : ReadOnlyData<GotchaData>
{
    protected override int GetId(GotchaData data)
    {
        fieldType = DataFieldType.GotchaData;
        return data.id;
    }
}



public class AccountJsonReader : EditableData<AccountData>
{
    protected override int GetId(AccountData data)
    {
        fieldType = DataFieldType.AccountData;
        return data.id;
    }

    public void SetCharValue(int id)
    {
        dataDict[0].useChar = id;
        SaveData();
    }

    public void SetStageValue(int value)
    {
        dataDict[0].stageIndex = value;
        SaveData();
    }
    public void SetPlanetValue(int value)
    {
        dataDict[0].planetIndex = value;
        SaveData();
    }

    public void AddExp(int addAmount)
    {
        dataDict[0].exp = addAmount;
        CalculateLevel();
        SaveData();
    }

    public void SetParts(int slotNum, int partsInvenId)
    {
        dataDict[0].useParts[slotNum-1] = partsInvenId;
        SaveData();
    }

    public void SetStageProgress(int progress)
    {
        dataDict[0].stageProgress = progress;
        SaveData();
    }

    public void CalculateLevel()
    {
        AccountData data = dataDict[0];
        int level = 1; // �⺻ ���� 1

        for (int i = 0; i < data.needExp.Count; i++)
        {
            if (data.needExp[i] == -1) // -1�̸� �� �̻� ������ �Ұ�
            {
                break;
            }

            if (data.exp >= data.needExp[i]) // ���� ����ġ�� �ʿ��� ����ġ �̻��� ��� ������
            {
                level++;
            }
            else
            {
                break; // ���� ����ġ�� �ʿ� ����ġ���� ������ �� �̻� ������ �Ұ�
            }
        }

        data.level = level;
    }

    public int GetChar()
    {
        return dataDict[0].useChar;
    }
    public int[] GetPartsArray()
    {
        return dataDict[0].useParts;
    }
    public int GetParts(int slotId)
    {
        return dataDict[0].useParts[slotId-1];
    }
    public int GetPlanet()
    {
        return dataDict[0].planetIndex;
    }
    public int GetStage()
    {
        return dataDict[0].stageIndex;
    }
    public int GetStageProgress()
    {
        return dataDict[0].stageProgress;
    }
}


public class InventoryDataReader : EditableData<InvenData>
{
    protected override int GetId(InvenData data)
    {
        fieldType = DataFieldType.InvenData;
        return data.id;
    }

    public InvenData GetDataWithMasterId(int masterId)
    {
        foreach (InvenData value in dataDict.Values)
        {
            if (value.masterId == masterId)
            {
                return value;
            }
        }
        return null;
    }

    public bool IsEnoughItem(int invenId, int needAmount)
    {
        InvenData check = GetData(invenId);

        if(check == null)
        {
            Debug.Log("�ش� �����;��̵� ���� �������� �������� ����");
            return false;
        }
        

        if (check.quantity < needAmount)
        {
            Debug.Log($"�ش� {check.id} �������� ���� ���ġ ����");
            return false;
        }

        return true;
    }

    public void DataAddOrUpdate(int masterId, int amount)
    {
        InvenData checkData = DataManager.inven.GetDataWithMasterId(masterId);
        if (checkData != null)
        {
            InvenData invenData = checkData;
            invenData.quantity += amount;
            DataManager.inven.UpdateData(invenData.id, invenData);
        }
        else
        {
            InvenData newData = new InvenData
            {
                id = DataManager.inven.GetLastKey() + 1,
                masterId = masterId,
                quantity = amount,
                name = DataManager.master.GetData(masterId).name
            };
            DataManager.inven.AddData(newData);
        }
    }

    public void DataUpdateOrDelete(int invenId, int amount)
    {
        InvenData invenData = DataManager.inven.GetData(invenId);
        invenData.quantity -= amount;
        if (invenData.quantity > 0)
        {
            DataManager.inven.UpdateData(invenData.id, invenData);
        }
        else
        {
            MasterData targetMaster = DataManager.master.GetData(invenData.masterId);
            if (targetMaster.type == MasterType.Money) 
            {
                //�Ӵ� �����ʹ� ���� ���� ����
                return;
            }
            if(targetMaster.type == MasterType.Parts)
            {
                //������� �������� �����͵� �������
                DataManager.parts.DeleteData(invenId);
            }
            DataManager.inven.DeleteData(invenData.id);
        }
    }
}

public class CharacterDataReader : EditableData<CharData>
{
    protected override int GetId(CharData data)
    {
        fieldType = DataFieldType.CharData;
        return data.id;
    }
}

public class PartsAbilityDataReader : EditableData<PartsAbilityData>
{

    protected override int GetId(PartsAbilityData data)
    {
        fieldType = DataFieldType.PartsAbilityData;
        return data.invenId;
    }

    public static void ApplyAbilityToCharacter(ref CharData result, Ability ability)
    {
        if (ability == null) return; // �ɷ��� null�̸� ����

        for (int i = 0; i < result.abilityDatas.Count; i++)
        {
            if (result.abilityDatas[i].key == ability.key)
            {
                result.abilityDatas[i].value += ability.value; // ���� value�� �߰�
                return; // ������Ʈ �� �޼��� ����
            }
        }

        //��ã�Ҵٸ� ����Ʈ�� �ش� �����Ƽ �߰�
        result.abilityDatas.Add(ability);
    }

}

