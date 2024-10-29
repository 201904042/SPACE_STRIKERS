using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MasterData
{
    public int id;
    public string name;
    public MasterType type;
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
    public int minRank;
    public int basicValue;
    public AbilityRate SRange;
    public AbilityRate ARange;
    public AbilityRate BRange;
    public AbilityRate CRange;
    public AbilityRate DRange;
}

[System.Serializable]
public class PartsAbilityData
{
    public int invenId;
    public bool isActive;
    public int level;
    public int rank;
    public Ability mainAbility;
    public List<Ability> subAbilities;
}

[System.Serializable]
public class AccountData
{
    public int id;
    public string name;
    public int level;
    public int exp;
    public List<int> needExp;
    public int stageProgress;
    public int useChar;
    public int[] useParts;
    public int planetIndex;
    public int stageIndex;
}

[System.Serializable]
public class CharData
{
    public int id;
    public string name;
    public int level;
    public List<Ability> abilityDatas;
}

[System.Serializable]
public class InvenData
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


[System.Serializable]
public class UpgradeData
{
    public int masterId;
    public UpgradeCost[] upgradeCost;
}

[System.Serializable]
public class EnemyData
{
    public int id;
    public int type; //���� ��� common,elite,midBoss, Boss, Obstacle
    public float hp;
    public float damage;
    public float moveSpeed;
    public float attackSpeed;
    public float expAmount;
    public int socreAmount;
    public bool isStop; //false�� ���缭 ���� true�� �̵��ϸ鼭 ����
    public bool isAim; //true�� �����Ͽ� ��� false�� �׳� ������ ���� ���
}


[System.Serializable]
public class GotchaData
{
    public int id;
    public GotchaInform[] items;
    public GotchaCost[] cost;
}

[System.Serializable]
public class SkillData
{
    public int id;
    public SkillType type;
    public List<Skill_LevelValue> levels;
}
