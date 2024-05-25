using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_ChargeShotLauncher : Ingame_Active
{
    [Header("차지샷 고유 스텟")]

    public bool isPenetrate;
    
    protected override void Awake()
    {
        base.Awake();
        skillProj = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player/Player_InGameSkill/Proj/skill_ChargeShot.prefab");
        DamageRate = 2f;
        coolTime = 5;
        timer = 0;
        projNum = 1;
        isPenetrate = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (level != transform.GetComponent<SkillInterface>().level)
        {
            level = transform.GetComponent<SkillInterface>().level;
            LevelSet(level);
        }

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            for (int i = 0; i < projNum; i++)
            {
                GameObject chargeShot = Instantiate(skillProj, transform.position, Quaternion.identity);
                Skill_ChargeShot chargeShot_Stat = chargeShot.GetComponent<Skill_ChargeShot>();
                chargeShot_Stat.damageRate = DamageRate;
                chargeShot_Stat.isPenetrate = isPenetrate;
            }

            timer = coolTime;
        }
    }

    void LevelSet(int level)
    {
        if (level == 1)
        {
            DamageRate = 2f;
            coolTime = 5;
            timer = coolTime;
            isPenetrate = false;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<차지샷>\n데미지 계수 20% 증가";

        }
        else if (level == 2)
        {
            DamageRate = 2.2f;
            coolTime = 5;
            timer = coolTime;
            isPenetrate = false;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<차지샷>\n쿨타임 1초 감소";


        }
        else if (level == 3)
        {
            DamageRate = 2.2f;
            coolTime = 4;
            timer = coolTime;
            isPenetrate = false;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<차지샷>\n데미지 계수 30% 증가";

        }
        else if (level == 4)
        {
            DamageRate = 2.5f;
            coolTime = 4;
            timer = coolTime;
            isPenetrate = false;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<차지샷>\n쿨타임 1초 감소";
        }
        else if (level == 5)
        {
            DamageRate = 2.5f;
            coolTime = 3;
            timer = coolTime;
            isPenetrate = false;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<차지샷>\n데미지 계수 50% 증가";
        }
        else if (level == 6)
        {
            DamageRate = 3f;
            coolTime = 3;
            timer = coolTime;
            isPenetrate = false;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<차지샷>\n차징샷이 적을 관통합니다.";

        }
        else if (level == 7)
        {
            DamageRate = 3f;
            coolTime = 3;
            timer = coolTime;
            isPenetrate = true;

        }
        else
        {
            Debug.Log("Already Max or Min");
        }
    }
}
