using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Character
{
    public int masterCode;
    public bool own;
    public string name;
    public int level;
    public float damage; //기본 능력치
    public float defense;
    public float attackSpeed;
    public float movementSpeed;
    public float maxHealth;

    public float hpRegen; //특수 능력치
    public float troopsDamageUp;
    public float bossDamageUp;
    public float stageExpRateUp;
    public float stageItemDropRateUp;
    public float powRegenRateUp;
    public float powAmountUp;
    public float accountExpUp;
    public float accountMoneyUp;
    public float startLevelUp;
    public float revival;
    public float startWeaponUp;
}

[System.Serializable]
public class CharacterList
{
    public List<Character> characters;
}

public class CharacterDataReader 
{
    public Dictionary<int, Character> characterDic;

    public void LoadData()
    {
        TextAsset json = Resources.Load<TextAsset>("JSON/CharacterData");
        if (json == null)
        {
            Debug.LogError("CharacterData: JSON이 로드되지 않음");
            return;
        }

        CharacterList dataInstance = JsonUtility.FromJson<CharacterList>(json.text);
        if (dataInstance == null || dataInstance.characters == null)
        {
            Debug.LogError("CharacterData: 파싱이 제대로 이루어지지 않음");
            return;
        }

        characterDic = new Dictionary<int, Character>();
        foreach (Character character in dataInstance.characters)
        {
            characterDic.Add(character.masterCode, character);
        }

        Debug.Log($"CharacterData: {characterDic.Count}개의 아이템이 로드됨");
    }
}
