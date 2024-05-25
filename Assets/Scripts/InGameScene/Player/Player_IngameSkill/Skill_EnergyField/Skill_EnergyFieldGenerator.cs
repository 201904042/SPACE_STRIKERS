using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_EnergyFieldGenerator : Ingame_Active
{
    [Header("에너지필드 고유 스텟")]
    public bool isLevelUp;
    private float duration;
    private float range;
    private bool isShootable;
    private GameObject field;

    protected override void Awake()
    {
        base.Awake();
        skillProj = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player/Player_InGameSkill/Proj/skill_EnergyField.prefab");
        DamageRate = 0.2f;
        coolTime = 10;
        timer = 0;
        duration = 5;
        range = 8;
        isShootable = false;
        isLevelUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (level != transform.GetComponent<SkillInterface>().level)
        {
            level = transform.GetComponent<SkillInterface>().level;
            
            if (field != null)
            {
                Destroy(field.gameObject);
            }
            LevelSet(level);
        }
        
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            field = Instantiate(skillProj, transform);
            Skill_EnergyField fieldstat = field.GetComponent<Skill_EnergyField>();
            fieldstat.enemyDamagerate = DamageRate;
            fieldstat.enemyDuration = duration;
            fieldstat.isEnemyShootable = isShootable;
            field.transform.localScale = field.transform.localScale * range / 8;
            timer = coolTime;
        }
    }

    void LevelSet(int level)
    {
        if (level == 1)
        {
            DamageRate = 0.2f;
            duration = 5;
            range = 8;
            isShootable = false;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<에너지 필드>\n데미지 20% 증가";
        }
        else if (level == 2)
        {
            DamageRate = 0.4f;
            duration = 5;
            range = 8;
            isShootable = false;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<에너지 필드>\n지속시간 2초 증가";
        }
        else if (level == 3)
        {
            DamageRate = 0.4f;
            duration = 7;
            range = 8;
            isShootable = false;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<에너지 필드>\n범위 30%증가";
        }
        else if (level == 4)
        {
            DamageRate = 0.4f;
            duration = 7;
            range = 10;
            isShootable = false;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<에너지 필드>\n데미지 30% 증가";

        }
        else if (level == 5)
        {
            DamageRate = 0.7f;
            duration = 7;
            range = 10;
            isShootable = false;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<에너지 필드>\n지속시간 2초 증가";

        }
        else if (level == 6)
        {
            DamageRate = 0.7f;
            duration = 9;
            range = 10;
            isShootable = false;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<에너지 필드>\n범위 30%증가\n지속시간이 종료된 에너지 필드를 전방으로 사출";

        }
        else if (level == 7)
        {
            DamageRate = 0.7f;
            duration = 9;
            range = 12;
            isShootable = true;
        }
        else
        {
            Debug.Log("Already Max or Min");
        }
        timer = 0;
    }
}
