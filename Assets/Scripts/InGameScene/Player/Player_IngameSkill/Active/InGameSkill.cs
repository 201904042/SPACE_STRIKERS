using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//공용스킬 , 스페셜 스킬
public abstract class InGameSkill
{
    public int SkillCode { get; protected set; }
    public SkillType type { get; protected set; }
    public List<Skill_LevelValue> SkillLevels { get; protected set; }
    public int currentLevel;
    public string description;
    public abstract void Init();
    public abstract void LevelUp();
    public abstract void SetLevel();
}
