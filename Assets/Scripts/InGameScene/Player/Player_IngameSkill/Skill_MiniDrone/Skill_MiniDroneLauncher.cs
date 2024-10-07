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
            GameObject drone = Managers.Instance.Pool.GetSkill(SkillProjType.Skill_MiniDrone, transform.position, Quaternion.identity);
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
            Managers.Instance.Pool.ReleasePool(activeDroneList[i]);
        }

        switch (level)
        {
            case 1:
                damageRate = 0.5f;
                coolTime = 20;
                projNum = 1;
                shootSpeedRate = -0.2f;
                transform.GetComponent<SkillInterface>().skillIntro =
                    "<미니드론>\n데미지 20% 증가";
                break;
            case 2:
                damageRate = 0.7f;
                coolTime = 20;
                projNum = 1;
                shootSpeedRate = -0.2f;
                transform.GetComponent<SkillInterface>().skillIntro =
                    "<미니드론>\n드론 공격 속도 30% 증가";
                break;
            case 3:
                damageRate = 0.7f;
                coolTime = 30;
                projNum = 1;
                shootSpeedRate = 0.1f;
                transform.GetComponent<SkillInterface>().skillIntro =
                    "<미니드론>\n드론 1개 증가";
                break;
            case 4:
                damageRate = 0.7f;
                coolTime = 20;
                projNum = 2;
                shootSpeedRate = 0.1f;
                transform.GetComponent<SkillInterface>().skillIntro =
                    "<미니드론>\n데미지 30% 증가";
                break;
            case 5:
                damageRate = 1f;
                coolTime = 20;
                projNum = 2;
                shootSpeedRate = 0.1f;
                transform.GetComponent<SkillInterface>().skillIntro =
                    "<미니드론>\n드론 공격 속도 30% 증가";
                break;
            case 6:
                damageRate = 1f;
                coolTime = 20;
                projNum = 2;
                shootSpeedRate = 0.3f;
                transform.GetComponent<SkillInterface>().skillIntro =
                    "<미니드론>\n데미지 50% 증가\n드론개수 2개 증가\n드론 공격 속도 20% 증가";
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
