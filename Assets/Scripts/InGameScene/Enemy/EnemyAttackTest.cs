using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class EnemyAttackTest : MonoBehaviour
{
    public GameObject enemyBullet;
    public GameObject enemyLaser;
    public GameObject enemySplitBullet;

    [SerializeField]
    private bool isforwardShot;
    [SerializeField]
    private bool isTargetShot;
    [SerializeField]
    private bool isAimToPlayer; 
    [SerializeField]
    private bool ismultiForwardShot;

    [SerializeField]
    private int splitCount;
    [SerializeField]
    private bool isSplitShot;
    [SerializeField]
    private bool isTargetSplitShot;
    [SerializeField]
    private bool ismultiSplitShot;

    [SerializeField]
    private float laserDangerZoneTime;
    [SerializeField]
    private float laserAttackTime;
    [SerializeField]
    private bool isLaserAttack;
    [SerializeField]
    private bool isAimToPlayerLaser;
    [SerializeField]
    private bool ismultiForwardLaser;
    


    [SerializeField]
    private float defaultSpeed;
    [SerializeField]
    private int enemyProjNum;
    [SerializeField]
    private float enemyProjAngle;
    [SerializeField]
    private float defaultStraightAngle;
    

    Vector2 dirUP;
    private void Awake()
    {
        isforwardShot = false;
        ismultiForwardShot = false;
        isTargetShot = false;
        isAimToPlayer = false;

        splitCount = 3;
        isSplitShot = false;
        isTargetSplitShot = false;
        ismultiSplitShot = false;

        laserDangerZoneTime = 1;
        laserAttackTime = 3;

        isLaserAttack = false;
        isAimToPlayerLaser = false;
        ismultiForwardLaser = false;

        


        defaultSpeed = 10f;
        enemyProjNum = 1;
        enemyProjAngle = 45;
        defaultStraightAngle = -180;
        dirUP = transform.up;
    }

    private void Update()
    {
        if (isforwardShot)
        {
            SingleShot(enemyBullet , dirUP*defaultSpeed);
            isforwardShot = false;
        }
        if (isTargetShot)
        {
            TargetShot(enemyBullet);
            isTargetShot = false;
        }
        if (ismultiForwardShot)
        {
            MultiShot(enemyBullet,enemyProjNum, enemyProjAngle, defaultSpeed, isAimToPlayer, defaultStraightAngle);
            ismultiForwardShot = false;
        }

        if(isSplitShot)
        {
            SingleShot(enemySplitBullet, dirUP * defaultSpeed,true);
            isSplitShot = false;
        }
        if (isTargetSplitShot)
        {
            TargetShot(enemySplitBullet,true);
            isTargetSplitShot = false;
        }
        if (ismultiSplitShot)
        {
            MultiShot(enemySplitBullet, enemyProjNum, enemyProjAngle, defaultSpeed, isAimToPlayer, defaultStraightAngle,true);
            ismultiSplitShot = false;
        }

        if (isLaserAttack)
        {
            Laser();
            isLaserAttack = false;
        }
        if (ismultiForwardLaser)
        {
            MultiLaser(enemyProjNum, enemyProjAngle, isAimToPlayerLaser, 0);
            ismultiForwardLaser = false;
        }

    }

    private void SingleShot(GameObject enemyBullet ,Vector3 velocity ,bool split = false)
    { 
        GameObject enemyProj = Instantiate(enemyBullet, transform.position, Quaternion.identity);
        Rigidbody2D rigid = enemyProj.GetComponent<Rigidbody2D>();
        if(split)
        {
            enemyProj.GetComponent<EnemySplitBullet>().splitCount = splitCount;
        }
        rigid.velocity = new Vector2(velocity.x, velocity.y); //fireDirection * bulletSpeed;
    }

    private void TargetShot(GameObject enemyBullet, bool split = false)
    {
        Transform player = GameObject.FindWithTag("Player").transform;
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        if (split)
        {
            SingleShot(enemyBullet, dirToPlayer * defaultSpeed,true);
        }
        else
        {
            SingleShot(enemyBullet, dirToPlayer * defaultSpeed);
        }
        
    }

    //projNum�� �߻��ϰ� ���� �߻�ü�� ����
    //projAngle �߻��� ���� 45, 90 , 180, 360
    //projBasicAngle�� -180�϶� default�� �Ѿ��� �Ʒ��� �߻�� Ŀ������ �����߻� �������� �������� �߻�
    private void MultiShot(GameObject enemyBullet, int projNum, float projAngle, float bulletSpeed = 10f, bool isAimToPlayer = false, float projBasicAngle = -180, bool split = false)
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

    private void Laser(float multiAngle = 0, bool isAimtoPlayer = false)
    {
        EnemyLaser laserObject = Instantiate(enemyLaser).GetComponent<EnemyLaser>();
        if(isAimtoPlayer)
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

    private void MultiLaser(int projNum, float projAngle, bool isAimToPlayer = false, float projBasicAngle =0)
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
                Laser(angle , isAimToPlayer);
            }
            else
            {
                Laser(angle, isAimToPlayer);
            }
        }
    }


}
