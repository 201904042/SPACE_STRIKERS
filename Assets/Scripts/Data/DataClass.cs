using System;

[System.Serializable]
public class Ability
{
    public int key; //�����Ƽ �������� Ű
    public float value; //���� ���귮
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
