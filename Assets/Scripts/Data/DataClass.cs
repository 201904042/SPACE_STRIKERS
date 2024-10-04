using System;

[System.Serializable]
public class Ability
{
    public int key; //어빌리티 데이터의 키
    public float value; //스텟 가산량
}

[System.Serializable]
public class AbilityRate
{
    public float? min; //어빌리티의 값이 될수있는 최솟값
    public float? max; //최댓값 즉 랜덤( min , max)
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
