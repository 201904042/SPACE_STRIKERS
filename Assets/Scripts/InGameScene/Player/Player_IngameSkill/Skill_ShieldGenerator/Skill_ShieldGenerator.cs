using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_ShieldGenerator : Ingame_Active
{
    [Header("���� ���� ����")]
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
        if (isLevelUp) //�������� �Ұ�� ���� ����
        {
            if (transform.GetChild(0).gameObject != null && transform.GetChild(0).gameObject.activeSelf == true)
            {
                //�������� �ߴµ� ���尡 �۵����̶�� ���带 ����
                transform.GetChild(0).gameObject.SetActive(false);
                ShieldOn = false;
            }
            LevelSet(level);
            isLevelUp = false;
        }
    }

    private IEnumerator StartCoolTime(float coolTime)
    {
        Debug.Log("���� ��Ÿ�� ����");

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
                    "<����>\n����� 3�� ����";
                break;
            case 2:
                damageRate = 1f;
                coolTime = 17;
                transform.GetComponent<SkillInterface>().skillIntro =
                    "<����>\n����� 3�� ����";
                break;
            case 3:
                damageRate = 1f;
                coolTime = 14;
                transform.GetComponent<SkillInterface>().skillIntro =
                    "<����>\n�浹�� ������ 150% ������";
                break;
            case 4:
                damageRate = 1.5f;
                coolTime = 14;
                transform.GetComponent<SkillInterface>().skillIntro =
                    "<����>\n����� 3�� ����";
                break;
            case 5:
                damageRate = 1.5f;
                coolTime = 11;
                transform.GetComponent<SkillInterface>().skillIntro =
                    "<����>\n����� 3�� ����";
                break;
            case 6:
                damageRate = 1.5f;
                coolTime = 8;
                transform.GetComponent<SkillInterface>().skillIntro =
                    "<����>\n�浹�� ������ 300% ������";
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
