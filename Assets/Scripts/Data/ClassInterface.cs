
//enemy 관련
[System.Serializable] //EnemyObject
public class Enemy
{
    public int enemyId;
    public string enemyGrade; //적의 등급 common,elite,midBoss, Boss, Obstacle
    public string enemyName;
    public float enemyMaxHp;
    public float enemyDamage;
    public float enemyMoveSpeed;
    public float enemyAttackSpeed;
    public float enemyExpAmount;
    public int enemyScoreAmount;
    public bool enemyMoveAttack; //false면 멈춰서 공격 true면 이동하면서 공격
    public bool isEnemyAiming; //true면 조준하여 사격 false면 그냥 앞으로 직선 사격
}

[System.Serializable]
public class EnemyList
{
    public Enemy[] enemy;
}

