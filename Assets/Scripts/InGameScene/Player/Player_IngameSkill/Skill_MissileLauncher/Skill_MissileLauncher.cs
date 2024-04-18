using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Skill_MissileLauncher : Ingame_Skill
{
    [Header("미사일런처 고유 스텟")]
    public float Explosion_Range;
    public bool is_levelUp;

    protected override void Awake()
    {
        base.Awake();
        DamageRate = 1.5f;
        coolTime = 3;
        timer = coolTime;
        proj_num = 1;

        Explosion_Range = 1.0f;
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
        if(timer <= 0)
        {
            findEnemyWithTag();
            if(enemies.Length == 0)
            {
                return;
            }
            for (int i = 0; i < proj_num; i++)
            {
                Vector3 randx_pos = new Vector3(Random.Range(-0.1f, 0.1f), 0, 0);
                GameObject target = enemies[Random.Range(0, enemies.Length)];
                Vector3 direction = (target.transform.position - transform.position+ randx_pos).normalized;
                GameObject missile = Instantiate(skill_proj, transform.position+ randx_pos, Quaternion.identity);
                missile.GetComponent<skill_Missile>().explosion_range = Explosion_Range;
                missile.GetComponent<skill_Missile>().missile_damageRate = DamageRate;
                missile.transform.up = direction;

            }
            timer = coolTime;
        }
    }

    void LevelSet(int level)
    {
        if (level == 1)
        {
            DamageRate = 1.5f;
            coolTime = 3;
            proj_num = 1;
            Explosion_Range = 1.0f;
            transform.GetComponent<skill_interface>().skill_intro =
                "<미사일런쳐>\n미사일 수 1개 증가";
        }
        else if (level == 2)
        {
            DamageRate = 1.5f;
            coolTime = 3;
            proj_num = 2;
            Explosion_Range = 1.0f;
            transform.GetComponent<skill_interface>().skill_intro =
                "<미사일런쳐>\n데미지 계수 20% 증가";
        }
        else if (level == 3)
        {
            DamageRate = 1.7f;
            coolTime = 3;
            proj_num = 2;
            Explosion_Range = 1.0f;
            transform.GetComponent<skill_interface>().skill_intro =
                "<미사일런쳐>\n쿨타임 1초 감소";
        }
        else if (level == 4)
        {
            DamageRate = 1.7f;
            coolTime = 2;
            proj_num = 2;
            Explosion_Range = 1.0f;
            transform.GetComponent<skill_interface>().skill_intro =
                "<미사일런쳐>\n미사일 수 2개 증가";
        }
        else if (level == 5)
        {
            DamageRate = 1.7f;
            coolTime = 2;
            proj_num = 4;
            Explosion_Range = 1.0f;
            transform.GetComponent<skill_interface>().skill_intro =
                "<미사일런쳐>\n쿨타임 1초 감소\n데미지 계수 30% 증가";
        }
        else if (level == 6)
        {
            DamageRate = 2f;
            coolTime = 1;
            proj_num = 4;
            Explosion_Range = 1.0f;
            transform.GetComponent<skill_interface>().skill_intro =
                "<미사일런쳐>\n폭발범위 50% 증가\n데미지 계수 50%증가\n미사일 수 1개 증가 ";
        }
        else if (level == 7)
        {
            DamageRate = 2.5f;
            coolTime = 1;
            proj_num = 5;
            Explosion_Range = 1.5f;
        }
        else
        {
            Debug.Log("Already Max or Min");
        }
    }
}
