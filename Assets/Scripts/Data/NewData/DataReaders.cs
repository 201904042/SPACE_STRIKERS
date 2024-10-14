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

    public bool IsEnoughItem(int masterId, int needAmount)
    {
        InvenData? check = GetDataWithMasterId(masterId);

        if(check == null)
        {
            Debug.Log("해당 마스터아이디를 가진 아이템이 존재하지 않음");
            return false;
        }
        

        if (check.quantity < needAmount)
        {
            Debug.Log($"해당 {check.id} 아이템의 량이 충분치 않음");
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
        if (ability == null) return; // 능력이 null이면 무시

        for (int i = 0; i < result.abilityDatas.Count; i++)
        {
            if (result.abilityDatas[i].key == ability.key)
            {
                result.abilityDatas[i].value += ability.value; // 기존 value에 추가
                return; // 업데이트 후 메서드 종료
            }
        }

        //못찾았다면 리스트에 해당 어빌리티 추가
        result.abilityDatas.Add(ability);
    }

}

