using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    Active,
    Passive,
    Other
}

[CreateAssetMenu(fileName = "New SkillData", menuName = "DataAsset/SkillData")]
public class SkillData : ScriptableObject
{
    public int skillID;
    public SkillType skilltype;
    public Sprite skillIcon;
}
