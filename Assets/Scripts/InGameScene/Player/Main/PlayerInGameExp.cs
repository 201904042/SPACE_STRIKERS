using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInGameExp : MonoBehaviour
{
    

    [Header("expAmount ���")]
    public int InGameLv;
    public float maxExp;
    public float curExp;
    
    private void Awake()
    {
        InGameLv = 1;
        maxExp = 5f;
        curExp = 0;
    }

    
}
