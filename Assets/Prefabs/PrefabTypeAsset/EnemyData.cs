using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    None,
    SandBag,
    Common,
    Elite,
    MidBoss,
    Boss
}

[CreateAssetMenu(fileName = "New EnemyData", menuName = "DataAsset/EnemyData")]
public class EnemyData : ScriptableObject
{
    public GameObject prefab;
    public EnemyType enemyType = EnemyType.None;
}
