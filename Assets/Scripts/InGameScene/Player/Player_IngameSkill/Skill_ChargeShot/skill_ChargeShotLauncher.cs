using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_ChargeShotLauncher : Ingame_Active
{
    [Header("차지샷 고유 스텟")]

    public bool isPenetrate;
    
    protected override void Awake()
    {
        base.Awake();
       
        //시작 변수 세팅
        damageRate = 2f;
        coolTime = 5;
        projNum = 1;
        isPenetrate = false;
    }

    protected override void OnEnable()
    {
        Init();
    }

    protected override void Init()
    {
        base.Init();
        level = transform.GetComponent<SkillInterface>().level;
    }

    protected override void Update()
    {
        base.Update();
        if (isLevelUp)
        {
            LevelSet(level);
        }

        if (!activated && SpawnManager.spawnInstance.activeEnemyList.Count != 0)
        {
            StartCoroutine(ActiveSkillInDelay(coolTime));
        }
    }

    private IEnumerator ActiveSkillInDelay(float coolTime)
    {
        activated = true;
        for (int i = 0; i < projNum; i++)
        {
            ObjectPool.poolInstance.GetSkill(SkillProjType.Skill_ChageShot, 
                transform.position, Quaternion.identity);
        }
        yield return new WaitForSeconds(coolTime);
        activated = false;
    }

    protected override void LevelSet(int level)
    {
        switch (level)
        {
            case 1:
                damageRate = 2f;
                coolTime = 5;
                isPenetrate = false;
                transform.GetComponent<SkillInterface>().skillIntro =
                    "<차지샷>\n데미지 계수 20% 증가";
                break;

            case 2:
                damageRate = 2.2f;
                coolTime = 5;
                isPenetrate = false;
                transform.GetComponent<SkillInterface>().skillIntro =
                    "<차지샷>\n쿨타임 1초 감소";
                break;

            case 3:
                damageRate = 2.2f;
                coolTime = 4;
                isPenetrate = false;
                transform.GetComponent<SkillInterface>().skillIntro =
                    "<차지샷>\n데미지 계수 30% 증가";
                break;

            case 4:
                damageRate = 2.5f;
                coolTime = 4;
                isPenetrate = false;
                transform.GetComponent<SkillInterface>().skillIntro =
                    "<차지샷>\n쿨타임 1초 감소";
                break;

            case 5:
                damageRate = 2.5f;
                coolTime = 3;
                isPenetrate = false;
                transform.GetComponent<SkillInterface>().skillIntro =
                    "<차지샷>\n데미지 계수 50% 증가";
                break;

            case 6:
                damageRate = 3f;
                coolTime = 3;
                isPenetrate = false;
                transform.GetComponent<SkillInterface>().skillIntro =
                    "<차지샷>\n차징샷이 적을 관통";
                break;

            case 7:
                damageRate = 3f;
                coolTime = 3;
                isPenetrate = true;
                transform.GetComponent<SkillInterface>().skillIntro = "<차지샷>";
                break;

            default:
                Debug.Log("Already Max or Min");
                break;
        }
        isLevelUp = false;
    }
}
