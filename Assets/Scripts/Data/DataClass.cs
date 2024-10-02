using System;

[System.Serializable]
public class Ability
{
    public int key;
    public float value;
}

[System.Serializable]
public class AbilityRate
{
    public float? min;
    public float? max;
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

[Serializable]
public class UpgradeCost
{
    public int level;
    public UpgradeIngred[] ingredients;
}

[Serializable]
public class UpgradeIngred
{
    public int ingredMasterId; 
    public int quantity;
}


public enum ItemType
{
    Money,
    Character,
    Parts,
    Ingredient,
    Consume
}
