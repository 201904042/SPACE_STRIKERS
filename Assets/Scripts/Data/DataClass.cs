using System;
using System.Collections.Generic;
using Unity.VisualScripting;

[System.Serializable]
public class Ability
{
    public int key; //어빌리티 데이터의 키
    public float value; //스텟 가산량

    public Ability(Ability other)
    {
        key = other.key;
        value = other.value;
    }
    public Ability(int key, int abilityKey, float value)
    {
        this.key = key;
        this.value = value;
    }
    /// <summary>
    /// 깊은 복사
    /// </summary>
    public static List<Ability> CopyList(List<Ability> copy)
    {
        List <Ability> a = new List<Ability> ();
        foreach (Ability ability in copy) {
            a.Add(new Ability(ability));
        }
        return a;
    }

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
    public Ability[] upgradeValues;
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
