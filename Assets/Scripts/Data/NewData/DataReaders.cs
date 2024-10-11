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



public class AccountJsonReader : EditableData<AccountData>
{
    protected override int GetId(AccountData data)
    {
        fieldType = DataFieldType.AccountData;
        return data.id;
    }
}


public class InventoryDataReader : EditableData<InvenData>
{
    protected override int GetId(InvenData data)
    {
        fieldType = DataFieldType.InvenData;
        return data.id;
    }

    public InvenData? GetDataWithMasterId(int masterId)
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

    public bool IsEnoughItem(int masterId, int needAmount)
    {
        InvenData? check = GetDataWithMasterId(masterId);

        if(check == null)
        {
            Debug.Log("�ش� �����;��̵� ���� �������� �������� ����");
            return false;
        }
        InvenData data = (InvenData)check;

        if (data.quantity < needAmount)
        {
            Debug.Log($"�ش� {data.id} �������� ���� ���ġ ����");
            return false;
        }

        return true;
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

public class PartsDataReader : EditableData<PartsData>
{

    protected override int GetId(PartsData data)
    {
        fieldType = DataFieldType.PartsData;
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

