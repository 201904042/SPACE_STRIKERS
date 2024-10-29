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
    private float detectionRange; // ����� �÷��̾�� ���� �������� �־������� �Ǵ��ϴ� �Ÿ�

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
        Debug.Log("��� ����");
    }

    protected override void Update()
    {
        //������Ʈ ������
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
        skill_MiniDroneBullet proj = GameManager.Instance.Pool.GetPlayerProj(PlayerProjType.Skilll_DroneBullet, transform.position, transform.rotation).GetComponent<skill_MiniDroneBullet>();
        proj.SetProjParameter(droneBulletSpd, damageRate, 0, 0);
    }

    private IEnumerator MoveToEnemyRoutine()
    {

        while (target != null && transform.position != target.position)
        {
            // �÷��̾�� ��� ������ �Ÿ� Ȯ��
            if (Vector3.Distance(transform.position, Player.transform.position) > detectionRange/2)
            {
                yield break;
            }

            // ������ �̵�
            targetPos = target.position + new Vector3(0, -1, 0);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);

            // �̵� �� ������ �Ÿ� Ȯ��
            if (Vector3.Distance(transform.position, targetPos) < 0.1f) // ����� ���������
            {
                DroneAttack();
                yield return new WaitForSeconds(dAtkDelay);
            }

            yield return null; // ���� ���������� �Ѿ
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
                SetTarget(); // Ÿ�� ����
            }

            // �÷��̾�� ��� ������ �Ÿ� Ȯ��
            if (Vector3.Distance(transform.position, Player.transform.position) > detectionRange/2)
            {
                // �÷��̾�� �ʹ� �־����� ���� �����ϰ� �÷��̾�� ���ư�
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
