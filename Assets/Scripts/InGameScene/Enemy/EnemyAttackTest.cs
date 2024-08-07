using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class EnemyAttackTest : MonoBehaviour
{
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
            SingleShot(dirUP*defaultSpeed);
            isforwardShot = false;
        }
        if (isTargetShot)
        {
            TargetShot();
            isTargetShot = false;
        }
        if (ismultiForwardShot)
        {
            MultiShot(enemyProjNum, enemyProjAngle, defaultSpeed, isAimToPlayer, defaultStraightAngle);
            ismultiForwardShot = false;
        }

        if(isSplitShot)
        {
            SingleShot(dirUP * defaultSpeed,true);
            isSplitShot = false;
        }
        if (isTargetSplitShot)
        {
            TargetShot(true);
            isTargetSplitShot = false;
        }
        if (ismultiSplitShot)
        {
            MultiShot(enemyProjNum, enemyProjAngle, defaultSpeed, isAimToPlayer, defaultStraightAngle,true);
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

    private void SingleShot(Vector3 velocity ,bool split = false)
    { 
        GameObject enemyProj = ObjectPool.poolInstance.GetProj(ProjType.Enemy_Bullet, transform.position, Quaternion.identity);
        Rigidbody2D rigid = enemyProj.GetComponent<Rigidbody2D>();
        if(split)
        {
            enemyProj.GetComponent<EnemySplitBullet>().splitCount = splitCount;
        }
        rigid.velocity = new Vector2(velocity.x, velocity.y); //fireDirection * bulletSpeed;
    }

    private void TargetShot(bool split = false)
    {
        Transform player = GameManager.gameInstance.myPlayer.transform;
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        if (split)
        {
            SingleShot(dirToPlayer * defaultSpeed,true);
        }
        else
        {
            SingleShot(dirToPlayer * defaultSpeed);
        }
        
    }

    //projNum은 발사하고 싶은 발사체의 개수
    //projAngle 발사할 각도 45, 90 , 180, 360
    //projBasicAngle가 -180일때 default로 총알이 아래로 발사됨 커질수록 우측발사 작을수록 좌측으로 발사
    private void MultiShot(int projNum, float projAngle, float bulletSpeed = 10f, bool isAimToPlayer = false, float projBasicAngle = -180, bool split = false)
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
                Transform player = GameManager.gameInstance.myPlayer.transform;
                dirToPlayer = (player.position - transform.position).normalized;

                Vector3 velocity = Quaternion.Euler(0, 0, angle) * -dirToPlayer;
                if (split)
                {
                    SingleShot(velocity * bulletSpeed, true);
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
                    SingleShot(velocity, true);
                }
                else
                {
                    SingleShot( velocity);
                }
            }
        }
    }

    private void Laser(float multiAngle = 0, bool isAimtoPlayer = false)
    {
        EnemyLaser laserObject = ObjectPool.poolInstance.GetProj(ProjType.Enemy_Laser,transform.position, transform.rotation).GetComponent<EnemyLaser>();
        if(isAimtoPlayer)
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

    private void MultiLaser(int projNum, float projAngle, bool isAimToPlayer = false, float projBasicAngle =0)
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
                Laser(angle , isAimToPlayer);
            }
            else
            {
                Laser(angle, isAimToPlayer);
            }
        }
    }


}
