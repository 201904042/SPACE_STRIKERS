using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInterface : MonoBehaviour
{
    public int level;
    public Sprite icon;
    public string skillIntro;

    private void Awake()
    {
        level = 1;
    }
    
}
