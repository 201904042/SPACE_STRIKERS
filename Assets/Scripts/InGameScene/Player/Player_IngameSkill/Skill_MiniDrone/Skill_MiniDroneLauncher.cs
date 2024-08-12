using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_MiniDroneLauncher : Ingame_Active
{
    List<GameObject> activeDroneList;
    public float shootSpeedRate;
    protected override void Awake()
    {
        base.Awake();
        activeDroneList = new List<GameObject>();
        
    }

    protected override void OnEnable()
    {
        Init();
        LevelSet(level);
    }

    private void OnDisable()
    {

    }

    protected override void Init()
    {
        base.Init();
        isLevelUp = false;
    }

    protected override void Update()
    {
        base.Update();
        if (isLevelUp)
        {
            LevelSet(level);
            isLevelUp = false;
        }

        if (!activated)
        {
            StartCoroutine(ActiveSkillInDelay(coolTime));
        }

    }

    private IEnumerator ActiveSkillInDelay(float coolTime)
    {
        activated = true;

        for (int i = 0; i < projNum; i++)
        {
            GameObject drone = PoolManager.poolInstance.GetSkill(SkillProjType.Skill_MiniDrone, transform.position, Quaternion.identity);
            skill_MiniDrone droneStat = drone.GetComponent<skill_MiniDrone>();
            droneStat.damageRate = damageRate;
            droneStat.shootSpeedRate = shootSpeedRate;
            activeDroneList.Add(drone);
        }

        yield return new WaitForSeconds(coolTime);

        activated = false;
    }

    protected override void LevelSet(int level)
    {
        base.LevelSet(level);
        for (int i = 0; i < activeDroneList.Count; i++)
        {
            PoolManager.poolInstance.ReleasePool(activeDroneList[i]);
        }

        switch (level)
        {
            case 1:
                damageRate = 0.5f;
                coolTime = 20;
                projNum = 1;
                shootSpeedRate = -0.2f;
                transform.GetComponent<SkillInterface>().skillIntro =
                    "<�̴ϵ��>\n������ 20% ����";
                break;
            case 2:
                damageRate = 0.7f;
                coolTime = 20;
                projNum = 1;
                shootSpeedRate = -0.2f;
                transform.GetComponent<SkillInterface>().skillIntro =
                    "<�̴ϵ��>\n��� ���� �ӵ� 30% ����";
                break;
            case 3:
                damageRate = 0.7f;
                coolTime = 30;
                projNum = 1;
                shootSpeedRate = 0.1f;
                transform.GetComponent<SkillInterface>().skillIntro =
                    "<�̴ϵ��>\n��� 1�� ����";
                break;
            case 4:
                damageRate = 0.7f;
                coolTime = 20;
                projNum = 2;
                shootSpeedRate = 0.1f;
                transform.GetComponent<SkillInterface>().skillIntro =
                    "<�̴ϵ��>\n������ 30% ����";
                break;
            case 5:
                damageRate = 1f;
                coolTime = 20;
                projNum = 2;
                shootSpeedRate = 0.1f;
                transform.GetComponent<SkillInterface>().skillIntro =
                    "<�̴ϵ��>\n��� ���� �ӵ� 30% ����";
                break;
            case 6:
                damageRate = 1f;
                coolTime = 20;
                projNum = 2;
                shootSpeedRate = 0.3f;
                transform.GetComponent<SkillInterface>().skillIntro =
                    "<�̴ϵ��>\n������ 50% ����\n��а��� 2�� ����\n��� ���� �ӵ� 20% ����";
                break;
            case 7:
                damageRate = 1.5f;
                coolTime = 20;
                projNum = 4;
                shootSpeedRate = 0.5f;
                break;
            default:
                Debug.Log("Already Max or Min");
                break;
        }
    }
}
