using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjType
{
    None,
    Item_Exp,
    Item_ShooterUP,
    Item_LevelUp,
    Item_PowUp,
    Item_SpecialUp,
    Player_Bullet,
    Player_Missile,
    Player_SplashRange,
    Player_Homing,
    Enemy_Bullet,
    Enemy_Laser,
    Enemy_Split
}

[CreateAssetMenu(fileName = "New ProjData", menuName = "DataAsset/ProjData")]
public class ProjData : ScriptableObject
{
    public GameObject prefab;
    public ProjType projType = ProjType.None;
}
