using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_ShieldGenerator : Ingame_Active
{
    [Header("쉴드 고유 스텟")]
    private bool isShieldOn;

    public bool ShieldOn
    {
        get => isShieldOn;
        set
        {
            isShieldOn = value;

            if (!value && coolTimerCoroutine == null)
            {
                coolTimerCoroutine = StartCoroutine(StartCoolTime(coolTime));
            }
            
        }
    }

    private Coroutine coolTimerCoroutine;

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
        coolTimerCoroutine = null;
        isLevelUp = false;
    }

    protected override void Update()
    {
        base.Update();
        if (isLevelUp) //레벨업을 할경우 쉴드 해제
        {
            if (transform.GetChild(0).gameObject != null && transform.GetChild(0).gameObject.activeSelf == true)
            {
                //레벨업을 했는데 쉴드가 작동중이라면 쉴드를 해제
                transform.GetChild(0).gameObject.SetActive(false);
                ShieldOn = false;
            }
            LevelSet(level);
            isLevelUp = false;
        }
    }

    private IEnumerator StartCoolTime(float coolTime)
    {
        Debug.Log("쉴드 쿨타임 시작");

        yield return new WaitForSeconds(coolTime);
        GenerateShield();

        coolTimerCoroutine = null;
    }

    private void GenerateShield()
    {
        PoolManager.poolInstance.GetSkill(SkillProjType.Skill_Shield, transform.position, transform.rotation);
        ShieldOn = true;
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
