using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skill_interface : MonoBehaviour
{
    public int level;
    public Sprite icon;
    public string skill_intro;

    private void Awake()
    {
        level = 1;
    }
    
}
