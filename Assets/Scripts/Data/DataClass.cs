using System;

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
    public StoreItemData[] DailyShopList;
}

[System.Serializable]
public struct MasterData //필드값
{
    public int id;
    public string name;
    public ItemType type;
    public string description;
    public string spritePath;
}

[System.Serializable]
public class MasterDatas //리스트
{
    public MasterData[] MasterData; // JSON에서의 루트 필드와 매칭
}

[System.Serializable]
public class Ability
{
    public int key;
    public float value;
}

[System.Serializable]
public class InvenPartsData
{
    public int invenId;
    public bool isActive;
    public int rank;
    public int mainAbility;
    public Ability subAbility1;
    public Ability subAbility2;
    public Ability subAbility3;
    public Ability subAbility4;
    public Ability subAbility5;
}

[System.Serializable]
public class InvenPartsDatas
{
    public InvenPartsData[] InvenPartsData;
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
    public int id;
    public string name;
    public int level;
    public int exp;
    public int stageProgress;
}


[System.Serializable]
public class AccountDatas
{
    public AccountData AccountData;
}



[System.Serializable]
public struct CharData
{
    public int id;
    public string name;
    public int level;

    public float damage; //기본 능력치
    public float defense;
    public float attackSpeed;
    public float moveSpeed;
    public float hp;

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
public struct InvenData //필드값
{
    public int id;
    public int masterId;
    public string name;
    public int quantity;
}

[System.Serializable]
public class InvenDatas //리스트
{
    public InvenData[] InvenData;
}

[System.Serializable]
public class StageData
{
    public int stageCode;
    public string stageType;
    public StageEnemyData[] stageEnemy;
    public StageItemReward[] firstReward;
    public StageItemReward[] defaultReward;
}

[Serializable]
public class StageEnemyData
{
    public int enemyId;
    public int quantity;
}

[Serializable]
public class StageItemReward
{
    public int itemId;
    public int quantity;
}

[System.Serializable]
public class StageDatas
{
    public StageData[] StageData;
}



public enum ItemType
{
    Money,
    Character,
    Parts,
    Ingredient,
    Consume
}
