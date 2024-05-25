using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_MiniDroneLauncher : Ingame_Active
{
    [Header("�̴ϵ�� ���� ����")]
    public bool isLevelUp;


    public float shootSpeedRate;
    protected override void Awake()
    {
        base.Awake();
        skillProj = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player/Player_InGameSkill/Proj/skill_miniDrone.prefab");
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
                "<�̴ϵ��>\n������ 20% ����";

        }
        else if (level == 2)
        {
            DamageRate = 0.7f;
            coolTime = 20;
            timer = 0;
            projNum = 1;
            shootSpeedRate = -0.2f;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<�̴ϵ��>\n��� ���� �ӵ� 30% ����";

        }
        else if (level == 3)
        {
            DamageRate = 0.7f;
            coolTime = 30;
            timer = coolTime;
            projNum = 1;
            shootSpeedRate = 0.1f;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<�̴ϵ��>\n��� 1�� ����";
        }
        else if (level == 4)
        {
            DamageRate = 0.7f;
            coolTime = 20;
            timer = 0;
            projNum = 2;
            shootSpeedRate = 0.1f;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<�̴ϵ��>\n������ 30% ����";
        }
        else if (level == 5)
        {
            DamageRate = 1f;
            coolTime = 20;
            timer = 0;
            projNum = 2;
            shootSpeedRate = 0.1f;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<�̴ϵ��>\n��� ���� �ӵ� 30% ����";
        }
        else if (level == 6)
        {
            DamageRate = 1;
            coolTime = 20;
            timer = 0;
            projNum = 2;
            shootSpeedRate = 0.3f;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<�̴ϵ��>\n������ 50% ����\n��а��� 2�� ����\n��� ���� �ӵ� 20% ����";
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
