using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class Skill_HomingLauncher : Ingame_Active
{
    protected override void Awake()
    {
        base.Awake();
        
        isLevelUp = false;
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
        
    }

    protected override void Update()
    {
        base.Update();
        if (isLevelUp)
        { 
            LevelSet(level);
            isLevelUp = false;
        }

        if(!activated)
        {
            StartCoroutine(ActiveSkillInDelay(coolTime));
        }
        
    }

    private IEnumerator ActiveSkillInDelay(float coolTime)
    {
        activated = true;

        for (int i = 0; i < projNum; i++)
        {
            Vector3 randx_pos = new Vector3(Random.Range(-0.1f, 0.1f), 0, 0);
            GameObject homing = PoolManager.poolInstance.GetSkill(SkillProjType.Skill_Homing, transform.position + randx_pos, Quaternion.identity);
            homing.GetComponent<Skill_Homing>().homingDamageRate = damageRate;

        }

        yield return new WaitForSeconds(coolTime);

        activated = false;
    }

    protected override void LevelSet(int level)
    {
        base.LevelSet(level);
        switch (level)
        {
            case 1:
                damageRate = 0.8f;
                coolTime = 2;
                projNum = 2;
                transform.GetComponent<SkillInterface>().skillIntro =
                    "<호밍미사일>\n미사일 수 2개 증가";
                break;
            case 2:
                damageRate = 0.8f;
                coolTime = 2;
                projNum = 4;
                transform.GetComponent<SkillInterface>().skillIntro =
                    "<호밍미사일>\n쿨타임 0.5초 감소";
                break;
            case 3:
                damageRate = 0.8f;
                coolTime = 1.5f;
                projNum = 4;
                transform.GetComponent<SkillInterface>().skillIntro =
                    "<호밍미사일>\n미사일 수 2개 증가";
                break;
            case 4:
                damageRate = 0.8f;
                coolTime = 1.5f;
                projNum = 6;
                transform.GetComponent<SkillInterface>().skillIntro =
                    "<호밍미사일>\n쿨타임 0.5초 감소";
                break;
            case 5:
                damageRate = 0.8f;
                coolTime = 1f;
                projNum = 6;
                transform.GetComponent<SkillInterface>().skillIntro =
                    "<호밍미사일>\n미사일 수 2개 증가\n쿨타임 0.5초 감소";
                break;
            case 6:
                damageRate = 0.8f;
                coolTime = 0.5f;
                projNum = 8;
                transform.GetComponent<SkillInterface>().skillIntro =
                    "<호밍미사일>\n미사일 수 2배";
                break;
            case 7:
                damageRate = 0.8f;
                coolTime = 0.5f;
                projNum = 16;
                break;
            default:
                Debug.Log("Already Max or Min");
                break;
        }
    }

}
