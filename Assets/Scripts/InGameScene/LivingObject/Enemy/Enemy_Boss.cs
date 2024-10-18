using System.Collections;
using UnityEngine;

public class Enemy_Boss : EnemyAct
{
    public int patternIndex;
    public string curPattern;
    private Coroutine attackCoroutine;

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

    private void Init()
    {
        isMove = true;
        isAttack = false;
        patternIndex = 0;
        curPattern = string.Empty;
        attackCoroutine = null;
    }

    protected override void OnDisable()
    {
        // Stop any running coroutine when disabled
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
        base.OnDisable();
    }

    protected override void Update()
    {
        base.Update();
        if (transform.position.y > 3 && isMove)
        {
            EnemyMoveForward(gameObject);
            return;
        }
        else
        {
            if (isMove)
            {
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
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null; 
        }

        patternIndex = (patternIndex == 4) ? 0 : patternIndex + 1;

        switch (patternIndex)
        {
            case 0: attackCoroutine = StartCoroutine(Pattern1()); break;
            case 1: attackCoroutine = StartCoroutine(Pattern2()); break;
            case 2: attackCoroutine = StartCoroutine(Pattern3()); break;
            case 3: attackCoroutine = StartCoroutine(Pattern4()); break;
            case 4: attackCoroutine = StartCoroutine(Pattern5()); break;
        }
    }

    private IEnumerator Pattern1()
    {
        isAttack = true;
        curPattern = "pattern1";
        BulletAttack(this,5, 10, 5, true);
        yield return new WaitForSeconds(1f);
        BulletAttack(this, 4, 10, 5, true);
        yield return new WaitForSeconds(1f);
        BulletAttack(this, 5, 10, 5, true);
        yield return new WaitForSeconds(3f);
        isAttack = false;
    }

    private IEnumerator Pattern2()
    {
        isAttack = true;
        curPattern = "pattern2";
        BulletAttack(this, 1, 0, 5, false, -180, true, 10);
        yield return new WaitForSeconds(0.2f);
        BulletAttack(this, 1, 0, 5, false, -125, true, 10);
        yield return new WaitForSeconds(0.2f);
        BulletAttack(this, 1, 0, 5, false, 125, true, 10);
        yield return new WaitForSeconds(3f);
        isAttack = false;
    }

    private IEnumerator Pattern3()
    {
        isAttack = true;
        curPattern = "pattern3";
        BulletAttack(this, 30, 360, 5, true);
        yield return new WaitForSeconds(1f);
        BulletAttack(this, 30, 355, 5, true);
        yield return new WaitForSeconds(1f);
        BulletAttack(this, 30, 360, 5, true);
        yield return new WaitForSeconds(3f);
        isAttack = false;
    }

    private IEnumerator Pattern4()
    {
        isAttack = true;
        curPattern = "pattern4";
        MultiLaser(this, 1, 30, true);
        yield return new WaitForSeconds(3f);
        MultiLaser(this, 1, 30, true);
        yield return new WaitForSeconds(3f);
        MultiLaser(this, 1, 30, true);
        yield return new WaitForSeconds(3f);
        isAttack = false;
    }

    private IEnumerator Pattern5()
    {
        isAttack = true;
        curPattern = "pattern5";
        SpawnPattern selectedPattern = Managers.Instance.Spawn.spawnPatterns[1]; //코드2번 적 3마리
        foreach (Vector2 pos in selectedPattern.positions)
        {
            Managers.Instance.Pool.GetEnemy(selectedPattern.enemyId, pos, selectedPattern.spawnZone.rotation);
        }
        yield return new WaitForSeconds(3f);
        isAttack = false;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}
