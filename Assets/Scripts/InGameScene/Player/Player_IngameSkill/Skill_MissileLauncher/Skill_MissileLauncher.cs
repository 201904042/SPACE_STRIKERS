using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Skill_MissileLauncher : Ingame_Active
{
    [Header("�̻��Ϸ�ó ���� ����")]
    public float explosionRange;
    public bool isLevelUp;

    protected override void Awake()
    {
        base.Awake();
        skillProj = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player/Player_InGameSkill/Proj/skill_missile.prefab");
        DamageRate = 1.5f;
        coolTime = 3;
        timer = coolTime;
        projNum = 1;

        explosionRange = 1.0f;
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
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            FindEnemyWithTag();
            if(enemies.Length == 0)
            {
                return;
            }
            for (int i = 0; i < projNum; i++)
            {
                Vector3 randx_pos = new Vector3(Random.Range(-0.1f, 0.1f), 0, 0);
                GameObject target = enemies[Random.Range(0, enemies.Length)];
                Vector3 direction = (target.transform.position - transform.position+ randx_pos).normalized;
                GameObject missile = Instantiate(skillProj, transform.position+ randx_pos, Quaternion.identity);
                missile.GetComponent<skill_Missile>().explosionRange = explosionRange;
                missile.GetComponent<skill_Missile>().missileDamageRate = DamageRate;
                missile.transform.up = direction;

            }
            timer = coolTime;
        }
    }

    void LevelSet(int level)
    {
        if (level == 1)
        {
            DamageRate = 1.5f;
            coolTime = 3;
            projNum = 1;
            explosionRange = 1.0f;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<�̻��Ϸ���>\n�̻��� �� 1�� ����";
        }
        else if (level == 2)
        {
            DamageRate = 1.5f;
            coolTime = 3;
            projNum = 2;
            explosionRange = 1.0f;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<�̻��Ϸ���>\n������ ��� 20% ����";
        }
        else if (level == 3)
        {
            DamageRate = 1.7f;
            coolTime = 3;
            projNum = 2;
            explosionRange = 1.0f;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<�̻��Ϸ���>\n��Ÿ�� 1�� ����";
        }
        else if (level == 4)
        {
            DamageRate = 1.7f;
            coolTime = 2;
            projNum = 2;
            explosionRange = 1.0f;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<�̻��Ϸ���>\n�̻��� �� 2�� ����";
        }
        else if (level == 5)
        {
            DamageRate = 1.7f;
            coolTime = 2;
            projNum = 4;
            explosionRange = 1.0f;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<�̻��Ϸ���>\n��Ÿ�� 1�� ����\n������ ��� 30% ����";
        }
        else if (level == 6)
        {
            DamageRate = 2f;
            coolTime = 1;
            projNum = 4;
            explosionRange = 1.0f;
            transform.GetComponent<SkillInterface>().skillIntro =
                "<�̻��Ϸ���>\n���߹��� 50% ����\n������ ��� 50%����\n�̻��� �� 1�� ���� ";
        }
        else if (level == 7)
        {
            DamageRate = 2.5f;
            coolTime = 1;
            projNum = 5;
            explosionRange = 1.5f;
        }
        else
        {
            Debug.Log("Already Max or Min");
        }
    }
}
