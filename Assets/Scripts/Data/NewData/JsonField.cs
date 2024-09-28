using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
public class StoreItemData
{
    public int storeItemId;
    public int masterId;
    public int[] amount;
}
[System.Serializable]
public class AbilityData
{
    public int id;
    public string name;
    public string minRank;
    public string basicValue;
    public AbilityRate SValue;
    public AbilityRate AValue;
    public AbilityRate BValue;
    public AbilityRate CValue;
    public AbilityRate DValue;
}

[System.Serializable]
public class PartsData
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
public class AccountData
{
    public int id;
    public string name;
    public int level;
    public int exp;
    public int stageProgress;
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
public struct InvenData //필드값
{
    public int id;
    public int masterId;
    public string name;
    public int quantity;
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