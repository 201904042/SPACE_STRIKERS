using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyAttack : MonoBehaviour
{
    [HideInInspector]
    public GameObject enemyBullet;
    [HideInInspector]
    public GameObject enemyLaser;
    [HideInInspector]
    public GameObject enemySplitBullet;


    public int splitCount = 3;
    public float laserDangerZoneTime = 1;
    public float laserAttackTime = 3;
    public float defaultSpeed = 10;
    //�߻��� �Ѿ� ������, �߻��ҹ���*�ӵ�, �п��Ѿ��̶�� true

    protected virtual void Awake()
    {
        enemyBullet = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemy/EnemyProj/Enemy_Bullet.prefab");
        enemyLaser = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemy/EnemyProj/Enemy_Laser.prefab");
        enemySplitBullet = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemy/EnemyProj/Enemy_SplitBullet.prefab");
    }


    public void SingleShot(GameObject enemyBullet, Vector3 velocity, bool split = false)
    {
        GameObject enemyProj = Instantiate(enemyBullet, transform.position, Quaternion.identity);
        Rigidbody2D rigid = enemyProj.GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.zero;
        

        if (split)
        {
            enemyProj.GetComponent<EnemySplitBullet>().splitCount = splitCount;
        }
        rigid.velocity = new Vector2(velocity.x, velocity.y); //fireDirection * bulletSpeed;
    }


    //�߻��� �Ѿ� ������, �п��Ѿ��̶�� true
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

    //�߻��� �Ѿ�, �Ѿ˰���, ��ݰ�������, �Ѿ˼ӵ�, ���ؿ���, �߻�ü�� �⺻ �ޱ�, �п��Ѿ˿���
    public void MultiShot(GameObject enemyBullet, int projNum, float projAngle, float bulletSpeed = 10f, bool isAimToPlayer = false, float projBasicAngle = -180, bool split = false)
    {
        float angleStep = projAngle / projNum;
        float angleInit;
        if (projNum % 2 == 0) //¦��
        {
            angleInit = -projBasicAngle - projAngle / 2 + angleStep / 2;
        }
        else // Ȧ��
        {
            angleInit = -projBasicAngle - (angleStep * (projNum / 2));
        }

        for (int i = 0; i < projNum; i++)
        {
            float angle = angleInit + angleStep * i;
            if (isAimToPlayer)
            {
                //������ ���
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
                //������ �ƴҰ��
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

    //������ �������� ����� �ޱ�(�⺻ 0), ���ؿ���
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
    //�߻簳��, �߻� �ִ����, ���ؿ��� , �⺻�������ޱ�(�⺻ 0)
    public void MultiLaser(int projNum, float projAngle, bool isAimToPlayer = false, float projBasicAngle = 0)
    {
        float angleStep = projAngle / projNum;
        float angleInit;
        if (projNum % 2 == 0) //¦��
        {
            angleInit = projBasicAngle - projAngle / 2 + angleStep / 2;
        }
        else // Ȧ��
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


    public void enemyMoveForward(GameObject movingObject)
    {
        Vector2 moveDirection = movingObject.transform.up;
        Rigidbody2D enemyRigid = movingObject.GetComponent<Rigidbody2D>();
        EnemyObject movingObjectStat = movingObject.GetComponent<EnemyObject>();
        enemyRigid.velocity = moveDirection * movingObjectStat.enemyStat.enemyMoveSpeed;
    }

    public void enemyMoveStop(GameObject movingObject)
    {
        Rigidbody2D enemyRigid = movingObject.GetComponent<Rigidbody2D>();

        enemyRigid.velocity = Vector2.zero;
    }
}
