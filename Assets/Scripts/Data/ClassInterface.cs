//플레이어 관련
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
    public Player[] player; //json파일의 분류의 이름과 동일한지 주의
}

//enemy 관련
[System.Serializable]
public class Enemy
{
    public int enemyId;
    public string enemyGrade; //적의 등급 common,elite,midBoss, Boss
    public string enemyName;
    public bool enemyMoveAttack; //false면 멈춰서 공격 true면 이동하면서 공격
    public bool isEnemyAiming; //true면 조준하여 사격 false면 그냥 앞으로 직선 사격
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
