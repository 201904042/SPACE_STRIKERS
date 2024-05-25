using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_ShieldGenerator : Ingame_Active
{
    [Header("쉴드 고유 스텟")]
    public bool isLevelUp;
    public bool isPenetrate;
    public bool isShieldOn;
    protected override void Awake()
    {
        base.Awake();
        skillProj = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player/Player_InGameSkill/Proj/skill_shield.prefab");
        DamageRate = 1f;
        coolTime = 20;
        timer = 0;
        isShieldOn = false;
        isLevelUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (level != transform.GetComponent<SkillInterface>().level)
        {
            level = transform.GetComponent<SkillInterface>().level;
            LevelSet(level);

        }

        if (!isShieldOn)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                GameObject shield = Instantiate(skillProj, transform);
                shield.GetComponent<Skill_Shield>();
                isShieldOn= true;
                timer = coolTime;
            }
        }
        
    }

    void LevelSet(int level)
    {
        if (level == 1)
        {
            DamageRate = 1f;
            coolTime = 20;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<쉴드>\n재생성 3초 감소";
        }
        else if (level == 2)
        {
            DamageRate = 1f;
            coolTime = 17;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<쉴드>\n재생성 3초 감소";
        }
        else if (level == 3)
        {
            DamageRate = 1f;
            coolTime = 14;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<쉴드>\n충돌한 적에게 150% 데미지";
        }
        else if (level == 4)
        {
            DamageRate = 1.5f;
            coolTime = 14;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<쉴드>\n재생성 3초 감소";
        }
        else if (level == 5)
        {
            DamageRate = 1.5f;
            coolTime = 11;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<쉴드>\n재생성 3초 감소";
        }
        else if (level == 6)
        {
            DamageRate = 1.5f;
            coolTime = 8;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<쉴드>\n충돌한 적에게 300% 데미지";
        }
        else if (level == 7)
        {
            DamageRate = 3f;
            coolTime = 5;
        }
        else
        {
            Debug.Log("Already Max or Min");
        }
        timer = coolTime;
    }
}
