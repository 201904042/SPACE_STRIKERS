
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
    public int enemyScoreAmount;
    public bool enemyMoveAttack; //false�� ���缭 ���� true�� �̵��ϸ鼭 ����
    public bool isEnemyAiming; //true�� �����Ͽ� ��� false�� �׳� ������ ���� ���
}

[System.Serializable]
public class EnemyList
{
    public Enemy[] enemy;
}

