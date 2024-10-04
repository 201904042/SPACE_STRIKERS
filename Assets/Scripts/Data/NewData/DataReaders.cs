using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class MasterDataReader : ReadOnlyData<MasterData>
{
    protected override int GetId(MasterData data)
    {
        return data.id;
    }
}

public class StoreItemReader : ReadOnlyData<StoreItemData>
{
    protected override int GetId(StoreItemData data)
    {
        return data.storeItemId;
    }
}

public class AbilityDataReader : ReadOnlyData<AbilityData>
{
    protected override int GetId(AbilityData data)
    {
        return data.id;
    }
}

public class StageDataReader : ReadOnlyData<StageData>
{
    protected override int GetId(StageData data)
    {
        return data.stageCode;
    }

}

public class UpgradeDataReader : ReadOnlyData<UpgradeData>
{
    protected override int GetId(UpgradeData data)
    {
        return data.masterId;
    }
}


public class AccountJsonReader : EditableData<AccountData>
{
    protected override int GetId(AccountData data)
    {
        return data.id;
    }
}


public class InventoryDataReader : EditableData<InvenData>
{
    protected override int GetId(InvenData data)
    {
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
}

public class CharacterDataReader : EditableData<CharData>
{
    protected override int GetId(CharData data)
    {
        return data.id;
    }
}

public class PartsDataReader : EditableData<PartsData>
{

    protected override int GetId(PartsData data)
    {
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

