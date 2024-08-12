using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_EnergyFieldGenerator : Ingame_Active
{
    [Header("에너지필드 고유 스텟")]
    public float duration;
    public float range;
    public bool isShootable;
    private GameObject activeField;

    protected override void Awake()
    {
        base.Awake();
        
        coolTime = 10;
        isLevelUp = false;
    }

    protected override void OnEnable()
    {
        Init();
        LevelSet(level);
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
            activated = false;
            if (activeField != null)
            {
                PoolManager.poolInstance.ReleasePool(activeField.gameObject);
            }
            LevelSet(level);
        }

        if (!activated)
        {
            StartCoroutine(ActiveSkillInDelay(coolTime));
        }
    }

    private IEnumerator ActiveSkillInDelay(float coolTime)
    {
        Debug.Log("액티브");
        activated = true;

        Skill_EnergyField energyField = PoolManager.poolInstance.GetSkill(SkillProjType.Skill_EnergyField,
                transform.position, transform.rotation).GetComponent<Skill_EnergyField>();
        activeField = energyField.gameObject;

        yield return new WaitForSeconds(coolTime);

        activated = false;
    }

    protected override void LevelSet(int level)
    {
        switch (level)
        {
            case 1:
                damageRate = 0.2f;
                duration = 5;
                range = 8;
                isShootable = false;
                transform.GetComponent<SkillInterface>().skillIntro = "<에너지 필드>\n데미지 20% 증가";
                break;
            case 2:
                damageRate = 0.4f;
                duration = 5;
                range = 8;
                isShootable = false;
                transform.GetComponent<SkillInterface>().skillIntro = "<에너지 필드>\n지속시간 2초 증가";
                break;
            case 3:
                damageRate = 0.4f;
                duration = 7;
                range = 8;
                isShootable = false;
                transform.GetComponent<SkillInterface>().skillIntro = "<에너지 필드>\n범위 30%증가";
                break;
            case 4:
                damageRate = 0.4f;
                duration = 7;
                range = 10;
                isShootable = false;
                transform.GetComponent<SkillInterface>().skillIntro = "<에너지 필드>\n데미지 30% 증가";
                break;
            case 5:
                damageRate = 0.7f;
                duration = 7;
                range = 10;
                isShootable = false;
                transform.GetComponent<SkillInterface>().skillIntro = "<에너지 필드>\n지속시간 2초 증가";
                break;
            case 6:
                damageRate = 0.7f;
                duration = 9;
                range = 12;
                isShootable = false;
                transform.GetComponent<SkillInterface>().skillIntro = "<에너지 필드>\n범위 30%증가\n지속시간이 종료된 에너지 필드를 전방으로 사출";
                break;
            case 7:
                damageRate = 0.7f;
                duration = 9;
                range = 12;
                isShootable = true;
                break;
            default:
                Debug.Log("Already Max or Min");
                break;
        }
        isLevelUp = false;
    }
}
