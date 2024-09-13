using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;


public class PartsDataReader
{
    public Dictionary<int, OwnPartsData> ownPartsDic;

    public void LoadData()
    {
        ownPartsDic = DataManager.SetDictionary<OwnPartsData, OwnPartDatas>("JSON/Writable/OwnParts",
            data => data.ownParts,
            item => item.masterCode
            );
    }

    public static void ApplyAbilityToCharacter(ref CharData result, Ability ability)
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
