using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_MiniDroneLauncher : Ingame_Skill
{
    [Header("�̴ϵ�� ���� ����")]
    public bool is_levelUp;


    public float shootSpeedRate;
    protected override void Awake()
    {
        base.Awake();
        DamageRate = 0.5f;
        coolTime = 20;
        timer = 0;
        proj_num = 1;
        shootSpeedRate = -0.2f;

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
        if (timer <= 0)
        {
            for (int i = 0; i < proj_num; i++)
            {
                GameObject drone = Instantiate(skill_proj, transform.position, Quaternion.identity);
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
            proj_num = 1;
            shootSpeedRate = -0.2f;
            transform.GetComponent<skill_interface>().skill_intro =
                "<�̴ϵ��>\n������ 20% ����";

        }
        else if (level == 2)
        {
            DamageRate = 0.7f;
            coolTime = 20;
            timer = 0;
            proj_num = 1;
            shootSpeedRate = -0.2f;
            transform.GetComponent<skill_interface>().skill_intro =
                "<�̴ϵ��>\n��� ���� �ӵ� 30% ����";

        }
        else if (level == 3)
        {
            DamageRate = 0.7f;
            coolTime = 30;
            timer = coolTime;
            proj_num = 1;
            shootSpeedRate = 0.1f;
            transform.GetComponent<skill_interface>().skill_intro =
                "<�̴ϵ��>\n��� 1�� ����";
        }
        else if (level == 4)
        {
            DamageRate = 0.7f;
            coolTime = 20;
            timer = 0;
            proj_num = 2;
            shootSpeedRate = 0.1f;
            transform.GetComponent<skill_interface>().skill_intro =
                "<�̴ϵ��>\n������ 30% ����";
        }
        else if (level == 5)
        {
            DamageRate = 1f;
            coolTime = 20;
            timer = 0;
            proj_num = 2;
            shootSpeedRate = 0.1f;
            transform.GetComponent<skill_interface>().skill_intro =
                "<�̴ϵ��>\n��� ���� �ӵ� 30% ����";
        }
        else if (level == 6)
        {
            DamageRate = 1;
            coolTime = 20;
            timer = 0;
            proj_num = 2;
            shootSpeedRate = 0.3f;
            transform.GetComponent<skill_interface>().skill_intro =
                "<�̴ϵ��>\n������ 50% ����\n��а��� 2�� ����\n��� ���� �ӵ� 20% ����";
        }
        else if (level == 7)
        {
            DamageRate = 1.5f;
            coolTime = 20;
            timer = 0;
            proj_num = 4;
            shootSpeedRate = 0.5f;

        }
        else
        {
            Debug.Log("Already Max or Min");
        }
    }
}
