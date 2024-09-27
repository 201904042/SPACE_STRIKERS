using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;


public class PartsDataReader
{
    public Dictionary<int, InvenPartsData> invenPartsDic;

    public void LoadData()
    {
        invenPartsDic = DataManager.SetDictionary<InvenPartsData, InvenPartsDatas>("JSON/Writable/InvenPartsData" +
            "",
            data => data.InvenPartsData,
            item => item.invenId
            );
    }

    public InvenPartsData? GetData(int targetId)
    {
        if (!invenPartsDic.ContainsKey(targetId))
        {
            Debug.Log($"�ش� ���̵� ����");
            return null;
        }
        return invenPartsDic[targetId];
    }

    public static void ApplyAbilityToCharacter(ref CharData result, Ability ability)
    {
        if (ability == null) return; // �ɷ��� null�̸� ����

        AbilityData abilityData;
        bool success = DataManager.abilityData.abilityDic.TryGetValue(ability.key, out abilityData);
        if (!success) return; // �ɷ� �����͸� ã�� �� ������ ����

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
            case "moveSpeed":
                result.moveSpeed += ability.value;
                break;
            case "hp":
                result.hp += ability.value;
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
