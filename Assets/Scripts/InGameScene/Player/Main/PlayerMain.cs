using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour //플레이어의 메인 스크립트
{
    public static PlayerMain Instance;

    public static PlayerStat pStat;
    public static PlayerControl pControl;
    public static PlayerUI pUI;
    public static playerShooterUpgrade pShooter;
    public static PlayerSpecialSkill pSpecial;
    public static PlayerSkillManager pSkill;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }

        pStat = GetComponent<PlayerStat>();
        pControl = GetComponent<PlayerControl>();
        pUI = GetComponent<PlayerUI>();
        pSpecial = GetComponent<PlayerSpecialSkill>();
        pSkill= GetComponent<PlayerSkillManager>();
    }

    public void playerInit()
    {

    }

}
