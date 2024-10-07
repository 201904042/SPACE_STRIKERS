using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class skill_MiniDrone : MonoBehaviour
{
    public GameObject proj;

    private GameObject targetEnemy;
    private PlayerStat stat;
    Vector3 targetPosition;

    private float drone_damage;
    public float damageRate;
    private float shootSpeed;
    public float shootSpeedRate;
    private float moveSpeed;

    public float shootTimer;

    private float liveTime;

    private float attackRange;
    private bool isEnemySet;
    private bool is_firstSet;

    private float basic_speed;
    public bool isDroneActive;
    private bool isAttack;

    void Awake()
    {

        stat = GameManager.Instance.myPlayer.GetComponent<PlayerStat>();

        liveTime = 15f;
        attackRange = 5f;
        basic_speed = 1;
        moveSpeed = 10f;
    }

    private void OnEnable()
    {
        Init();

        StartCoroutine(ActiveSkill(liveTime));
    }

    private void Init()
    {
        drone_damage = stat.damage * damageRate;
        shootSpeed = basic_speed - shootSpeedRate;
        shootTimer = shootSpeed;
        isEnemySet = false;
        isAttack = false;
        isDroneActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!is_firstSet)
        {
            drone_damage = stat.damage * damageRate;
            shootTimer = shootSpeed;
            is_firstSet = true;
        }


        //에너미가 지정되지 않았거나, 타겟인 에너미가 비활성화되거나 , 적과 플레이어의 위치가 공격범위보다 크게 되면 적 재지정
        if (!isEnemySet || targetEnemy.activeSelf == false || Vector2.Distance(GameManager.Instance.myPlayer.transform.position, targetEnemy.transform.position) > attackRange) //타겟이 정해지지 않으면 다시 타겟을 정함
        {
            SetTarget();
            if(targetEnemy == null)
            {
                ReturnToPlayer();
            }
        }
        else
        {
            MoveToEnemy();
            if (transform.position == targetPosition)
            {
                if (!isAttack)
                {
                    StartCoroutine(AttackRoutine(shootSpeed));
                }
            }
        }
    }

    private IEnumerator ActiveSkill(float liveTime)
    {
        isDroneActive = true;
        yield return new WaitForSeconds(liveTime);

        isDroneActive = false;

        Managers.Instance.Pool.ReleasePool(gameObject);
    }

    private IEnumerator AttackRoutine(float attackDelay)
    {
        isAttack = true;
        DroneAttack();
        yield return new WaitForSeconds(attackDelay);

        isAttack = false;

    }

    private void SetTarget()
    {
        List<GameObject> enemyCloseToPlayer = new List<GameObject>();

        foreach(GameObject enemyObj in Managers.Instance.Spawn.activeEnemyList)
        {
            float dist = Vector2.Distance(enemyObj.transform.position, GameManager.Instance.myPlayer.transform.position);

            if (dist <= attackRange)
            {
                enemyCloseToPlayer.Add(enemyObj);
            }
        }

        
        if (enemyCloseToPlayer.Count > 0)
        {
            targetEnemy = enemyCloseToPlayer[Random.Range(0, enemyCloseToPlayer.Count)];
            isEnemySet = true;
        }
        else
        {
            targetEnemy = null;
            isEnemySet = false;
        }
    }

    private void MoveToEnemy()
    {
        targetPosition = targetEnemy.transform.position + new Vector3(0, -1, 0);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * moveSpeed);
    }

    private void ReturnToPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, GameManager.Instance.myPlayer.transform.position, Time.deltaTime * moveSpeed);
    }

    private void DroneAttack()
    {
        
        GameObject droneBullet = Managers.Instance.Pool.GetSkill(SkillProjType.Skilll_DroneBullet, transform.position, Quaternion.identity);
        droneBullet.GetComponent<skill_MiniDroneBullet>().damage = drone_damage;
    }

}
