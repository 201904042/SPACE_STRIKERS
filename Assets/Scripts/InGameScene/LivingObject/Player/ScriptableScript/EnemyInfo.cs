using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    None = 0,
    Common = 1,
    Elite =2,
    MidBoss =3,
    Boss = 4,
    SandBag = 5
}

[CreateAssetMenu(fileName = "New EnemyInfo", menuName = "DataAsset/EnemyInfo")]
public class EnemyInfo : PoolData
{
    public EnemyType enemyType = EnemyType.None;
}
