using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Skill_MissileLauncher : Ingame_Active
{
    [Header("미사일런처 고유 스텟")]
    public float explosionRange;
    public bool isLevelUp;

    protected override void Awake()
    {
        base.Awake();
        DamageRate = 1.5f;
        coolTime = 3;
        timer = coolTime;
        projNum = 1;

        explosionRange = 1.0f;
        isLevelUp = false;

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
        if(timer <= 0)
        {
            FindEnemyWithTag();
            if(enemies.Length == 0)
            {
                return;
            }
            for (int i = 0; i < projNum; i++)
            {
                Vector3 randx_pos = new Vector3(Random.Range(-0.1f, 0.1f), 0, 0);
                GameObject target = enemies[Random.Range(0, enemies.Length)];
                Vector3 direction = (target.transform.position - transform.position+ randx_pos).normalized;
                GameObject missile = Instantiate(skillProj, transform.position+ randx_pos, Quaternion.identity);
                missile.GetComponent<skill_Missile>().explosionRange = explosionRange;
                missile.GetComponent<skill_Missile>().missileDamageRate = DamageRate;
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
            projNum = 1;
            explosionRange = 1.0f;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<미사일런쳐>\n미사일 수 1개 증가";
        }
        else if (level == 2)
        {
            DamageRate = 1.5f;
            coolTime = 3;
            projNum = 2;
            explosionRange = 1.0f;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<미사일런쳐>\n데미지 계수 20% 증가";
        }
        else if (level == 3)
        {
            DamageRate = 1.7f;
            coolTime = 3;
            projNum = 2;
            explosionRange = 1.0f;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<미사일런쳐>\n쿨타임 1초 감소";
        }
        else if (level == 4)
        {
            DamageRate = 1.7f;
            coolTime = 2;
            projNum = 2;
            explosionRange = 1.0f;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<미사일런쳐>\n미사일 수 2개 증가";
        }
        else if (level == 5)
        {
            DamageRate = 1.7f;
            coolTime = 2;
            projNum = 4;
            explosionRange = 1.0f;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<미사일런쳐>\n쿨타임 1초 감소\n데미지 계수 30% 증가";
        }
        else if (level == 6)
        {
            DamageRate = 2f;
            coolTime = 1;
            projNum = 4;
            explosionRange = 1.0f;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<미사일런쳐>\n폭발범위 50% 증가\n데미지 계수 50%증가\n미사일 수 1개 증가 ";
        }
        else if (level == 7)
        {
            DamageRate = 2.5f;
            coolTime = 1;
            projNum = 5;
            explosionRange = 1.5f;
        }
        else
        {
            Debug.Log("Already Max or Min");
        }
    }
}
