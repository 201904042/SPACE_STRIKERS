using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerProjType
{
    None,
    Skill_ChageShot,
    Skill_ElecShock,
    Skill_EnergyField,
    Skill_Homing,
    Skill_MiniDrone,
    Skilll_DroneBullet,
    Skill_Missile,
    Explosion,
    Skill_Shield,
    Player_Bullet,
    Player_Missile,
    Player_Homing,
    Player_Shield,
    Spcial_Player1,
    Spcial_Player2,
    Spcial_Player3,
    Spcial_Player4
}

[CreateAssetMenu(fileName = "New PlayerProjData", menuName = "DataAsset/PlayerProjData")]
public class PlayerProjData : ScriptableObject
{
    public GameObject prefab;
    public PlayerProjType projType = PlayerProjType.None;
}
