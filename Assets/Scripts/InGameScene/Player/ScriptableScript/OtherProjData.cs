using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OtherProjType
{
    None,
    Item_Exp,
    Item_ShooterUP,
    Item_LevelUp,
    Item_PowUp,
    Item_SpecialUp,
    Enemy_Bullet,
    Enemy_Laser,
    Enemy_Split
}

[CreateAssetMenu(fileName = "New OtherProjData", menuName = "DataAsset/OtherProjData")]
public class OtherProjData : PoolData
{
    public OtherProjType projType = OtherProjType.None;
}
