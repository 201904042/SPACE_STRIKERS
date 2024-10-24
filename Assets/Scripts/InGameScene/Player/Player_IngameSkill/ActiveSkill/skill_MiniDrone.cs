using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.MaterialProperty;

public class skill_MiniDrone : PlayerProjectile
{
    private const int droneBulletSpd = 15;
    private float droneAtkTime;
    private Transform target;
    private List<EnemyObject> triggedList;
    private SkillProjType droneBullet;

    private GameObject Player;
    private bool isAttacking = false;
    private Vector3 targetPos;
    private Coroutine attackCoroutine;
    private Coroutine behaviorCoroutine;

    private float detectionRange; // 드론이 플레이어와 감지 범위에서 멀어졌는지 판단하는 거리

    protected override void ResetProj()
    {
        base.ResetProj();
        droneAtkTime = 0;
        detectionRange = 0;
        triggedList = new List<EnemyObject>();
        droneBullet = SkillProjType.Skilll_DroneBullet;
        Player = GameManager.Instance.myPlayer;
        isAttacking = false;

        if (behaviorCoroutine != null)
        {
            StopCoroutine(behaviorCoroutine);
        }
        
    }

    public override void SetAddParameter(float value1, float value2 = 0, float value3 = 0)
    {
        base.SetAddParameter(value1, value2, value3);
        droneAtkTime = value1;
    }

    public override void SetProjParameter(int _projSpeed, int _dmgRate, float _liveTime, float _range)
    {
        Debug.Log("드론 파라미터 세팅");
        isParameterSet = true;

        if (_projSpeed == 0)
        {
            isShootingObj = false;
        }
        else
        {
            isShootingObj = true;
            speed = _projSpeed;
        }

        if (_dmgRate == 0)
        {
            Debug.LogError("데미지가 설정되지 않음");
        }
        else
        {
            damageRate = _dmgRate;
        }

        if (_liveTime != 0)
        {
            liveTime = _liveTime;
            StartCoroutine(LiveTimer(liveTime));
        }

        range = _range;
        detectionRange = range;
        transform.GetComponent<CircleCollider2D>().radius = range;

        behaviorCoroutine = StartCoroutine(DroneBehaviorRoutine());
    }

    protected override void Update()
    {
        Debug.Log("자식 Update");
    }

    private void SetTarget()
    {
        if (triggedList.Count == 0)
        {
            return;
        }
        else if (triggedList.Count == 1)
        {
            target = triggedList[0].transform;
        }
        else
        {
            int randomCount = Random.Range(0, triggedList.Count);
            target = triggedList[randomCount].transform;
        }
    }

    private IEnumerator AttackRoutine(float attackDelay)
    {
        DroneAttack();
        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
        attackCoroutine = null;
    }

    private void DroneAttack()
    {
        skill_MiniDroneBullet proj = GameManager.Instance.Pool.GetSkill(droneBullet, transform.position, transform.rotation).GetComponent<skill_MiniDroneBullet>();
        proj.SetProjParameter(droneBulletSpd, damageRate, liveTime, range);
    }

    private IEnumerator MoveToEnemyRoutine()
    {

        while (target != null && transform.position != target.position)
        {
            // 플레이어와 드론 사이의 거리 확인
            if (Vector3.Distance(transform.position, Player.transform.position) > detectionRange)
            {
                yield break;
            }

            // 적에게 이동
            targetPos = target.position + new Vector3(0, -1, 0);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);

            // 이동 후 적과의 거리 확인
            if (Vector3.Distance(transform.position, targetPos) < 0.1f) // 충분히 가까워지면
            {
                // 공격 시작
                if (!isAttacking)
                {
                    isAttacking = true; // 공격 중 표시
                    attackCoroutine = StartCoroutine(AttackRoutine(droneAtkTime));
                }
            }

            yield return null; // 다음 프레임으로 넘어감
        }

        // 공격이 끝나면 isAttacking을 false로 설정
        if (isAttacking)
        {
            isAttacking = false; // 공격 종료
        }
    }


    private IEnumerator ReturnToPlayerRoutine()
    {
        while (target == null && transform.position != Player.transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, Time.deltaTime * speed);
            yield return null;
        }
    }

    private IEnumerator DroneBehaviorRoutine()
    {
        while (true)
        {
            if (!isParameterSet)
            {
                yield return null;
            }
            if (target == null)
            {
                SetTarget(); // 타겟 설정
            }

            // 플레이어와 드론 사이의 거리 확인
            if (Vector3.Distance(transform.position, Player.transform.position) > detectionRange)
            {
                // 플레이어와 너무 멀어지면 적을 무시하고 플레이어에게 돌아감
                target = null;
                yield return StartCoroutine(ReturnToPlayerRoutine());
            }
            else if (target == null)
            {
                yield return StartCoroutine(ReturnToPlayerRoutine());
            }
            else
            {
                yield return StartCoroutine(MoveToEnemyRoutine());
            }

            yield return null;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (!triggedList.Contains(collision.GetComponent<EnemyObject>()))
            {
                triggedList.Add(collision.GetComponent<EnemyObject>());
            }
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (triggedList.Contains(collision.GetComponent<EnemyObject>()))
            {
                if (target != null && target.gameObject == collision.gameObject)
                {
                    target = null;
                }
                triggedList.Remove(collision.GetComponent<EnemyObject>());
            }
        }
    }

}
