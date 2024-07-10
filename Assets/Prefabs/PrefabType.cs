using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ProjPoolType
{
    None,
    Player_Bullet,
    Player_Missile,
    Player_SplashRange,
    Player_Homing,
    Skill_ChageShot,
    Skill_ElecShock,
    Skill_EnergyField,
    Skill_Homing,
    Skill_MiniDrone,
    Skilll_DroneBullet,
    Skill_Missile,
    Skill_Splash,
    Skill_Shield,
    Enemy_Bullet,
    Enemy_Laser,
    Enemy_Split,
    Object_Exp
}

public enum EnemyPoolType
{
    None,
    SandBag,
    Common,
    Elite,
    MidBoss,
    Boss
}


[CreateAssetMenu(fileName ="new priefabType", menuName = "PrefabType")]
public class PrefabTypeAsset : ScriptableObject
{
    public GameObject prefab;
    public ProjPoolType projPoolType = ProjPoolType.None;
    public EnemyPoolType enemyPoolType = EnemyPoolType.None;
}
