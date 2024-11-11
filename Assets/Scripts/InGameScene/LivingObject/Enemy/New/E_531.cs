using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class E_531 : E_BossBase
{
    private Vector3 startPos;
    private void Awake() //���� ���̵� �ο�
    {
        id = 531;
    }

    protected override void SetStat()
    {
        base.SetStat();
    }

    protected override IEnumerator AppearMove() //������ �����Ҷ� ������
    {
        yield return StartCoroutine(MoveToPosition(Vector3.up * moveDistance, moveSpeed/2));
        startPos = transform.position;
    }

    public const float moveDistance = 3f; // �翷���� �̵��� �Ÿ�
    protected override IEnumerator RepeatMove() //�������� ���������� �ݺ��ؼ� ������ ����
    {
        while (true)
        {
            // ���� ������ �̵�
            yield return StartCoroutine(MoveToPosition(startPos - Vector3.right * moveDistance, moveSpeed));

            // ������ ������ �̵�
            yield return StartCoroutine(MoveToPosition(startPos + Vector3.right * moveDistance, moveSpeed));
        }
    }

    

    protected override void InitializeAttackPatterns()
    {
        attackPatterns.Add(() => StartPattern(Pattern1()));
        attackPatterns.Add(() => StartPattern(Pattern2()));
        attackPatterns.Add(() => StartPattern(Pattern3()));
        attackPatterns.Add(() => StartPattern(Pattern4()));
    }
    private void StartPattern(IEnumerator pattern)
    {
        StartCoroutine(pattern);
    }

    private IEnumerator Pattern1()
    {
        isAttack = true;
        //�÷��̾ �����Ͽ� ��ä�� ���ÿ� �Ѿ� �߻�
        //1������ 3�� 45�������� 5��
        //2������ 5�� 60�������� 7��
        int fireCount = phase == 1 ? 3 : 5;
        int projCount = phase == 1 ? 5 : 7;
        float projRange = phase == 1 ? 45 : 60;
        float delay = phase == 1 ? 1 : 0.5f;

        for (int i = 0; i < fireCount; i++)
        {
            EnemyProjectile[] proj = FireMulti(OtherProjType.Enemy_Bullet, projCount, projRange, true);
            foreach (EnemyProjectile p in proj)
            {
                p.SetProjParameter(damage, 0);
            }
            yield return new WaitForSeconds(delay);
        }

        isAttack = false;
    }

    private IEnumerator Pattern2()
    {
        isAttack = true;

        int fireCount = phase == 1 ? 3 : 1;
        int splitCount = phase == 1 ? 5 : 10;
        float delay = phase == 1 ? 3 : 0;
        bool aim = phase == 1 ? true : false;

        int projCount = phase == 1 ? 0 : 5;
        int projRange = phase == 1 ? 0 : 120;

 
        //�п�ź�� �߻�
        //phase1 : ����, �̱�, 3ȸ�߻�, 3�ʿ� ���ļ�
        //phase2 : 1ȸ �߻�, ��Ƽ, 120������ 5��
        for (int i = 0; i < fireCount; i++)
        {
            if(phase == 1)
            {
                EnemyProjectile proj = FireSingle(OtherProjType.Enemy_Split, 0, aim);
                proj.SetProjParameter(damage, 0);
                proj.SetSplitCount(splitCount);
            }
            else
            {
                EnemyProjectile[] projs = FireMulti(OtherProjType.Enemy_Split, projCount, projRange, aim);
                foreach (EnemyProjectile proj in projs)
                {
                    proj.SetProjParameter(damage, 0);
                    proj.SetSplitCount(splitCount);
                }
            }
            
            yield return new WaitForSeconds(delay);
        }

       
        isAttack = false;
    }

    private IEnumerator Pattern3()
    {
        isAttack = true;
        //�������� źȯ�� �߻�
        //������1 360������ �Ѿ� 30���� 3ȸ �߻�
        //������2 360������ �п��Ѿ� 10�� 1ȸ �߻�

        int fireCount = phase == 1 ? 3 : 1;
        int splitCount = 3;
        int projCount = phase == 1 ? 30 : 10;
        float projRange = 360;
        float delay = 1;
        bool aim = false;

        for (int i = 0; i < fireCount; i++)
        {
            if (phase == 1)
            {
                EnemyProjectile[] proj = FireMulti(OtherProjType.Enemy_Bullet, projCount, projRange, aim);
                foreach (EnemyProjectile p in proj)
                {
                    p.SetProjParameter(damage, 0);
                }
            }
            else
            {
                EnemyProjectile[] proj = FireMulti(OtherProjType.Enemy_Split, projCount, projRange, aim);
                foreach (EnemyProjectile p in proj)
                {
                    p.SetProjParameter(damage, 0);
                    p.SetSplitCount(splitCount);
                }
            }
            yield return new WaitForSeconds(delay);
        }

        isAttack = false;
    }

    private IEnumerator Pattern4()
    {
        isAttack = true;
        //�������� �߻�
        //������1 ���濡 �������� 120������ 5�� �߻�
        //������2 �÷��̾ �����Ͽ� 1�ʰ������� ������ 10ȸ �߻�

        int fireCount = phase == 1 ? 1 : 10;
        int projCount = phase == 1 ? 5 : 1;
        float projRange = 120;
        float delay = 1;


        for (int i = 0; i < fireCount; i++)
        {
            if (phase == 1)
            {
                float startAngle = -30;
                float angleOffset = 30;
                EnemyProjectile[] proj = FireMulti(OtherProjType.Enemy_Laser, projCount, projRange, false);
                for(int j = 0; j< proj.Length; j++)
                {
                    float angle = startAngle + i* angleOffset;
                    proj[j].SetProjParameter(damage, 0);
                    proj[j].SetLaser(gameObject, false, angle, 3, 1, 1);
                }
            }
            else
            {
                EnemyProjectile proj = FireSingle(OtherProjType.Enemy_Laser, 0, true);
                proj.SetProjParameter(damage, 0);
                proj.SetLaser(gameObject, true, 0, 3, 1, 1);
                yield return new WaitForSeconds(delay);
            }
            
        }

        yield return new WaitForSeconds(3f);
        isAttack = false;
    }

    //private IEnumerator Pattern5()
    //{  �Ϲ��� Ȥ�� ����Ʈ�� ��ȯ�Ѵ�
    //    isAttack = true;
    //    curPattern = "pattern5";
    //    SpawnPattern selectedPattern = GameManager.Game.Spawn.spawnPatterns[1]; //�ڵ�2�� �� 3����
    //    foreach (Vector2 pos in selectedPattern.positions)
    //    {
    //        GameManager.Game.Pool.GetEnemy(selectedPattern.enemyId, pos, selectedPattern.spawnZone.rotation);
    //    }
    //    yield return new WaitForSeconds(3f);
    //    isAttack = false;
    //}

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}
