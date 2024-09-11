[System.Serializable]
public class StoreItemData
{
    public int storeItemId;
    public int masterId;
    public int[] amount;
}

[System.Serializable]
public class StoreItemDatas
{
    public StoreItemData[] storeItem;
}

[System.Serializable]
public struct MasterItemData //필드값
{
    public int masterId;
    public string name;
    public int type;
    public string description;
    public string spritePath;
    public int buyPrice;
    public int sellPrice;
}

[System.Serializable]
public class MasterItemDatas //리스트
{
    public MasterItemData[] masterItems; // JSON에서의 루트 필드와 매칭
}

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
public class OwnPartDatas
{
    public OwnPartsData[] ownParts;
}

[System.Serializable]
public class AbilityData
{
    public int abilityCode;
    public string specialAbility;
    public string minRank;
    public string basicAbilityValue;
    public AbilityRate S;
    public AbilityRate A;
    public AbilityRate B;
    public AbilityRate C;
    public AbilityRate D;
}

[System.Serializable]
public class AbilityRate
{
    public float? min;
    public float? max;
}

[System.Serializable]
public class AbilityDatas
{
    public AbilityData[] abilityData;
}

[System.Serializable]
public class AccountData
{
    public int accountId;
    public string accountName;
    public int accountLevel;
    public int currentExperience;
    public int stageProgress;
}


[System.Serializable]
public class AccountDatas
{
    public AccountData accountData;
}



[System.Serializable]
public struct CharData
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
public class CharDatas
{
    public CharData[] characters;
}

[System.Serializable]
public struct InvenItemData //필드값
{
    public int storageId;
    public int itemType;
    public int masterId;
    public string name;
    public int amount;
}

[System.Serializable]
public class InvenItemDatas //리스트
{
    public InvenItemData[] storageItems;
}
