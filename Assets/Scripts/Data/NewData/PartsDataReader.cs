using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

[System.Serializable]
public class Ability
{
    public int key;
    public float value;
}

[System.Serializable]
public class OwnPartsData
{
    public int inventoryCode;
    public int masterCode;
    public bool isOn;
    public int grade;
    public int mainAbility;
    public Ability ability1;
    public Ability ability2;
    public Ability ability3;
    public Ability ability4;
    public Ability ability5;

}

[System.Serializable]
public class OwnPartsList
{
    public OwnPartsData[] ownParts;
}

public class PartsDataReader
{
    public Dictionary<int, OwnPartsData> ownPartsDic;

    public void LoadData()
    {
        TextAsset json = Resources.Load<TextAsset>("JSON/OwnParts");
        if (json == null)
        {
            Debug.LogError("OwnPartsData: JSON이 로드되지 않음");
            return;
        }

        OwnPartsList dataInstance = JsonUtility.FromJson<OwnPartsList>(json.text);
        if (dataInstance == null || dataInstance.ownParts == null)
        {
            Debug.LogError("OwnPartsData: 파싱이 제대로 이루어지지 않음");
            return;
        }
        
        ownPartsDic = new Dictionary<int, OwnPartsData>();
        foreach (OwnPartsData ownParts in dataInstance.ownParts)
        {
            ownPartsDic.Add(ownParts.masterCode, ownParts);
        }

        Debug.Log($"OwnPartsData: {ownPartsDic.Count}개의 아이템이 로드됨");
    }

    public static void ApplyAbilityToCharacter(ref Character result, Ability ability)
    {
        if (ability == null) return; // 능력이 null이면 무시

        AbilityData abilityData;
        bool success = DataManager.abilityData.abilityDic.TryGetValue(ability.key, out abilityData);
        if (!success) return; // 능력 데이터를 찾을 수 없으면 무시

        switch (abilityData.specialAbility)
        {
            case "damage":
                result.damage += ability.value;
                break;
            case "defense":
                result.defense += ability.value;
                break;
            case "attackSpeed":
                result.attackSpeed += ability.value;
                break;
            case "movementSpeed":
                result.movementSpeed += ability.value;
                break;
            case "maxHealth":
                result.maxHealth += ability.value;
                break;
            case "hpRegen":
                result.hpRegen += ability.value;
                break;
            case "troopsDamageUp":
                result.troopsDamageUp += ability.value;
                break;
            case "bossDamageUp":
                result.bossDamageUp += ability.value;
                break;
            case "stageExpRateUp":
                result.stageExpRateUp += ability.value;
                break;
            case "stageItemDropRateUp":
                result.stageItemDropRateUp += ability.value;
                break;
            case "powRegenRateUp":
                result.powRegenRateUp += ability.value;
                break;
            case "powAmountUp":
                result.powAmountUp += ability.value;
                break;
            case "accountExpUp":
                result.accountExpUp += ability.value;
                break;
            case "accountMoneyUp":
                result.accountMoneyUp += ability.value;
                break;
            case "startLevelUp":
                result.startLevelUp += ability.value;
                break;
            case "revival":
                result.revival += ability.value;
                break;
            case "startWeaponUp":
                result.startWeaponUp += ability.value;
                break;
            default:
                Debug.LogWarning($"Unknown ability type: {abilityData.specialAbility}");
                break;
        }
    }

}
