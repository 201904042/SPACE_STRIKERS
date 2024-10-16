using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbilityRate
{
    public int min; //어빌리티의 값이 될수있는 최솟값
    public int max; //최댓값 즉 랜덤( min , max)
}

[System.Serializable]
public class Ability
{
    public int key; //어빌리티 데이터의 키
    public int value; //스텟 가산량

    public Ability(Ability other)
    {
        key = other.key;
        value = other.value;
    }
    public Ability(int key, int value)
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

[System.Serializable]
public class GotchaInform
{
    public string type;
    public float rate;
}

[System.Serializable]
public class GotchaCost
{
    public int id;
    public int amount;
}


[System.Serializable]
public enum TradeType
{
    Item,
    Cash
}

[System.Serializable]
public class TradeData
{
    public TradeType tradeCost;
    public int costInvenId;   //대가로 감소될 아이템 아이디
    public int costAmount;    //대가로 감소될 아이템 양
    public int targetMasterId; //교환으로 증가될 아이템 아이디
    public int tradeAmount; //교환으로 증가될 아이템의 양
    public bool isMultiTrade; //여러번 거래 가능
}

public class PartsGradeColor
{
    public static Color S_Color = new Color(1, 1, 0, 1);
    public static Color A_Color = new Color(1, 0, 1, 1);
    public static Color B_Color = new Color(0, 0, 1, 1);
    public static Color C_Color = new Color(0, 1, 0, 1);
    public static Color D_Color = new Color(1, 1, 1, 1);
}


public enum MasterType
{
    Money,
    Character,
    Parts,
    Ingredient,
    Consume,
    Enemy,
    None
}

