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

[CreateAssetMenu(fileName = "New EnemyInfo", menuName = "DataAsset/EnemyInfo")]
public class EnemyInfo : ScriptableObject
{
    public GameObject prefab;
    public EnemyType enemyType = EnemyType.None;
}
