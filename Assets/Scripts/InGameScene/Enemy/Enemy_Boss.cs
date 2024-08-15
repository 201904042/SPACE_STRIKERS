using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Enemy_Boss : EnemyAct
{
    public int patternIndex;
    public string curPattern;
    protected override void Awake()
    {
        base.Awake();

    }

    protected override void Start()
    {
        base.Start();
        Init();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Init();
    }
    /// <summary>
    /// 초기화
    /// </summary>
    private void Init()
    {
        isMove = true;
        isAttack = false;
        patternIndex = 0;

        curPattern = string.Empty;
    }

    protected override void Update()
    {
        base.Update();
        if (transform.position.y >  3 && isMove)
        {
            EnemyMoveForward(gameObject);
            return;
        }
        else
        {
            if (isMove) {
                EnemyMoveStop(gameObject);
                isMove = false;
                isAttackReady = true;
            }
        }

        if (!isAttack && isAttackReady)
        {
            SetAttackType();
        }
    }

    private void SetAttackType()
    {
        patternIndex = patternIndex == 4 ? 0 : patternIndex + 1;
        switch (patternIndex)
        {
            case 0: StartCoroutine(Pattern1()); break;
            case 1: StartCoroutine(Pattern2()); break;
            case 2: StartCoroutine(Pattern3()); break;
            case 3: StartCoroutine(Pattern4()); break;
            case 4: StartCoroutine(Pattern5()); break;
        }
    }

    /// <summary>
    /// 패턴1 : 타겟불렛 5발 -> 4발 -> 5발
    /// </summary>
    private IEnumerator Pattern1()
    {
        isAttack = true;
        curPattern = "pattern1";
        BulletAttack(5, 10, 5,true);
        yield return new WaitForSeconds(1f);
        BulletAttack(4, 10, 5, true);
        yield return new WaitForSeconds(1f);
        BulletAttack(5, 10, 5, true);
        yield return new WaitForSeconds(1f);

        isAttack = false;
    }


    /// <summary>
    /// 10중 분열탄 3발
    /// </summary>
    private IEnumerator Pattern2()
    {
        isAttack = true;
        curPattern = "pattern2";
        BulletAttack(1, 0, 5, false,-180,true,10);
        yield return new WaitForSeconds(0.2f);
        BulletAttack(1, 0, 5, false, -125, true, 10);
        yield return new WaitForSeconds(0.2f);
        BulletAttack(1, 0, 5, false, 125, true, 10);
        yield return new WaitForSeconds(1f);

        isAttack = false;
    }

    /// <summary>
    ///30발 원형발사
    /// </summary>
    private IEnumerator Pattern3()
    {
        isAttack = true;
        curPattern = "pattern3";
        BulletAttack(30, 360, 5, true);
        yield return new WaitForSeconds(1f);
        BulletAttack(30, 355, 5, true);
        yield return new WaitForSeconds(1f);
        BulletAttack(30, 360, 5, true);
        yield return new WaitForSeconds(1f);

        isAttack = false;
    }

    /// <summary>
    /// 레이저
    /// </summary>
    /// <returns></returns>
    private IEnumerator Pattern4()
    {
        isAttack = true;
        curPattern = "pattern4";
        MultiLaser(1, 30, true);
        yield return new WaitForSeconds(3f);
        MultiLaser(1, 30, true);
        yield return new WaitForSeconds(3f);
        MultiLaser(1, 30, true);
        yield return new WaitForSeconds(3f);

        isAttack = false;
    }

    /// <summary>
    /// 일반적 소환
    /// </summary>
    /// <returns></returns>
    private IEnumerator Pattern5()
    {
        isAttack = true;
        curPattern = "pattern5";

        SpawnPattern selectedPattern = SpawnManager.spawnInstance.spawnPatterns[0];

        foreach (Vector2 pos in selectedPattern.positions)
        {
            PoolManager.poolInstance.GetEnemy(selectedPattern.enemyId, pos, selectedPattern.spawnZone.rotation);
        }

        yield return new WaitForSeconds(1f);

        isAttack = false;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}
