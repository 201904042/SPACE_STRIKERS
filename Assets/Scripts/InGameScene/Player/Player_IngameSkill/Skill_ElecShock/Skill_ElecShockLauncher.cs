using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillElecShockLauncher : Ingame_Active
{
    [Header("일렉쇼크 고유 스텟")]
    public float shockRange;
    public float slowRate;
    public float slowTime;
    public bool isExtraDamageToSlowEnemy;
    protected override void Awake()
    {
        base.Awake();
       
        damageRate = 0.3f;
        coolTime = 5;
        projNum = 1;
        shockRange = 1.0f;
        slowRate = 0.3f;
        slowTime = 1f;
        isExtraDamageToSlowEnemy = false;
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

        PoolManager.poolInstance.GetSkill(SkillProjType.Skill_ElecShock, transform.position, Quaternion.identity).GetComponent<Skill_ElecShock>();
        yield return new WaitForSeconds(coolTime);
        activated = false;
    }


    protected override void LevelSet(int level)
    {
        if (level == 1)
        {
            damageRate = 0.3f;
            shockRange = 1.0f;
            slowRate = 0.3f;
            slowTime = 2f;
            isExtraDamageToSlowEnemy = false;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<일렉쇼크>\n둔화시간 0.5초 증가";
        }
        else if (level == 2)
        {
            damageRate = 0.3f;
            shockRange = 1.0f;
            slowRate = 0.3f;
            slowTime = 2.5f;
            isExtraDamageToSlowEnemy = false;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<일렉쇼크>\n둔화율 20% 증가";
        }
        else if (level == 3)
        {
            damageRate = 0.3f;
            shockRange = 1.0f;
            slowRate = 0.5f;
            slowTime = 2.5f;
            isExtraDamageToSlowEnemy = false;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<일렉쇼크>\n범위 50% 증가";
        }
        else if (level == 4)
        {
            damageRate = 0.3f;
            shockRange = 1.5f;
            slowRate = 0.5f;
            slowTime = 2.5f;
            isExtraDamageToSlowEnemy = false;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<일렉쇼크>\n둔화시간 0.5초 증가";
        }
        else if (level == 5)
        {
            damageRate = 0.3f;
            shockRange = 1.5f;
            slowRate = 0.5f;
            slowTime = 3f;
            isExtraDamageToSlowEnemy = false;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<일렉쇼크>\n둔화율 20% 증가";
        }
        else if (level == 6)
        {
            damageRate = 0.3f;
            shockRange = 1.5f;
            slowRate = 0.7f;
            slowTime = 3f;
            isExtraDamageToSlowEnemy = false;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<일렉쇼크>\n둔화된 적에게 데미지 100%증가\n범위 100% 증가";
        }
        else if (level == 7)
        {
            damageRate = 0.3f;
            shockRange = 2.5f;
            slowRate = 0.7f;
            slowTime = 3f;
            isExtraDamageToSlowEnemy = true;
        }
        else
        {
            Debug.Log("Already Max or Min");
        }

        isLevelUp = false;
    }
}
