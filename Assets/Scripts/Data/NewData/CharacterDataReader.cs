using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Character
{
    public int masterCode;
    public string name;
    public int level;
    public float damage; //�⺻ �ɷ�ġ
    public float defense;
    public float attackSpeed;
    public float movementSpeed;
    public float maxHealth;

    public float hpRegen; //Ư�� �ɷ�ġ
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
            Debug.LogError("CharacterData: JSON�� �ε���� ����");
            return;
        }

        CharacterList dataInstance = JsonUtility.FromJson<CharacterList>(json.text);
        if (dataInstance == null || dataInstance.characters == null)
        {
            Debug.LogError("CharacterData: �Ľ��� ����� �̷������ ����");
            return;
        }

        characterDic = new Dictionary<int, Character>();
        foreach (Character character in dataInstance.characters)
        {
            characterDic.Add(character.masterCode, character);
        }

        Debug.Log($"CharacterData: {characterDic.Count}���� �������� �ε��");
    }
}
