using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_MiniDroneLauncher : Ingame_Active
{
    [Header("미니드론 고유 스텟")]
    public bool isLevelUp;


    public float shootSpeedRate;
    protected override void Awake()
    {
        base.Awake();
        DamageRate = 0.5f;
        coolTime = 20;
        timer = 0;
        projNum = 1;
        shootSpeedRate = -0.2f;

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
        if (timer <= 0)
        {
            for (int i = 0; i < projNum; i++)
            {
                GameObject drone = Instantiate(skillProj, transform.position, Quaternion.identity);
                skill_MiniDrone dronestat = drone.GetComponent<skill_MiniDrone>();
                dronestat.damageRate = DamageRate;
                dronestat.shootSpeedRate = shootSpeedRate;
            }

            timer = coolTime;
        }
    }

    void LevelSet(int level)
    {
        GameObject[] Drone = GameObject.FindGameObjectsWithTag("Player_Drone");
        for(int i = 0; i < Drone.Length;i++)
        {
            Destroy(Drone[i]);
        }
        
        if (level == 1)
        {
            DamageRate = 0.5f;
            coolTime = 20;
            timer = 0;
            projNum = 1;
            shootSpeedRate = -0.2f;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<미니드론>\n데미지 20% 증가";

        }
        else if (level == 2)
        {
            DamageRate = 0.7f;
            coolTime = 20;
            timer = 0;
            projNum = 1;
            shootSpeedRate = -0.2f;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<미니드론>\n드론 공격 속도 30% 증가";

        }
        else if (level == 3)
        {
            DamageRate = 0.7f;
            coolTime = 30;
            timer = coolTime;
            projNum = 1;
            shootSpeedRate = 0.1f;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<미니드론>\n드론 1개 증가";
        }
        else if (level == 4)
        {
            DamageRate = 0.7f;
            coolTime = 20;
            timer = 0;
            projNum = 2;
            shootSpeedRate = 0.1f;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<미니드론>\n데미지 30% 증가";
        }
        else if (level == 5)
        {
            DamageRate = 1f;
            coolTime = 20;
            timer = 0;
            projNum = 2;
            shootSpeedRate = 0.1f;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<미니드론>\n드론 공격 속도 30% 증가";
        }
        else if (level == 6)
        {
            DamageRate = 1;
            coolTime = 20;
            timer = 0;
            projNum = 2;
            shootSpeedRate = 0.3f;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<미니드론>\n데미지 50% 증가\n드론개수 2개 증가\n드론 공격 속도 20% 증가";
        }
        else if (level == 7)
        {
            DamageRate = 1.5f;
            coolTime = 20;
            timer = 0;
            projNum = 4;
            shootSpeedRate = 0.5f;

        }
        else
        {
            Debug.Log("Already Max or Min");
        }
    }
}
