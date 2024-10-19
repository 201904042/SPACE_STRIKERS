using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InGameSkill
{
    public int SkillCode { get; protected set; }
    public abstract void Init();
    public abstract void LevelUp();
    public abstract void SetLevel();
}
