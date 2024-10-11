using System;
using System.Collections.Generic;
using Unity.VisualScripting;

[System.Serializable]
public class Ability
{
    public int key; //�����Ƽ �������� Ű
    public float value; //���� ���귮

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

[System.Serializable]
public class AbilityRate
{
    public float? min; //�����Ƽ�� ���� �ɼ��ִ� �ּڰ�
    public float? max; //�ִ� �� ����( min , max)
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
