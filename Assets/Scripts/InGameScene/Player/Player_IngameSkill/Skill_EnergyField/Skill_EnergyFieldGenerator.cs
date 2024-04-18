using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_EnergyFieldGenerator : Ingame_Skill
{
    [Header("�������ʵ� ���� ����")]
    public bool is_levelUp;
    private float duration;
    private float range;
    private bool is_shootable;
    private GameObject field;

    protected override void Awake()
    {
        base.Awake();
        DamageRate = 0.2f;
        coolTime = 10;
        timer = 0;
        duration = 5;
        range = 8;
        is_shootable = false;

        is_levelUp = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (level != transform.GetComponent<skill_interface>().level)
        {
            level = transform.GetComponent<skill_interface>().level;
            
            if (field != null)
            {
                Destroy(field.gameObject);
            }
            LevelSet(level);
        }
        
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            field = Instantiate(skill_proj, transform);
            Skill_EnergyField fieldstat = field.GetComponent<Skill_EnergyField>();
            fieldstat.e_damageRate = DamageRate;
            fieldstat.e_duration = duration;
            fieldstat.e_is_shootable = is_shootable;
            field.transform.localScale = field.transform.localScale * range / 8;
            timer = coolTime;
        }
    }

    void LevelSet(int level)
    {
        if (level == 1)
        {
            DamageRate = 0.2f;
            duration = 5;
            range = 8;
            is_shootable = false;
            transform.GetComponent<skill_interface>().skill_intro =
                "<������ �ʵ�>\n������ 20% ����";
        }
        else if (level == 2)
        {
            DamageRate = 0.4f;
            duration = 5;
            range = 8;
            is_shootable = false;
            transform.GetComponent<skill_interface>().skill_intro =
                "<������ �ʵ�>\n���ӽð� 2�� ����";
        }
        else if (level == 3)
        {
            DamageRate = 0.4f;
            duration = 7;
            range = 8;
            is_shootable = false;
            transform.GetComponent<skill_interface>().skill_intro =
                "<������ �ʵ�>\n���� 30%����";
        }
        else if (level == 4)
        {
            DamageRate = 0.4f;
            duration = 7;
            range = 10;
            is_shootable = false;
            transform.GetComponent<skill_interface>().skill_intro =
                "<������ �ʵ�>\n������ 30% ����";

        }
        else if (level == 5)
        {
            DamageRate = 0.7f;
            duration = 7;
            range = 10;
            is_shootable = false;
            transform.GetComponent<skill_interface>().skill_intro =
                "<������ �ʵ�>\n���ӽð� 2�� ����";

        }
        else if (level == 6)
        {
            DamageRate = 0.7f;
            duration = 9;
            range = 10;
            is_shootable = false;
            transform.GetComponent<skill_interface>().skill_intro =
                "<������ �ʵ�>\n���� 30%����\n���ӽð��� ����� ������ �ʵ带 �������� ����";

        }
        else if (level == 7)
        {
            DamageRate = 0.7f;
            duration = 9;
            range = 12;
            is_shootable = true;
        }
        else
        {
            Debug.Log("Already Max or Min");
        }
        timer = 0;
    }
}
