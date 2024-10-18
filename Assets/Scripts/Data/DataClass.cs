using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbilityRate
{
    public int min; //�����Ƽ�� ���� �ɼ��ִ� �ּڰ�
    public int max; //�ִ� �� ����( min , max)
}

[System.Serializable]
public class Ability
{
    public int key; //�����Ƽ �������� Ű
    public int value; //���� ���귮

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
    /// ���� ����
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
    public int costInvenId;   //�밡�� ���ҵ� ������ ���̵�
    public int costAmount;    //�밡�� ���ҵ� ������ ��
    public int targetMasterId; //��ȯ���� ������ ������ ���̵�
    public int tradeAmount; //��ȯ���� ������ �������� ��
    public bool isMultiTrade; //������ �ŷ� ����
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
    None,
    Money,
    Character,
    Parts,
    Ingredient, 
    Consume,
    Enemy 
}

