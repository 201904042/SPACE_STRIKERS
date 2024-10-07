using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Skill_MissileLauncher : Ingame_Active
{
    [Header("미사일런처 고유 스텟")]
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

        if (!activated)
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
            GameObject target = Managers.Instance.Spawn.activeEnemyList.Count > 0 ? Managers.Instance.Spawn.activeEnemyList[Random.Range(0, Managers.Instance.Spawn.activeEnemyList.Count)] : null;
            Vector3 direction = target != null ? (target.transform.position - transform.position + randx_pos).normalized : Vector2.up;
            GameObject missile = Managers.Instance.Pool.GetSkill(SkillProjType.Skill_Missile, transform.position + randx_pos, Quaternion.identity);
            
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
                    "<미사일런쳐>\n미사일 수 1개 증가";
                break;
            case 2:
                damageRate = 1.5f;
                coolTime = 3;
                projNum = 2;
                explosionRange = 1.0f;
                skillInterface.skillIntro =
                    "<미사일런쳐>\n데미지 계수 20% 증가";
                break;
            case 3:
                damageRate = 1.7f;
                coolTime = 3;
                projNum = 2;
                explosionRange = 1.0f;
                skillInterface.skillIntro =
                    "<미사일런쳐>\n쿨타임 1초 감소";
                break;
            case 4:
                damageRate = 1.7f;
                coolTime = 2;
                projNum = 2;
                explosionRange = 1.0f;
                skillInterface.skillIntro =
                    "<미사일런쳐>\n미사일 수 2개 증가";
                break;
            case 5:
                damageRate = 1.7f;
                coolTime = 2;
                projNum = 4;
                explosionRange = 1.0f;
                skillInterface.skillIntro =
                    "<미사일런쳐>\n쿨타임 1초 감소\n데미지 계수 30% 증가";
                break;
            case 6:
                damageRate = 2f;
                coolTime = 1;
                projNum = 4;
                explosionRange = 1.0f;
                skillInterface.skillIntro =
                    "<미사일런쳐>\n폭발범위 50% 증가\n데미지 계수 50%증가\n미사일 수 1개 증가 ";
                break;
            case 7:
                damageRate = 2.5f;
                coolTime = 1;
                projNum = 5;
                explosionRange = 1.5f;

                break;
            default:
                Debug.Log("레벨이 올바르지 않습니다.");
                break;
        }
        isLevelUp =false;
    }
}
