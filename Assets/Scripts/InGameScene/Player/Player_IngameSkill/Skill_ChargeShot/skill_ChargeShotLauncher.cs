using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skill_ChargeShotLauncher : Ingame_Skill
{
    [Header("차지샷 고유 스텟")]

    public bool is_penetrate;
    
    protected override void Awake()
    {
        base.Awake();
        DamageRate = 2f;
        coolTime = 5;
        timer = 0;
        proj_num = 1;
        is_penetrate = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (level != transform.GetComponent<skill_interface>().level)
        {
            level = transform.GetComponent<skill_interface>().level;
            LevelSet(level);
        }

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            for (int i = 0; i < proj_num; i++)
            {
                GameObject chargeShot = Instantiate(skill_proj, transform.position, Quaternion.identity);
                skill_ChargeShot chargeShot_Stat = chargeShot.GetComponent<skill_ChargeShot>();
                chargeShot_Stat.damageRate = DamageRate;
                chargeShot_Stat.is_penetrate = is_penetrate;
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
            is_penetrate = false;
            transform.GetComponent<skill_interface>().skill_intro =
                "<차지샷>\n데미지 계수 20% 증가";

        }
        else if (level == 2)
        {
            DamageRate = 2.2f;
            coolTime = 5;
            timer = coolTime;
            is_penetrate = false;
            transform.GetComponent<skill_interface>().skill_intro =
                "<차지샷>\n쿨타임 1초 감소";


        }
        else if (level == 3)
        {
            DamageRate = 2.2f;
            coolTime = 4;
            timer = coolTime;
            is_penetrate = false;
            transform.GetComponent<skill_interface>().skill_intro =
                "<차지샷>\n데미지 계수 30% 증가";

        }
        else if (level == 4)
        {
            DamageRate = 2.5f;
            coolTime = 4;
            timer = coolTime;
            is_penetrate = false;
            transform.GetComponent<skill_interface>().skill_intro =
                "<차지샷>\n쿨타임 1초 감소";
        }
        else if (level == 5)
        {
            DamageRate = 2.5f;
            coolTime = 3;
            timer = coolTime;
            is_penetrate = false;
            transform.GetComponent<skill_interface>().skill_intro =
                "<차지샷>\n데미지 계수 50% 증가";
        }
        else if (level == 6)
        {
            DamageRate = 3f;
            coolTime = 3;
            timer = coolTime;
            is_penetrate = false;
            transform.GetComponent<skill_interface>().skill_intro =
                "<차지샷>\n차징샷이 적을 관통합니다.";

        }
        else if (level == 7)
        {
            DamageRate = 3f;
            coolTime = 3;
            timer = coolTime;
            is_penetrate = true;

        }
        else
        {
            Debug.Log("Already Max or Min");
        }
    }
}
