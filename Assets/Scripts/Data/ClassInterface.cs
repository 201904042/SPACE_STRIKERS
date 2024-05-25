//�÷��̾� ����
//PlayerJsonReader
[System.Serializable]
public class Player //Player
{
    public int id;
    public string name;
    public int level;
    public float damage;
    public float defence;
    public float move_speed;
    public float attack_speed;
    public float hp;
}

[System.Serializable]
public class PlayerList
{
    public Player[] player; //json������ �з��� �̸��� �������� ����
}

//enemy ����
[System.Serializable] //EnemyObject
public class Enemy
{
    public int enemyId;
    public string enemyGrade; //���� ��� common,elite,midBoss, Boss, Obstacle
    public string enemyName;
    public float enemyMaxHp;
    public float enemyDamage;
    public float enemyMoveSpeed;
    public float enemyAttackSpeed;
    public float enemyExpAmount;
    public float enemyScoreAmount;
    public bool enemyMoveAttack; //false�� ���缭 ���� true�� �̵��ϸ鼭 ����
    public bool isEnemyAiming; //true�� �����Ͽ� ��� false�� �׳� ������ ���� ���
}

[System.Serializable]
public class EnemyList
{
    public Enemy[] enemy;
}

//�κ� ����
[System.Serializable] //GameManager
public class invenAccountData
{
    public PlayerAccount[] Account;
    public Parts[] parts;
    public Ingredients[] ingredients;
    public Consumables[] consumables;
}


[System.Serializable]
public class PlayerAccount
{
    public int accountCode;
    public string accountName;
    public int accountLevel;
    public int mineral;
    public int ruby;
    public bool is_player2Open;
    public bool is_player3Open;
    public bool is_player4Open;
    public int clearedPlanet1Stage;
    public int clearedPlanet2Stage;
    public int clearedPlanet3Stage;
    public int clearedPlanet4Stage;
}


[System.Serializable] //AccountData
public class Parts
{
    public int PartsId;
    public int PartsCode;
    public string PartsName;
    public string PartsType;
    public int PartsLevel;
    public string PartsRank;
    public int mainAmount;
    public string Partsability1;
    public int abilityAmount1;
    public string Partsability2;
    public int abilityAmount2;
    public string Partsability3;
    public int abilityAmount3;
    public string Partsability4;
    public int abilityAmount4;
    public string Partsability5;
    public int abilityAmount5;
}
[System.Serializable]
public class Ingredients
{
    public int ingredId;
    public int ingredCode;
    public string ingredName;
    public int ingredAmount;
}
[System.Serializable]
public class Consumables
{
    public int consId;
    public int consCode;
    public string consName;
    public int consAmount;
}

[System.Serializable]
public class PlayerPartsList
{
    public Parts[] parts;
}
[System.Serializable]
public class PlayerIngredList
{
    public Ingredients[] ingredients;
}
[System.Serializable]
public class PlayerConsList
{
    public Consumables[] consumables;
}
[System.Serializable]
public class PlayerAccountList
{
    public PlayerAccount[] Account;
}