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
    private bool isCollideStopLine;
    private void Awake() //최초 아이디 부여
    {
        id = 531;
    }

    protected override void SetStat()
    {
        base.SetStat();
        isCollideStopLine = false;
    }

    protected override IEnumerator AppearMove() //생성시 등장할때 움직임
    {
        yield return StartCoroutine(MoveToPosition(Vector3.up * moveDistance, moveSpeed/2));
        startPos = transform.position;
    }

    public const float moveDistance = 3f; // 양옆으로 이동할 거리
    protected override IEnumerator RepeatMove() //등장이후 보스전동안 반복해서 움직일 패턴
    {
        while (true)
        {
            // 왼쪽 끝으로 이동
            yield return StartCoroutine(MoveToPosition(startPos - Vector3.right * moveDistance, moveSpeed));

            // 오른쪽 끝으로 이동
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
        //플레이어를 조준하여 부채꼴 동시에 총알 발사
        //1페이즈 3번 45도각으로 5발
        //2페이즈 5번 60도각으로 7발
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

 
        //분열탄을 발사
        //phase1 : 조준, 싱글, 3회발사, 3초에 걸쳐서
        //phase2 : 1회 발사, 멀티, 120각도로 5발
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
        //원형으로 탄환을 발사
        //페이즈1 360각도로 총알 30발을 3회 발사
        //페이즈2 360각도로 분열총알 10발 1회 발사

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
        //레이저를 발사
        //페이즈1 전방에 레이저를 120각도로 5발 발사
        //페이즈2 플레이어를 추적하여 1초간격으로 레이저 10회 발사

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
    //{  일반적 혹은 엘리트를 소환한다
    //    isAttack = true;
    //    curPattern = "pattern5";
    //    SpawnPattern selectedPattern = GameManager.Game.Spawn.spawnPatterns[1]; //코드2번 적 3마리
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
        if (collision.CompareTag("Enemy_StopZone"))
        {
            isCollideStopLine = true;
        }
    }
}
