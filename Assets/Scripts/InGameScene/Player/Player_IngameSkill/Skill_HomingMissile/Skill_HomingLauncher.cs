using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_HomingLauncher : Ingame_Skill
{
    [Header("호밍런쳐 고유 스텟")]
    public bool is_levelUp;

    protected override void Awake()
    {
        base.Awake();
        DamageRate = 0.8f;
        coolTime = 2;
        timer = coolTime;
        proj_num = 2;

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

        timer -= Time.deltaTime;
        if(timer <= 0 )
        {
            for(int i = 0; i<proj_num; i++)
            {
                Vector3 randx_pos = new Vector3(Random.Range(-0.1f, 0.1f), 0, 0);
                GameObject homing = Instantiate(skill_proj, transform.position + randx_pos, Quaternion.identity);
                homing.GetComponent<Skill_Homing>().homing_damageRate = DamageRate;
  
            }

            timer = coolTime;
        }
    }

    void LevelSet(int level)
    {
        if (level == 1)
        {
            DamageRate = 0.8f;
            coolTime = 2;
            proj_num = 2;
            transform.GetComponent<skill_interface>().skill_intro =
                "<호밍미사일>\n미사일 수 2개 증가";
        }
        else if (level == 2)
        {
            DamageRate = 0.8f;
            coolTime = 2;
            proj_num = 4;
            transform.GetComponent<skill_interface>().skill_intro =
                "<호밍미사일>\n쿨타임 0.5초 감소";
        }
        else if (level == 3)
        {
            DamageRate = 0.8f;
            coolTime = 1.5f;
            proj_num = 4;
            transform.GetComponent<skill_interface>().skill_intro =
                "<호밍미사일>\n미사일 수 2개 증가";
        }
        else if (level == 4)
        {
            DamageRate = 0.8f;
            coolTime = 1.5f;
            proj_num = 6;
            transform.GetComponent<skill_interface>().skill_intro =
                "<호밍미사일>\n쿨타임 0.5초 감소";
        }
        else if (level == 5)
        {
            DamageRate = 0.8f;
            coolTime = 1f;
            proj_num = 6;
            transform.GetComponent<skill_interface>().skill_intro =
                "<호밍미사일>\n미사일 수 2개 증가\n쿨타임 0.5초 감소";
        }
        else if (level == 6)
        {
            DamageRate = 0.8f;
            coolTime = 0.5f;
            proj_num = 8;
            transform.GetComponent<skill_interface>().skill_intro =
                "<호밍미사일>\n미사일 수 2배";
        }
        else if (level == 7)
        {
            DamageRate = 0.8f;
            coolTime = 0.5f;
            proj_num = 16;
            
        }
        else
        {
            Debug.Log("Already Max or Min");
        }
    }
}
