using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyAct : EnemyObject
{
    public static void SingleShot(EnemyObject enemy,Vector3 velocity, bool split = false, int splitCount = 3)
    {
        GameObject enemyProj;

        if (split)
        {
            enemyProj = Managers.Instance.Pool.GetProj(ProjType.Enemy_Split, enemy.transform.position, Quaternion.identity);
        }
        else
        {
            enemyProj = Managers.Instance.Pool.GetProj(ProjType.Enemy_Bullet, enemy.transform.position, Quaternion.identity);
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
            enemyProj.GetComponent<EnemyBullet>().setDamage(enemy.enemyStat.damage);
        }

        rigid.velocity = new Vector2(velocity.x, velocity.y); //fireDirection * bulletSpeed;
    }


    //발사할 총알 프리팹, 분열총알이라면 true
    public static void TargetShot(EnemyObject enemy,  bool split = false, float speed = 1, int splitCount = 3)
    {
        Transform player = GameManager.game.myPlayer.transform;
        Vector3 dirToPlayer = (player.position - enemy.transform.position).normalized;
        if (split)
        {
            SingleShot(enemy,dirToPlayer * speed, true, splitCount);
        }
        else
        {
            SingleShot(enemy, dirToPlayer * speed);
        }

    }

    //발사할 총알, 총알개수, 사격각도범위, 총알속도, 조준여부, 발사체의 기본 앵글, 분열총알여부
    public static void BulletAttack(EnemyObject enemy, int projNum, float projAngle, float bulletSpeed = 10f, bool isAimToPlayer = false, float projBasicAngle = -180, bool split = false, int splitCount = 3)
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
                Transform player = GameManager.game.myPlayer.transform;
                dirToPlayer = (player.position - enemy.transform.position).normalized;

                Vector3 velocity = Quaternion.Euler(0, 0, angle) * -dirToPlayer;
                if (split)
                {
                    SingleShot(enemy, velocity * bulletSpeed, true, splitCount);
                }
                else
                {
                    SingleShot(enemy, velocity * bulletSpeed);
                }
            }
            else
            {
                //조준이 아닐경우
                float projectileDirXPosition = enemy.transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180);
                float projectileDirYPosition = enemy.transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180);

                Vector2 projectileVector = new Vector2(projectileDirXPosition, projectileDirYPosition);
                Vector2 projectileMoveDirection = (projectileVector - (Vector2)enemy.transform.position).normalized * bulletSpeed;
                Vector3 velocity = new Vector3(projectileMoveDirection.x, projectileMoveDirection.y, 0);
                if (split)
                {
                    SingleShot(enemy, velocity, true,splitCount);
                }
                else
                {
                    SingleShot(enemy, velocity);
                }
            }
        }
    }

    //레이저 여러발일 경우의 앵글(기본 0), 조준여부
    public static void Laser(EnemyObject enemy,float multiAngle = 0, bool isAimtoPlayer = false)
    {
        EnemyLaser laserObject = Managers.Instance.Pool.GetProj(ProjType.Enemy_Laser, enemy.transform.position, Quaternion.identity).GetComponent<EnemyLaser>();
        laserObject.setDamage(enemy.enemyStat.damage);
        if (isAimtoPlayer)
        {
            Transform player = GameManager.game.myPlayer.transform;
            Vector3 direction = player.position - laserObject.gameObject.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            laserObject.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90 - multiAngle));
        }
        else
        {
            laserObject.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, multiAngle));
        }

        laserObject.LaserActive(enemy.gameObject);
    }

    //발사개수, 발사 최대범위, 조준여부 , 기본레이저앵글(기본 0)
    public static void MultiLaser(EnemyObject enemy, int projNum, float projAngle, bool isAimToPlayer = false, float projBasicAngle = 0)
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
                Laser(enemy,angle, isAimToPlayer);
            }
            else
            {
                Laser(enemy, angle, isAimToPlayer);
            }
        }
    }


    public static void EnemyMoveForward(GameObject movingObject)
    {
        Vector2 moveDirection = movingObject.transform.up;
        Rigidbody2D enemyRigid = movingObject.GetComponent<Rigidbody2D>();
        EnemyObject movingObjectStat = movingObject.GetComponent<EnemyObject>();
        enemyRigid.velocity = moveDirection * movingObjectStat.enemyStat.moveSpeed;
    }

    public static void EnemyMoveStop(GameObject movingObject)
    {
        Rigidbody2D enemyRigid = movingObject.GetComponent<Rigidbody2D>();

        enemyRigid.velocity = Vector2.zero;
    }



    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}
