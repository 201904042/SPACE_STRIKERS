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

public class AccountDataReader : OnlyAccountData
{
    protected override string GetUserId(AccountData data)
    {
        fieldType = DataFieldType.AccountData;
        return data.id;
    }
    public void SetCharValue(int id)
    {
        data.useChar = id;
        SaveData();
    }
    public void SetStageValue(int value)
    {
        data.stageIndex = value;
        SaveData();
    }
    public void SetPlanetValue(int value)
    {
        data.planetIndex = value;
        SaveData();
    }
    public void AddExp(int addAmount)
    {
        data.exp += addAmount;

        while (true)
        {
            int maxExp = data.level * AccountData.DefaultMaxExp;
            if (data.exp >= maxExp)
            {
                data.level++;
                data.exp -= maxExp;
            }
            else
            {
                break;
            }
        }
        SaveData();
    }
    public void SetParts(int slotNum, int partsInvenId)
    {
        data.useParts[slotNum - 1] = partsInvenId;
        SaveData();
    }
    public void SetStageProgress(int progress)
    {
        data.stageProgress = progress;
        SaveData();
    }
    public int GetChar()
    {
        return data.useChar;
    }
    public int[] GetPartsArray()
    {
        return data.useParts;
    }
    public int GetParts(int slotId)
    {
        return data.useParts[slotId - 1];
    }
    public int GetPlanet()
    {
        return data.planetIndex;
    }
    public int GetStage()
    {
        return data.stageIndex;
    }
    public int GetStageProgress()
    {
        return data.stageProgress;
    }
}

public class MasterDataReader : ReadOnlyData<MasterData>
{
    protected override int GetId(MasterData data)
    {
        fieldType = DataFieldType.MasterData;
        return data.id;
    }


    public List<MasterData> GetItemsByType(MasterType type)
    {
        // 특정 type의 데이터를 필터링하여 반환
        return dataDict.Values.Where(item => item.type == type).ToList();
    }
}

public class StoreDataReader : ReadOnlyData<StoreData>
{
    protected override int GetId(StoreData data)
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

public class SkillDataReader : ReadOnlyData<SkillData>
{
    protected override int GetId(SkillData data)
    {
        fieldType = DataFieldType.SkillData;
        return data.id;
    }

    public int? GetUSkillIdFromCharId(int charId)
    {  
        SkillData data = dataDict.Values.First(skill => skill.useChar ==  charId && skill.type == SkillType.Unique);
        if(data == null)
        {
            Debug.LogError("유니크 스킬중 해당 캐릭터의 스킬이 없음");
            return null;
        }

        return data.id;
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
                //머니 데이터는 삭제 하지 않음
                return;
            }
            if(targetMaster.type == MasterType.Parts)
            {
                //파츠라면 파츠스텟 데이터도 연쇄삭제
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

