using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [HideInInspector]
    public GameObject enemyBullet = UnityEditor.AssetDatabase
        .LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemy/EnemyProj/Enemy_Bullet.prefab");
    [HideInInspector]
    public GameObject enemyLaser = UnityEditor.AssetDatabase
        .LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemy/EnemyProj/Enemy_Laser.prefab");
    [HideInInspector]
    public GameObject enemySplitBullet = UnityEditor.AssetDatabase
        .LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemy/EnemyProj/Enemy_SplitBullet.prefab");


    public int splitCount = 3;
    public float laserDangerZoneTime = 1;
    public float laserAttackTime = 3;
    public float defaultSpeed = 10;
    //발사할 총알 프리팹, 발사할방향*속도, 분열총알이라면 true
    public void SingleShot(GameObject enemyBullet, Vector3 velocity, bool split = false)
    {
        GameObject enemyProj = Instantiate(enemyBullet, transform.position, Quaternion.identity);
        Rigidbody2D rigid = enemyProj.GetComponent<Rigidbody2D>();
        if (split)
        {
            enemyProj.GetComponent<EnemySplitBullet>().splitCount = splitCount;
        }
        rigid.velocity = new Vector2(velocity.x, velocity.y); //fireDirection * bulletSpeed;
    }


    //발사할 총알 프리팹, 분열총알이라면 true
    public void TargetShot(GameObject enemyBullet, bool split = false)
    {
        Transform player = GameObject.FindWithTag("Player").transform;
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        if (split)
        {
            SingleShot(enemyBullet, dirToPlayer * defaultSpeed, true);
        }
        else
        {
            SingleShot(enemyBullet, dirToPlayer * defaultSpeed);
        }

    }

    //발사할 총알, 총알개수, 사격각도범위, 총알속도, 조준여부, 발사체의 기본 앵글, 분열총알여부
    public void MultiShot(GameObject enemyBullet, int projNum, float projAngle, float bulletSpeed = 10f, bool isAimToPlayer = false, float projBasicAngle = -180, bool split = false)
    {
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
                Transform player = GameObject.FindWithTag("Player").transform;
                dirToPlayer = (player.position - transform.position).normalized;

                Vector3 velocity = Quaternion.Euler(0, 0, angle) * -dirToPlayer;
                if (split)
                {
                    SingleShot(enemyBullet, velocity * bulletSpeed, true);
                }
                else
                {
                    SingleShot(enemyBullet, velocity * bulletSpeed);
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
                    SingleShot(enemyBullet, velocity, true);
                }
                else
                {
                    SingleShot(enemyBullet, velocity);
                }
            }
        }
    }

    //레이저 여러발일 경우의 앵글(기본 0), 조준여부
    public void Laser(float multiAngle = 0, bool isAimtoPlayer = false)
    {
        EnemyLaser laserObject = Instantiate(enemyLaser).GetComponent<EnemyLaser>();
        if (isAimtoPlayer)
        {
            Transform player = GameObject.FindWithTag("Player").transform;
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
}
