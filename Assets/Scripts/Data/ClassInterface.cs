//�÷��̾� ����
//PlayerJsonReader
[System.Serializable]
public class Player
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
[System.Serializable]
public class Enemy
{
    public int enemyId;
    public string enemyGrade; //���� ��� common,elite,midBoss, Boss
    public string enemyName;
    public bool enemyMoveAttack; //false�� ���缭 ���� true�� �̵��ϸ鼭 ����
    public bool isEnemyAiming; //true�� �����Ͽ� ��� false�� �׳� ������ ���� ���
    public float enemyMaxHp;
    public float enemyDamage;
    public float enemyMoveSpeed;
    public float enemyAttackSpeed;
    public float enemyExpAmount;
    public float enemyScoreAmount;
}

[System.Serializable]
public class EnemyList
{
    public Enemy[] enemy;
}
