using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyAct : EnemyObject
{
    [Header("공통 행동")]
    public int splitCount= 3;
    public float laserDangerZoneTime = 3;
    public float laserAttackTime = 3;
    public float defaultSpeed = 1;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void Update()
    {
        base.Update(); 
    }

    public void SingleShot(Vector3 velocity, bool split = false)
    {
        GameObject enemyProj;

        if (split)
        {
            enemyProj = PoolManager.poolInstance.GetProj(ProjType.Enemy_Split, transform.position, Quaternion.identity);
        }
        else
        {
            enemyProj = PoolManager.poolInstance.GetProj(ProjType.Enemy_Bullet, transform.position, Quaternion.identity);
        }

        if (enemyProj == null)
        {
            Debug.Log("총알을 불러오지 못함");
            return;
        }
        

        Rigidbody2D rigid = enemyProj.GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.zero;

        if (split)
        {
            enemyProj.GetComponent<EnemySplitBullet>().splitCount = splitCount;
        }
        else
        {
            enemyProj.GetComponent<EnemyBullet>().setDamage(enemyStat.enemyDamage);
        }

        rigid.velocity = new Vector2(velocity.x, velocity.y); //fireDirection * bulletSpeed;
    }


    //발사할 총알 프리팹, 분열총알이라면 true
    public void TargetShot(bool split = false)
    {
        Transform player = GameManager.gameInstance.myPlayer.transform;
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        if (split)
        {
            SingleShot(dirToPlayer * defaultSpeed, true);
        }
        else
        {
            SingleShot(dirToPlayer * defaultSpeed);
        }

    }

    //발사할 총알, 총알개수, 사격각도범위, 총알속도, 조준여부, 발사체의 기본 앵글, 분열총알여부
    public void BulletAttack(int projNum, float projAngle, float bulletSpeed = 10f, bool isAimToPlayer = false, float projBasicAngle = -180, bool split = false)
    {
        //Debug.Log($"샷 진입" +
           // $"projNum {projNum} projAngle {projAngle}  bulletSpeed {bulletSpeed},  isAimToPlayer {isAimToPlayer} , projBasicAngle {projBasicAngle} , split {split}");
        float angleStep = projAngle / projNum;
        float angleInit;
        if (projNum % 2 == 0) //짝수
        {
            angleInit = -projBasicAngle - projAngle / 2 + angleStep / 2;
        }
        else // 홀수
        {
            angleInit = -projBasicAngle - (angleStep * (projNum / 2));
        }

        for (int i = 0; i < projNum; i++)
        {
            float angle = angleInit + angleStep * i;
            if (isAimToPlayer)
            {
                //조준일 경우
                Vector3 dirToPlayer;
                Transform player = GameManager.gameInstance.myPlayer.transform;
                dirToPlayer = (player.position - transform.position).normalized;

                Vector3 velocity = Quaternion.Euler(0, 0, angle) * -dirToPlayer;
                if (split)
                {
                    SingleShot( velocity * bulletSpeed, true);
                }
                else
                {
                    SingleShot(velocity * bulletSpeed);
                }
            }
            else
            {
                //조준이 아닐경우
                float projectileDirXPosition = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180);
                float projectileDirYPosition = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180);

                Vector2 projectileVector = new Vector2(projectileDirXPosition, projectileDirYPosition);
                Vector2 projectileMoveDirection = (projectileVector - (Vector2)transform.position).normalized * bulletSpeed;
                Vector3 velocity = new Vector3(projectileMoveDirection.x, projectileMoveDirection.y, 0);
                if (split)
                {
                    SingleShot( velocity, true);
                }
                else
                {
                    SingleShot(velocity);
                }
            }
        }
    }

    //레이저 여러발일 경우의 앵글(기본 0), 조준여부
    public void Laser(float multiAngle = 0, bool isAimtoPlayer = false)
    {
        EnemyLaser laserObject = PoolManager.poolInstance.GetProj(ProjType.Enemy_Laser, transform.position, Quaternion.identity).GetComponent<EnemyLaser>();
        laserObject.setDamage(enemyStat.enemyDamage);
        if (isAimtoPlayer)
        {
            Transform player = GameManager.gameInstance.myPlayer.transform;
            Vector3 direction = player.position - laserObject.gameObject.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            laserObject.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90 - multiAngle));
        }
        else
        {
            laserObject.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, multiAngle));
        }

        laserObject.startPointObj = gameObject;
        laserObject.chargingTime = laserDangerZoneTime;
        laserObject.laserTime = laserAttackTime;
    }
    //발사개수, 발사 최대범위, 조준여부 , 기본레이저앵글(기본 0)
    public void MultiLaser(int projNum, float projAngle, bool isAimToPlayer = false, float projBasicAngle = 0)
    {
        float angleStep = projAngle / projNum;
        float angleInit;
        if (projNum % 2 == 0) //짝수
        {
            angleInit = projBasicAngle - projAngle / 2 + angleStep / 2;
        }
        else // 홀수
        {
            angleInit = projBasicAngle - (angleStep * (projNum / 2));
        }

        for (int i = 0; i < projNum; i++)
        {
            float angle = angleInit + angleStep * i;
            if (isAimToPlayer)
            {
                Debug.Log(isAimToPlayer);
                Laser(angle, isAimToPlayer);
            }
            else
            {
                Laser(angle, isAimToPlayer);
            }
        }
    }


    public void EnemyMoveForward(GameObject movingObject)
    {
        Vector2 moveDirection = movingObject.transform.up;
        Rigidbody2D enemyRigid = movingObject.GetComponent<Rigidbody2D>();
        EnemyObject movingObjectStat = movingObject.GetComponent<EnemyObject>();
        enemyRigid.velocity = moveDirection * movingObjectStat.enemyStat.enemyMoveSpeed;
    }

    public void EnemyMoveStop(GameObject movingObject)
    {
        Rigidbody2D enemyRigid = movingObject.GetComponent<Rigidbody2D>();

        enemyRigid.velocity = Vector2.zero;
    }



    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}
