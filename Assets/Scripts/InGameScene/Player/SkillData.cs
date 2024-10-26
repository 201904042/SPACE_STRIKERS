using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    Active,
    Passive,
    Unique,
    Other
}

[CreateAssetMenu(fileName = "New SkillData", menuName = "DataAsset/SkillData")]
public class SkillData : ScriptableObject
{
    public int skillID;
    public SkillType skilltype;
}
