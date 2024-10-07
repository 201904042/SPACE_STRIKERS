using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_EnergyFieldGenerator : Ingame_Active
{
    [Header("�������ʵ� ���� ����")]
    public float duration;
    public float range;
    public bool isShootable;
    private GameObject activeField;

    private Coroutine cooltimeCoroutine;

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
        cooltimeCoroutine = null;
    }

    protected override void Update()
    {
        base.Update();
        if (isLevelUp)
        {
            if(cooltimeCoroutine != null)
            {
                StopCoroutine(cooltimeCoroutine);
                cooltimeCoroutine = null;
            }
            if (activeField != null)
            {
                Managers.Instance.Pool.ReleasePool(activeField.gameObject);
            }
            LevelSet(level);
            activated = false;
        }

        if (!activated && cooltimeCoroutine == null)
        {
            cooltimeCoroutine = StartCoroutine(ActiveSkillInDelay(coolTime));
        }
    }

    private IEnumerator ActiveSkillInDelay(float coolTime)
    {
        activated = true;
        Debug.Log("��Ƽ�� ������ �ʵ� ������");
        Skill_EnergyField energyField = Managers.Instance.Pool.GetSkill(SkillProjType.Skill_EnergyField,
                transform.position, transform.rotation).GetComponent<Skill_EnergyField>();
        activeField = energyField.gameObject;

        yield return new WaitForSeconds(coolTime);
        cooltimeCoroutine = null;
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
                transform.GetComponent<SkillInterface>().skillIntro = "<������ �ʵ�>\n������ 20% ����";
                break;
            case 2:
                damageRate = 0.4f;
                duration = 5;
                range = 8;
                isShootable = false;
                transform.GetComponent<SkillInterface>().skillIntro = "<������ �ʵ�>\n���ӽð� 2�� ����";
                break;
            case 3:
                damageRate = 0.4f;
                duration = 7;
                range = 8;
                isShootable = false;
                transform.GetComponent<SkillInterface>().skillIntro = "<������ �ʵ�>\n���� 30%����";
                break;
            case 4:
                damageRate = 0.4f;
                duration = 7;
                range = 10;
                isShootable = false;
                transform.GetComponent<SkillInterface>().skillIntro = "<������ �ʵ�>\n������ 30% ����";
                break;
            case 5:
                damageRate = 0.7f;
                duration = 7;
                range = 10;
                isShootable = false;
                transform.GetComponent<SkillInterface>().skillIntro = "<������ �ʵ�>\n���ӽð� 2�� ����";
                break;
            case 6:
                damageRate = 0.7f;
                duration = 9;
                range = 12;
                isShootable = false;
                transform.GetComponent<SkillInterface>().skillIntro = "<������ �ʵ�>\n���� 30%����\n���ӽð��� ����� ������ �ʵ带 �������� ����";
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
