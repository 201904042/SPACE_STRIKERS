using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.MaterialProperty;

public class skill_MiniDrone : PlayerProjectile
{
    private const int droneBulletSpd = 15;
    private List<EnemyObject> triggedList;

    private GameObject Player;
    private Transform target;
    private Vector3 targetPos;

    private Coroutine behaviorCoroutine;

    private float dAtkDelay;
    private float detectionRange; // 드론이 플레이어와 감지 범위에서 멀어졌는지 판단하는 거리

    protected override void ResetProj()
    {
        base.ResetProj();
        dAtkDelay = 0;
        detectionRange = 0;
        triggedList = new List<EnemyObject>();
        Player = PlayerMain.Instance.gameObject;

        if (behaviorCoroutine != null)
        {
            StopCoroutine(behaviorCoroutine);
        }
        
    }

    public override void SetAddParameter(float value1, float value2 =0, float value3 = 0, float value4 = 0)
    {
        base.SetAddParameter(value1, value2, value3, value4);
        dAtkDelay = value1;
        detectionRange = value2;
        GetComponent<CircleCollider2D>().radius = detectionRange;
    }

    public override void SetProjParameter(int _projSpeed, int _dmgRate, float _liveTime, float _range)
    {
        base.SetProjParameter(_projSpeed, _dmgRate, _liveTime, _range);
        
        behaviorCoroutine = StartCoroutine(DroneBehaviorRoutine());
        Debug.Log("드론 시작");
    }

    protected override void Update()
    {
        //업데이트 사용안함
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

   

    private void DroneAttack()
    {
        skill_MiniDroneBullet proj = GameManager.Game.Pool.GetPlayerProj(PlayerProjType.Skilll_DroneBullet, transform.position, transform.rotation).GetComponent<skill_MiniDroneBullet>();
        proj.SetProjParameter(droneBulletSpd, damageRate, 0, 0);
    }

    private IEnumerator MoveToEnemyRoutine()
    {

        while (target != null && transform.position != target.position)
        {
            // 플레이어와 드론 사이의 거리 확인
            if (Vector3.Distance(transform.position, Player.transform.position) > detectionRange/2)
            {
                yield break;
            }

            // 적에게 이동
            targetPos = target.position + new Vector3(0, -1, 0);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);

            // 이동 후 적과의 거리 확인
            if (Vector3.Distance(transform.position, targetPos) < 0.1f) // 충분히 가까워지면
            {
                DroneAttack();
                yield return new WaitForSeconds(dAtkDelay);
            }

            yield return null; // 다음 프레임으로 넘어감
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
                continue;
            }
            if (target == null)
            {
                SetTarget(); // 타겟 설정
            }

            // 플레이어와 드론 사이의 거리 확인
            if (Vector3.Distance(transform.position, Player.transform.position) > detectionRange/2)
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
