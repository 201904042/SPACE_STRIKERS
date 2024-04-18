using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_ShieldGenerator : Ingame_Skill
{
    [Header("쉴드 고유 스텟")]
    public bool is_levelUp;
    public bool is_penetrate;
    public bool is_shieldOn;
    protected override void Awake()
    {
        base.Awake();
        DamageRate = 1f;
        coolTime = 20;
        timer = 0;
        is_shieldOn = false;
        is_levelUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (level != transform.GetComponent<skill_interface>().level)
        {
            level = transform.GetComponent<skill_interface>().level;
            LevelSet(level);

        }

        if (!is_shieldOn)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                GameObject shield = Instantiate(skill_proj, transform);
                shield.GetComponent<Skill_Shield>();
                is_shieldOn= true;
                timer = coolTime;
            }
        }
        
    }

    void LevelSet(int level)
    {
        if (level == 1)
        {
            DamageRate = 1f;
            coolTime = 20;
            transform.GetComponent<skill_interface>().skill_intro =
                "<쉴드>\n재생성 3초 감소";
        }
        else if (level == 2)
        {
            DamageRate = 1f;
            coolTime = 17;
            transform.GetComponent<skill_interface>().skill_intro =
                "<쉴드>\n재생성 3초 감소";
        }
        else if (level == 3)
        {
            DamageRate = 1f;
            coolTime = 14;
            transform.GetComponent<skill_interface>().skill_intro =
                "<쉴드>\n충돌한 적에게 150% 데미지";
        }
        else if (level == 4)
        {
            DamageRate = 1.5f;
            coolTime = 14;
            transform.GetComponent<skill_interface>().skill_intro =
                "<쉴드>\n재생성 3초 감소";
        }
        else if (level == 5)
        {
            DamageRate = 1.5f;
            coolTime = 11;
            transform.GetComponent<skill_interface>().skill_intro =
                "<쉴드>\n재생성 3초 감소";
        }
        else if (level == 6)
        {
            DamageRate = 1.5f;
            coolTime = 8;
            transform.GetComponent<skill_interface>().skill_intro =
                "<쉴드>\n충돌한 적에게 300% 데미지";
        }
        else if (level == 7)
        {
            DamageRate = 3f;
            coolTime = 5;
        }
        else
        {
            Debug.Log("Already Max or Min");
        }
        timer = coolTime;
    }
}
