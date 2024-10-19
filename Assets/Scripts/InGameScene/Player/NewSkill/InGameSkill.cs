using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InGameSkill : MonoBehaviour 
{
    public int SkillCode { get; protected set; }
    public float Cooldown { get; protected set; } // ��Ƽ�� ��ų��
    
    public abstract void LevelUp();
    
}
