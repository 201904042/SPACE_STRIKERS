using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillProjType
{
    None,
    Skill_ChageShot,
    Skill_ElecShock,
    Skill_EnergyField,
    Skill_Homing,
    Skill_MiniDrone,
    Skilll_DroneBullet,
    Skill_Missile,
    Skill_Splash,
    Skill_Shield,
    Spcial_Player1,
    Spcial_Player2,
    Spcial_Player3,
    Spcial_Player4
}

[CreateAssetMenu(fileName = "New SkillProjData", menuName = "DataAsset/SkillProjData")]
public class SkillProjData : ScriptableObject
{
    public GameObject prefab;
    public SkillProjType skillType = SkillProjType.None;
}
