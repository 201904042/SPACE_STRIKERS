using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.U2D.Animation;
using UnityEngine;
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

        AbilityData abilityData = DataManager.ability.GetData(ability.key);

        if (abilityData == null) return; // 능력 데이터를 찾을 수 없으면 무시

        switch (abilityData.id)
        {
            case 1:
                result.damage += ability.value;
                break;
            case 2:
                result.defense += ability.value;
                break;
            case 3:
                result.attackSpeed += ability.value;
                break;
            case 4:
                result.moveSpeed += ability.value;
                break;
            case 5:
                result.hp += ability.value;
                break;
            case 101:
                result.hpRegen += ability.value;
                break;
            case 102:
                result.troopsDamageUp += ability.value;
                break;
            case 103:
                result.bossDamageUp += ability.value;
                break;
            case 104:
                result.stageExpRateUp += ability.value;
                break;
            case 105:
                result.stageItemDropRateUp += ability.value;
                break;
            case 201:
                result.powRegenRateUp += ability.value;
                break;
            case 202:
                result.powAmountUp += ability.value;
                break;
            case 203:
                result.accountExpUp += ability.value;
                break;
            case 204:
                result.accountMoneyUp += ability.value;
                break;
            case 301:
                result.startLevelUp += ability.value;
                break;
            case 401:
                result.revival += ability.value;
                break;
            case 402:
                result.startWeaponUp += ability.value;
                break;
            default:
                Debug.LogWarning($"Unknown ability type: {abilityData.id}");
                break;
        }
    }

}

