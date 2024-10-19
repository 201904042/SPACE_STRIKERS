using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InGameSkill : MonoBehaviour 
{
    public int SkillCode { get; protected set; }
    public float Cooldown { get; protected set; } // 액티브 스킬용
    
    public abstract void LevelUp();
    
}
