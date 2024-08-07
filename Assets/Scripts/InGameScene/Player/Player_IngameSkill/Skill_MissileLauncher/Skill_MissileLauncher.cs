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
    

    protected override void Awake()
    {
        base.Awake();
        
        damageRate = 1.5f;
        coolTime = 3;
        projNum = 1;
        explosionRange = 1.0f;
        isLevelUp = false;
    }

    protected override void OnEnable()
    {
        Init();
    }


    protected override void Init()
    {
        base.Init();
        level = skillInterface.level;
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
        for (int i = 0; i < projNum; i++)
        {
            Vector3 randx_pos = new Vector3(Random.Range(-0.1f, 0.1f), 0, 0);
            GameObject target = SpawnManager.spawnInstance.activeEnemyList[Random.Range(0, SpawnManager.spawnInstance.activeEnemyList.Count)];
            Vector3 direction = (target.transform.position - transform.position + randx_pos).normalized;
            GameObject missile = ObjectPool.poolInstance.GetSkill(SkillProjType.Skill_Missile, transform.position + randx_pos, Quaternion.identity);
            
            missile.transform.up = direction;
        }
        yield return new WaitForSeconds(coolTime);
        activated = false;
    }

    protected override void LevelSet(int level)
    {
        base.LevelSet(level);
        switch (level)
        {
            case 1:
                damageRate = 1.5f;
                coolTime = 3;
                projNum = 1;
                explosionRange = 1.0f;
                skillInterface.skillIntro =
                    "<�̻��Ϸ���>\n�̻��� �� 1�� ����";
                break;
            case 2:
                damageRate = 1.5f;
                coolTime = 3;
                projNum = 2;
                explosionRange = 1.0f;
                skillInterface.skillIntro =
                    "<�̻��Ϸ���>\n������ ��� 20% ����";
                break;
            case 3:
                damageRate = 1.7f;
                coolTime = 3;
                projNum = 2;
                explosionRange = 1.0f;
                skillInterface.skillIntro =
                    "<�̻��Ϸ���>\n��Ÿ�� 1�� ����";
                break;
            case 4:
                damageRate = 1.7f;
                coolTime = 2;
                projNum = 2;
                explosionRange = 1.0f;
                skillInterface.skillIntro =
                    "<�̻��Ϸ���>\n�̻��� �� 2�� ����";
                break;
            case 5:
                damageRate = 1.7f;
                coolTime = 2;
                projNum = 4;
                explosionRange = 1.0f;
                skillInterface.skillIntro =
                    "<�̻��Ϸ���>\n��Ÿ�� 1�� ����\n������ ��� 30% ����";
                break;
            case 6:
                damageRate = 2f;
                coolTime = 1;
                projNum = 4;
                explosionRange = 1.0f;
                skillInterface.skillIntro =
                    "<�̻��Ϸ���>\n���߹��� 50% ����\n������ ��� 50%����\n�̻��� �� 1�� ���� ";
                break;
            case 7:
                damageRate = 2.5f;
                coolTime = 1;
                projNum = 5;
                explosionRange = 1.5f;

                break;
            default:
                Debug.Log("������ �ùٸ��� �ʽ��ϴ�.");
                break;
        }
        isLevelUp =false;
    }
}
