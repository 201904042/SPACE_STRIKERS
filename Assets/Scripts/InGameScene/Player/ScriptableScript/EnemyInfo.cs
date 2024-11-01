using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    None = 0,
    CommonType1 =1,
    CommonType2 =2,
    EliteType1 =3,
    EliteType2 =4,
    MidBoss =5,
    Boss = 6,
    SandBag = 7
}

[CreateAssetMenu(fileName = "New EnemyInfo", menuName = "DataAsset/EnemyInfo")]
public class EnemyInfo : PoolData
{
    public EnemyType enemyType = EnemyType.None;
}
