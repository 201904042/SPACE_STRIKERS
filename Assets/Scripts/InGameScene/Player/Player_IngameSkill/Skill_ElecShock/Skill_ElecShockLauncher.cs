using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_ElecShockLauncher : Ingame_Skill
{
    [Header("�Ϸ���ũ ���� ����")]
    public float shock_Range;
    public float slow_Rate;
    public float slow_Time;
    public bool is_ExtraDamageToSlowEnemyOn;
    protected override void Awake()
    {
        base.Awake();
        DamageRate = 0.3f;
        coolTime = 5;
        timer = coolTime;
        proj_num = 1;

        shock_Range = 1.0f;
        slow_Rate = 0.3f;
        slow_Time = 1f;
        is_ExtraDamageToSlowEnemyOn = false;
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

            Skill_ElecShock shockObj = Instantiate(skill_proj, transform.position, Quaternion.identity).GetComponent<Skill_ElecShock>();
            shockObj.shock_damageRate = DamageRate;
            shockObj.shock_range = shock_Range;
            shockObj.slowRate = slow_Rate;
            shockObj.slowTime = slow_Time;
            shockObj.is_ExtraDamageToSlowEnemyOn = is_ExtraDamageToSlowEnemyOn;

            timer = coolTime;
        }
    }

    void LevelSet(int level)
    {
        if (level == 1)
        {
            DamageRate = 0.3f;
            shock_Range = 1.0f;
            slow_Rate = 0.3f;
            slow_Time = 2f;
            is_ExtraDamageToSlowEnemyOn = false;
            transform.GetComponent<skill_interface>().skill_intro =
                "<�Ϸ���ũ>\n��ȭ�ð� 0.5�� ����";
        }
        else if (level == 2)
        {
            DamageRate = 0.3f;
            shock_Range = 1.0f;
            slow_Rate = 0.3f;
            slow_Time = 2.5f;
            is_ExtraDamageToSlowEnemyOn = false;
            transform.GetComponent<skill_interface>().skill_intro =
                "<�Ϸ���ũ>\n��ȭ�� 20% ����";
        }
        else if (level == 3)
        {
            DamageRate = 0.3f;
            shock_Range = 1.0f;
            slow_Rate = 0.5f;
            slow_Time = 2.5f;
            is_ExtraDamageToSlowEnemyOn = false;
            transform.GetComponent<skill_interface>().skill_intro =
                "<�Ϸ���ũ>\n���� 50% ����";
        }
        else if (level == 4)
        {
            DamageRate = 0.3f;
            shock_Range = 1.5f;
            slow_Rate = 0.5f;
            slow_Time = 2.5f;
            is_ExtraDamageToSlowEnemyOn = false;
            transform.GetComponent<skill_interface>().skill_intro =
                "<�Ϸ���ũ>\n��ȭ�ð� 0.5�� ����";
        }
        else if (level == 5)
        {
            DamageRate = 0.3f;
            shock_Range = 1.5f;
            slow_Rate = 0.5f;
            slow_Time = 3f;
            is_ExtraDamageToSlowEnemyOn = false;
            transform.GetComponent<skill_interface>().skill_intro =
                "<�Ϸ���ũ>\n��ȭ�� 20% ����";
        }
        else if (level == 6)
        {
            DamageRate = 0.3f;
            shock_Range = 1.5f;
            slow_Rate = 0.7f;
            slow_Time = 3f;
            is_ExtraDamageToSlowEnemyOn = false;
            transform.GetComponent<skill_interface>().skill_intro =
                "<�Ϸ���ũ>\n��ȭ�� ������ ������ 100%����\n���� 100% ����";
        }
        else if (level == 7)
        {
            DamageRate = 0.3f;
            shock_Range = 2.5f;
            slow_Rate = 0.7f;
            slow_Time = 3f;
            is_ExtraDamageToSlowEnemyOn = true;
        }
        else
        {
            Debug.Log("Already Max or Min");
        }
    }
}
