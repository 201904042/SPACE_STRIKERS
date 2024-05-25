using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillElecShockLauncher : Ingame_Active
{
    [Header("�Ϸ���ũ ���� ����")]
    public float shockRange;
    public float slowRate;
    public float slowTime;
    public bool isExtraDamageToSlowEnemyOn;
    protected override void Awake()
    {
        base.Awake();
        skillProj = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player/Player_InGameSkill/Proj/skill_ElecShock.prefab");
        DamageRate = 0.3f;
        coolTime = 5;
        timer = coolTime;
        projNum = 1;

        shockRange = 1.0f;
        slowRate = 0.3f;
        slowTime = 1f;
        isExtraDamageToSlowEnemyOn = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (level != transform.GetComponent<SkillInterface>().level)
        {
            level = transform.GetComponent<SkillInterface>().level;
            LevelSet(level);
        }

        timer -= Time.deltaTime;
        if (timer <= 0)
        {

            Skill_ElecShock shockObj = Instantiate(skillProj, transform.position, Quaternion.identity).GetComponent<Skill_ElecShock>();
            shockObj.shockDamageRate = DamageRate;
            shockObj.shockRange = shockRange;
            shockObj.slowRate = slowRate;
            shockObj.slowTime = slowTime;
            shockObj.isExtraDamageToSlowEnemyOn = isExtraDamageToSlowEnemyOn;

            timer = coolTime;
        }
    }

    void LevelSet(int level)
    {
        if (level == 1)
        {
            DamageRate = 0.3f;
            shockRange = 1.0f;
            slowRate = 0.3f;
            slowTime = 2f;
            isExtraDamageToSlowEnemyOn = false;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<�Ϸ���ũ>\n��ȭ�ð� 0.5�� ����";
        }
        else if (level == 2)
        {
            DamageRate = 0.3f;
            shockRange = 1.0f;
            slowRate = 0.3f;
            slowTime = 2.5f;
            isExtraDamageToSlowEnemyOn = false;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<�Ϸ���ũ>\n��ȭ�� 20% ����";
        }
        else if (level == 3)
        {
            DamageRate = 0.3f;
            shockRange = 1.0f;
            slowRate = 0.5f;
            slowTime = 2.5f;
            isExtraDamageToSlowEnemyOn = false;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<�Ϸ���ũ>\n���� 50% ����";
        }
        else if (level == 4)
        {
            DamageRate = 0.3f;
            shockRange = 1.5f;
            slowRate = 0.5f;
            slowTime = 2.5f;
            isExtraDamageToSlowEnemyOn = false;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<�Ϸ���ũ>\n��ȭ�ð� 0.5�� ����";
        }
        else if (level == 5)
        {
            DamageRate = 0.3f;
            shockRange = 1.5f;
            slowRate = 0.5f;
            slowTime = 3f;
            isExtraDamageToSlowEnemyOn = false;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<�Ϸ���ũ>\n��ȭ�� 20% ����";
        }
        else if (level == 6)
        {
            DamageRate = 0.3f;
            shockRange = 1.5f;
            slowRate = 0.7f;
            slowTime = 3f;
            isExtraDamageToSlowEnemyOn = false;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<�Ϸ���ũ>\n��ȭ�� ������ ������ 100%����\n���� 100% ����";
        }
        else if (level == 7)
        {
            DamageRate = 0.3f;
            shockRange = 2.5f;
            slowRate = 0.7f;
            slowTime = 3f;
            isExtraDamageToSlowEnemyOn = true;
        }
        else
        {
            Debug.Log("Already Max or Min");
        }
    }
}
