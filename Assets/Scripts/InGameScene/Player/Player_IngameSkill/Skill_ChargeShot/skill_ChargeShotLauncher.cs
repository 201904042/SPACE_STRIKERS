using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_ChargeShotLauncher : Ingame_Active
{
    [Header("������ ���� ����")]

    public bool isPenetrate;
    
    protected override void Awake()
    {
        base.Awake();
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
                "<������>\n������ ��� 20% ����";

        }
        else if (level == 2)
        {
            DamageRate = 2.2f;
            coolTime = 5;
            timer = coolTime;
            isPenetrate = false;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<������>\n��Ÿ�� 1�� ����";


        }
        else if (level == 3)
        {
            DamageRate = 2.2f;
            coolTime = 4;
            timer = coolTime;
            isPenetrate = false;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<������>\n������ ��� 30% ����";

        }
        else if (level == 4)
        {
            DamageRate = 2.5f;
            coolTime = 4;
            timer = coolTime;
            isPenetrate = false;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<������>\n��Ÿ�� 1�� ����";
        }
        else if (level == 5)
        {
            DamageRate = 2.5f;
            coolTime = 3;
            timer = coolTime;
            isPenetrate = false;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<������>\n������ ��� 50% ����";
        }
        else if (level == 6)
        {
            DamageRate = 3f;
            coolTime = 3;
            timer = coolTime;
            isPenetrate = false;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<������>\n��¡���� ���� �����մϴ�.";

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
