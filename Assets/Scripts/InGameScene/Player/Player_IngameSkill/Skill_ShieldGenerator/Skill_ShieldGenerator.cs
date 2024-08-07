using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_ShieldGenerator : Ingame_Active
{
    [Header("쉴드 고유 스텟")]
    public bool isPenetrate;
    public bool isShieldOn;
    protected override void Awake()
    {
        base.Awake();
       
    }

    protected override void OnEnable()
    {
        Init();
        LevelSet(level);
        GenerateShield();
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
            if (transform.GetChild(0).gameObject == null && transform.GetChild(0).gameObject.activeSelf == true)
            {
                transform.GetChild(0).gameObject.SetActive(false);
                isShieldOn = false;
            }
            LevelSet(level);
            isLevelUp = false;
        }

        if (!activated && !isShieldOn)
        {
            StartCoroutine(ActiveSkillInDelay(coolTime));
        }
    }

    private IEnumerator ActiveSkillInDelay(float coolTime)
    {
        activated = true;
        yield return new WaitForSeconds(coolTime);
        activated = false;
        GenerateShield();
    }

    private void GenerateShield()
    {
        ObjectPool.poolInstance.GetSkill(SkillProjType.Skill_Shield, transform.position, transform.rotation);
        isShieldOn = true;
    }


    protected override void LevelSet(int level)
    {
        base.LevelSet(level);

        switch (level)
        {
            case 1:
                damageRate = 1f;
                coolTime = 20;
                transform.GetComponent<SkillInterface>().skillIntro =
                    "<쉴드>\n재생성 3초 감소";
                break;
            case 2:
                damageRate = 1f;
                coolTime = 17;
                transform.GetComponent<SkillInterface>().skillIntro =
                    "<쉴드>\n재생성 3초 감소";
                break;
            case 3:
                damageRate = 1f;
                coolTime = 14;
                transform.GetComponent<SkillInterface>().skillIntro =
                    "<쉴드>\n충돌한 적에게 150% 데미지";
                break;
            case 4:
                damageRate = 1.5f;
                coolTime = 14;
                transform.GetComponent<SkillInterface>().skillIntro =
                    "<쉴드>\n재생성 3초 감소";
                break;
            case 5:
                damageRate = 1.5f;
                coolTime = 11;
                transform.GetComponent<SkillInterface>().skillIntro =
                    "<쉴드>\n재생성 3초 감소";
                break;
            case 6:
                damageRate = 1.5f;
                coolTime = 8;
                transform.GetComponent<SkillInterface>().skillIntro =
                    "<쉴드>\n충돌한 적에게 300% 데미지";
                break;
            case 7:
                damageRate = 3f;
                coolTime = 5;
                break;
            default:
                Debug.Log("Already Max or Min");
                break;
        }
    }

}
